namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using System;
using System.IO;

public sealed class StableOsuDatabaseWriter
{
    private static readonly int[] _osuModsDbOrder = [0, 64, 256, 2, 66, 258, 16, 80, 272];

    public static void WriteDatabase(StableOsuDatabaseData stableOsuDatabase, string filePath)
    {
        if (stableOsuDatabase is null)
        {
            throw new ArgumentNullException(nameof(stableOsuDatabase), "stableOsuDatabase cannot be null.");
        }

        if (!stableOsuDatabase.IsValid)
        {
            throw new InvalidStableOsuDatabaseException("Provided metadata would create unreadable database.");
        }

        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
        WriteDatabase(stableOsuDatabase, fileStream);
    }

    public static void WriteDatabase(StableOsuDatabaseData stableOsuDatabase, Stream outputStream)
    {
        if (stableOsuDatabase is null)
        {
            throw new ArgumentNullException(nameof(stableOsuDatabase), "stableOsuDatabase cannot be null.");
        }

        if (!stableOsuDatabase.IsValid)
        {
            throw new InvalidStableOsuDatabaseException("Provided metadata would create unreadable database.");
        }

        using OsuBinaryWriter binaryWriter = new(outputStream);

        WriteDatabaseHeader(binaryWriter, stableOsuDatabase);

        foreach (Beatmap beatmap in stableOsuDatabase.Beatmaps)
        {
            WriteBeatmap(beatmap, binaryWriter);
        }

        binaryWriter.Write(stableOsuDatabase.Permissions);
    }

    private static void WriteDatabaseHeader(OsuBinaryWriter binaryWriter, StableOsuDatabaseData stableOsuDatabase)
    {
        binaryWriter.Write(stableOsuDatabase.FileDate);
        binaryWriter.Write(stableOsuDatabase.FolderCount);
        binaryWriter.Write(stableOsuDatabase.AccountUnlocked);
        binaryWriter.Write(stableOsuDatabase.UnlockDate.ToBinary());
        binaryWriter.Write(stableOsuDatabase.Username);
        binaryWriter.Write(stableOsuDatabase.NumberOfBeatmaps);
    }

    private static void WriteBeatmap(Beatmap beatmap, OsuBinaryWriter binaryWriter)
    {
        binaryWriter.Write(beatmap.ArtistRoman);
        binaryWriter.Write(beatmap.ArtistUnicode);
        binaryWriter.Write(beatmap.TitleRoman);
        binaryWriter.Write(beatmap.TitleUnicode);
        binaryWriter.Write(beatmap.Creator);
        binaryWriter.Write(beatmap.DiffName);
        binaryWriter.Write(beatmap.Mp3Name);
        binaryWriter.Write(beatmap.Md5);
        binaryWriter.Write(beatmap.OsuFileName);
        binaryWriter.Write(beatmap.State);
        binaryWriter.Write(beatmap.Circles);
        binaryWriter.Write(beatmap.Sliders);
        binaryWriter.Write(beatmap.Spinners);
        binaryWriter.Write(beatmap.EditDate);
        binaryWriter.Write(beatmap.ApproachRate);
        binaryWriter.Write(beatmap.CircleSize);
        binaryWriter.Write(beatmap.HpDrainRate);
        binaryWriter.Write(beatmap.OverallDifficulty);
        binaryWriter.Write(beatmap.SliderVelocity);

        const int playModeCount = 4;
        for (int playMode = 0; playMode < playModeCount; playMode++)
        {
            WriteStars(beatmap.ModPpStars[(PlayMode)playMode], binaryWriter);
        }

        binaryWriter.Write(beatmap.DrainingTime);
        binaryWriter.Write(beatmap.TotalTime);
        binaryWriter.Write(beatmap.PreviewTime);

        WriteTimingPoints(beatmap.TimingPoints, binaryWriter);

        binaryWriter.Write(beatmap.MapId);
        binaryWriter.Write(beatmap.MapSetId);
        binaryWriter.Write(beatmap.ThreadId);
        binaryWriter.Write((byte)beatmap.OsuGrade);
        binaryWriter.Write((byte)beatmap.TaikoGrade);
        binaryWriter.Write((byte)beatmap.CatchGrade);
        binaryWriter.Write((byte)beatmap.ManiaGrade);
        binaryWriter.Write((short)beatmap.Offset);
        binaryWriter.Write(beatmap.StackLeniency);
        binaryWriter.Write((byte)beatmap.PlayMode);
        binaryWriter.Write(beatmap.Source);
        binaryWriter.Write(beatmap.Tags);
        binaryWriter.Write(beatmap.AudioOffset);
        binaryWriter.Write(beatmap.LetterBox);
        binaryWriter.Write(!beatmap.Played);
        binaryWriter.Write(beatmap.LastPlayed);
        binaryWriter.Write(beatmap.IsOsz2);
        binaryWriter.Write(beatmap.Dir);
        binaryWriter.Write(beatmap.LastSync);
        binaryWriter.Write(beatmap.DisableHitsounds);
        binaryWriter.Write(beatmap.DisableSkin);
        binaryWriter.Write(beatmap.DisableSb);
        binaryWriter.Write(beatmap.DisableVideo);
        binaryWriter.Write(beatmap.VisualOverride);
        binaryWriter.Write(beatmap.LastModification);
        binaryWriter.Write(beatmap.ManiaScrollSpeed);
    }

    private static void WriteStars(StarRating starRating, OsuBinaryWriter binaryWriter)
    {
        if (starRating is null || !starRating.Any())
        {
            binaryWriter.Write(0);
            return;
        }

        binaryWriter.Write(starRating.Count());

        foreach (int modNumber in _osuModsDbOrder)
        {
            if (starRating.ContainsKey(modNumber))
            {
                WriteStarRating(binaryWriter, modNumber, starRating[modNumber]);
            }
        }

        // & leftovers in case we have made some manual changes
        foreach (KeyValuePair<int, float> kvp in starRating)
        {
            if (!_osuModsDbOrder.Contains(kvp.Key))
            {
                WriteStarRating(binaryWriter, kvp.Key, kvp.Value);
            }
        }

        static void WriteStarRating(OsuBinaryWriter binaryWriter, int modNumber, float stars)
        {
            binaryWriter.Write((byte)8);
            binaryWriter.Write(modNumber);
            binaryWriter.Write((byte)12);
            binaryWriter.Write(stars);
        }
    }

    private static void WriteTimingPoints(TimingPoint[] timingPoints, OsuBinaryWriter binaryWriter)
    {
        if (timingPoints is null)
        {
            binaryWriter.Write(-1);
            return;
        }

        binaryWriter.Write(timingPoints.Length);

        foreach (TimingPoint tp in timingPoints)
        {
            binaryWriter.Write(tp.BpmDuration);
            binaryWriter.Write(tp.Offset);
            binaryWriter.Write(tp.InheritsBpm);
        }
    }
}