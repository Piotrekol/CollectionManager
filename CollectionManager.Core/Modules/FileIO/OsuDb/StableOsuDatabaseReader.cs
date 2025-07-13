namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using System;
using System.IO;
using System.Threading;

public sealed class StableOsuDatabaseReader
{
    public const int LatestOsuDbVersion = 20191105;

    public static StableOsuDatabaseData ReadDatabase(string filePath, CancellationToken cancellationToken, IProgress<string> progress = null)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified osu! database file does not exist.", filePath);
        }

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return ReadDatabase(fileStream, cancellationToken, progress);
    }

    public static StableOsuDatabaseData ReadDatabase(Stream inputStream, CancellationToken cancellationToken, IProgress<string> progress = null)
    {
        using OsuBinaryReader binaryReader = new(inputStream);

        StableOsuDatabaseData stableDatabaseData = ReadDatabaseHeader(binaryReader);

        if (stableDatabaseData.FolderCount <= 0 || stableDatabaseData.NumberOfBeatmaps <= 0)
        {
            return stableDatabaseData;
        }

        ReadAllBeatmaps(stableDatabaseData, binaryReader, cancellationToken, progress);

        int permissions = binaryReader.ReadInt32();

        return stableDatabaseData with { Permissions = permissions };
    }

    private static void ReadAllBeatmaps(StableOsuDatabaseData databaseHeader, OsuBinaryReader binaryReader, CancellationToken cancellationToken, IProgress<string> progress = null)
    {
        int numberOfLoadedBeatmaps = 0;
        for (; numberOfLoadedBeatmaps < databaseHeader.NumberOfBeatmaps; numberOfLoadedBeatmaps++)
        {
            if (numberOfLoadedBeatmaps % 100 == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                progress?.Report($"Loading {numberOfLoadedBeatmaps} of {databaseHeader.NumberOfBeatmaps} beatmaps ({databaseHeader.FolderCount} beatmap sets)");

            }

            databaseHeader.Beatmaps.Add(ReadNextBeatmap(binaryReader));
        }

        progress?.Report($"Loaded {numberOfLoadedBeatmaps} of {databaseHeader.NumberOfBeatmaps} beatmaps ({databaseHeader.FolderCount} beatmap sets)");

    }

    private static BeatmapExtension ReadNextBeatmap(OsuBinaryReader binaryReader)
    {
#pragma warning disable IDE0017 // Simplify object initialization
        BeatmapExtension beatmap = new();
#pragma warning restore IDE0017 // Simplify object initialization

        // Header
        beatmap.ArtistRoman = binaryReader.ReadString();
        beatmap.ArtistUnicode = binaryReader.ReadString();
        beatmap.TitleRoman = binaryReader.ReadString();
        beatmap.TitleUnicode = binaryReader.ReadString();
        beatmap.Creator = binaryReader.ReadString();
        beatmap.DiffName = binaryReader.ReadString();
        beatmap.Mp3Name = binaryReader.ReadString();
        beatmap.Md5 = binaryReader.ReadString();
        beatmap.OsuFileName = binaryReader.ReadString();
        beatmap.State = binaryReader.ReadByte();
        beatmap.Circles = binaryReader.ReadInt16();
        beatmap.Sliders = binaryReader.ReadInt16();
        beatmap.Spinners = binaryReader.ReadInt16();
        beatmap.EditDate = binaryReader.ReadDateTime();
        beatmap.ApproachRate = binaryReader.ReadSingle();
        beatmap.CircleSize = binaryReader.ReadSingle();
        beatmap.HpDrainRate = binaryReader.ReadSingle();
        beatmap.OverallDifficulty = binaryReader.ReadSingle();
        beatmap.SliderVelocity = binaryReader.ReadDouble();

        const int playModeCount = 4;
        for (int playMode = 0; playMode < playModeCount; playMode++)
        {
            StarRating stars = ReadStars(binaryReader);
            beatmap.ModPpStars[(PlayMode)playMode] = stars;
        }

        beatmap.DrainingTime = binaryReader.ReadInt32();
        beatmap.TotalTime = binaryReader.ReadInt32();
        beatmap.PreviewTime = binaryReader.ReadInt32();

        beatmap.TimingPoints = ReadTimingPoints(binaryReader, beatmap);
        if (beatmap.TimingPoints is not null)
        {
            (beatmap.MinBpm, beatmap.MaxBpm, beatmap.MainBpm) = CalculateBpmFromTimingPoints(beatmap.TimingPoints, beatmap.TotalTime);
        }

        beatmap.MapId = binaryReader.ReadInt32();
        beatmap.MapSetId = binaryReader.ReadInt32();
        beatmap.ThreadId = binaryReader.ReadInt32();
        beatmap.OsuGrade = (OsuGrade)binaryReader.ReadByte();
        beatmap.TaikoGrade = (OsuGrade)binaryReader.ReadByte();
        beatmap.CatchGrade = (OsuGrade)binaryReader.ReadByte();
        beatmap.ManiaGrade = (OsuGrade)binaryReader.ReadByte();
        beatmap.Offset = binaryReader.ReadInt16();
        beatmap.StackLeniency = binaryReader.ReadSingle();
        beatmap.PlayMode = (PlayMode)binaryReader.ReadByte();
        beatmap.Source = binaryReader.ReadString();
        beatmap.Tags = binaryReader.ReadString();
        beatmap.AudioOffset = binaryReader.ReadInt16();
        beatmap.LetterBox = binaryReader.ReadString();
        beatmap.Played = !binaryReader.ReadBoolean();
        beatmap.LastPlayed = binaryReader.ReadDateTime();
        beatmap.IsOsz2 = binaryReader.ReadBoolean();
        beatmap.Dir = binaryReader.ReadString();
        beatmap.LastSync = binaryReader.ReadDateTime();
        beatmap.DisableHitsounds = binaryReader.ReadBoolean();
        beatmap.DisableSkin = binaryReader.ReadBoolean();
        beatmap.DisableSb = binaryReader.ReadBoolean();
        beatmap.DisableVideo = binaryReader.ReadBoolean();
        beatmap.VisualOverride = binaryReader.ReadBoolean();
        beatmap.LastModification = binaryReader.ReadInt32();
        beatmap.ManiaScrollSpeed = binaryReader.ReadByte();

        return beatmap;
    }

    private static TimingPoint[] ReadTimingPoints(OsuBinaryReader binaryReader, Beatmap beatmap)
    {
        int amountOfTimingPoints = binaryReader.ReadInt32();

        if (amountOfTimingPoints < 0)
        {
            return null;
        }

        TimingPoint[] timingPoints = new TimingPoint[amountOfTimingPoints];

        for (int i = 0; i < amountOfTimingPoints; i++)
        {
            timingPoints[i] = new TimingPoint(binaryReader.ReadDouble(), binaryReader.ReadDouble(), binaryReader.ReadBoolean());
        }

        return timingPoints;
    }

    private static StarRating ReadStars(OsuBinaryReader binaryReader)
    {
        int combinationsCount = binaryReader.ReadInt32();

        if (combinationsCount <= 0)
        {
            return [];
        }

        StarRating starRating = [];

        for (int j = 0; j < combinationsCount; j++)
        {
            int modEnum = (int)binaryReader.OsuConditionalRead();
            float stars = (float)binaryReader.OsuConditionalRead();

            if (!starRating.ContainsKey(modEnum))
            {
                starRating.Add(modEnum, stars);
            }
            else
            {
                if (starRating[modEnum] < stars)
                {
                    starRating[modEnum] = stars;
                }
            }
        }

        return starRating;
    }

    private static StableOsuDatabaseData ReadDatabaseHeader(OsuBinaryReader binaryReader)
    {
        int fileDate = binaryReader.ReadInt32();

        if (fileDate < LatestOsuDbVersion)
        {
            throw new InvalidStableOsuDatabaseException("osu! stable database is outdated. Please update your osu! client to version after 20191105.");
        }

        int folderCount = binaryReader.ReadInt32();
        bool accountUnlocked = binaryReader.ReadBoolean();
        DateTime unlockDate = DateTime.FromBinary(binaryReader.ReadInt64());
        string username = binaryReader.ReadString();
        int numberOfBeatmaps = binaryReader.ReadInt32();

        return new StableOsuDatabaseData(
            fileDate,
            folderCount,
            accountUnlocked,
            unlockDate,
            username,
            numberOfBeatmaps,
            [],
            -1);
    }

    public static (double Min, double Max, double Main) CalculateBpmFromTimingPoints(TimingPoint[] timingPoints, int totalBeatmapTime)
    {
        double minBpm = double.MinValue,
            maxBpm = double.MaxValue,
            currentBpmLength = 0,
            lastTime = totalBeatmapTime;
        Dictionary<double, int> bpmTimes = [];

        for (int i = timingPoints.Length - 1; i >= 0; i--)
        {
            TimingPoint tp = timingPoints[i];

            if (tp.InheritsBpm)
            {
                currentBpmLength = tp.BpmDuration;
            }

            if (currentBpmLength == 0 || tp.Offset > lastTime || (!tp.InheritsBpm && i > 0))
            {
                continue;
            }

            if (currentBpmLength > minBpm)
            {
                minBpm = currentBpmLength;
            }

            if (currentBpmLength < maxBpm)
            {
                maxBpm = currentBpmLength;
            }

            if (!bpmTimes.ContainsKey(currentBpmLength))
            {
                bpmTimes[currentBpmLength] = 0;
            }

            bpmTimes[currentBpmLength] += (int)(lastTime - (i == 0 ? 0 : tp.Offset));

            lastTime = tp.Offset;
        }

        maxBpm = Math.Round(60000 / maxBpm);
        minBpm = Math.Round(60000 / minBpm);
        double mainBpm = 0;

        if (Math.Abs(maxBpm - minBpm) < double.Epsilon)
        {
            mainBpm = maxBpm;
        }
        else if (bpmTimes.Count != 0)
        {
            mainBpm = Math.Round(60000 / bpmTimes.Aggregate((i1, i2) => i1.Value > i2.Value ? i1 : i2).Key);
        }

        return (minBpm, maxBpm, mainBpm);
    }
}
