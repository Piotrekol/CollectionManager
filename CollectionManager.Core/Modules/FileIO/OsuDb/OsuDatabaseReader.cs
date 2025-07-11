namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

//TODO: refactor to allow for read/write access.
public class OsuDatabaseLoader : IDisposable
{
    private readonly IMapDataManager _mapDataStorer;
    private BinaryReader _binaryReader;
    private readonly Beatmap _tempBeatmap;

    public bool LoadedSuccessfully { get; private set; }

    public int MapsWithNoId { get; private set; }
    public string Username { get; private set; }
    public int FileDate { get; private set; }
    public int ExpectedNumberOfMapSets { get; private set; }
    public int ExpectedNumOfBeatmaps { get; private set; } = -1;
    public int NumberOfLoadedBeatmaps { get; private set; }

    public OsuDatabaseLoader(IMapDataManager mapDataStorer, Beatmap tempBeatmap)
    {
        _tempBeatmap = tempBeatmap;
        _mapDataStorer = mapDataStorer;
    }

    public virtual void LoadDatabase(string fullOsuDbPath, IProgress<string> progress, CancellationToken cancellationToken)
    {
        if (FileExists(fullOsuDbPath))
        {
            using FileStream fileStream = new(fullOsuDbPath, FileMode.Open, FileAccess.Read);

            try
            {
                _binaryReader = new BinaryReader(fileStream);
                if (DatabaseContainsData(progress))
                {
                    ReadDatabaseEntries(progress, cancellationToken);
                }

            }
            finally
            {
                _binaryReader.Dispose();
            }

        }

        GC.Collect();
    }

    protected virtual void ReadDatabaseEntries(IProgress<string> progress, CancellationToken cancellationToken)
    {
        progress?.Report($"Starting load of {ExpectedNumOfBeatmaps} beatmaps");

        _mapDataStorer.StartMassStoring();
        for (NumberOfLoadedBeatmaps = 0; NumberOfLoadedBeatmaps < ExpectedNumOfBeatmaps; NumberOfLoadedBeatmaps++)
        {
            if (NumberOfLoadedBeatmaps % 100 == 0)
            {
                progress?.Report($"Loading {NumberOfLoadedBeatmaps} of {ExpectedNumOfBeatmaps}");
            }

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            ReadNextBeatmap();
        }

        progress?.Report($"Loaded {NumberOfLoadedBeatmaps} beatmaps");
        _mapDataStorer.EndMassStoring();
        if (!cancellationToken.IsCancellationRequested)
        {
            LoadedSuccessfully = true;
        }
    }
    private void ReadNextBeatmap()
    {
        _tempBeatmap.InitEmptyValues();

        ReadMapHeader(_tempBeatmap);
        ReadMapInfo(_tempBeatmap);
        ReadTimingPoints(_tempBeatmap);
        ReadMapMetaData(_tempBeatmap);

        _mapDataStorer.StoreBeatmap(_tempBeatmap);
    }

    private void ReadMapMetaData(Beatmap beatmap)
    {
        beatmap.MapId = Math.Abs(_binaryReader.ReadInt32());
        if (beatmap.MapId == 0)
        {
            MapsWithNoId++;
        }

        beatmap.MapSetId = Math.Abs(_binaryReader.ReadInt32());
        beatmap.ThreadId = Math.Abs(_binaryReader.ReadInt32());

        beatmap.OsuGrade = (OsuGrade)_binaryReader.ReadByte();
        beatmap.TaikoGrade = (OsuGrade)_binaryReader.ReadByte();
        beatmap.CatchGrade = (OsuGrade)_binaryReader.ReadByte();
        beatmap.ManiaGrade = (OsuGrade)_binaryReader.ReadByte();

        beatmap.Offset = _binaryReader.ReadInt16();
        beatmap.StackLeniency = _binaryReader.ReadSingle();
        beatmap.PlayMode = (PlayMode)_binaryReader.ReadByte();
        beatmap.Source = ReadString();
        beatmap.Tags = ReadString();
        beatmap.AudioOffset = _binaryReader.ReadInt16();
        beatmap.LetterBox = ReadString();
        beatmap.Played = !_binaryReader.ReadBoolean();
        beatmap.LastPlayed = GetDate();
        beatmap.IsOsz2 = _binaryReader.ReadBoolean();
        beatmap.Dir = ReadString();
        beatmap.LastSync = GetDate();
        beatmap.DisableHitsounds = _binaryReader.ReadBoolean();
        beatmap.DisableSkin = _binaryReader.ReadBoolean();
        beatmap.DisableSb = _binaryReader.ReadBoolean();
        _ = _binaryReader.ReadBoolean();
        _ = _binaryReader.ReadBoolean();
        if (FileDate < 20140609)
        {
            beatmap.BgDim = _binaryReader.ReadInt16();
        }
        //bytes not analysed.
        _ = _binaryReader.ReadInt32();
        _ = _binaryReader.ReadByte();
    }

    private class TimingPoint
    {
        public bool InheritsBpm;
        public double Offset;
        public double BpmDuration;

        public TimingPoint(double bpmDuration, double offset, bool inheritsBpm)
        {
            Offset = offset;
            BpmDuration = bpmDuration;
            InheritsBpm = inheritsBpm;
        }
    }
    private void ReadTimingPoints(Beatmap beatmap)
    {
        int amountOfTimingPoints = _binaryReader.ReadInt32();

        List<TimingPoint> timingPoints = new(amountOfTimingPoints);
        for (int i = 0; i < amountOfTimingPoints; i++)
        {
            timingPoints.Add(new TimingPoint(_binaryReader.ReadDouble(), _binaryReader.ReadDouble(), _binaryReader.ReadBoolean()));
        }

        double minBpm = double.MinValue,
            maxBpm = double.MaxValue,
            currentBpmLength = 0,
            lastTime = beatmap.TotalTime;
        Dictionary<double, int> bpmTimes = [];
        for (int i = timingPoints.Count - 1; i >= 0; i--)
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

        beatmap.MaxBpm = Math.Round(60000 / maxBpm);
        beatmap.MinBpm = Math.Round(60000 / minBpm);

        if (Math.Abs(beatmap.MaxBpm - beatmap.MinBpm) < double.Epsilon)
        {
            beatmap.MainBpm = beatmap.MaxBpm;
        }
        else if (bpmTimes.Count != 0)
        {
            beatmap.MainBpm = Math.Round(60000 / bpmTimes.Aggregate((i1, i2) => i1.Value > i2.Value ? i1 : i2).Key);
        }
    }
    private void ReadMapInfo(Beatmap beatmap)
    {
        beatmap.State = _binaryReader.ReadByte();
        beatmap.Circles = _binaryReader.ReadInt16();
        beatmap.Sliders = _binaryReader.ReadInt16();
        beatmap.Spinners = _binaryReader.ReadInt16();
        beatmap.EditDate = GetDate();
        beatmap.ApproachRate = _binaryReader.ReadSingle();
        beatmap.CircleSize = _binaryReader.ReadSingle();
        beatmap.HpDrainRate = _binaryReader.ReadSingle();
        beatmap.OverallDifficulty = _binaryReader.ReadSingle();
        beatmap.SliderVelocity = _binaryReader.ReadDouble();

        for (int j = 0; j < 4; j++)
        {
            ReadStarsData(beatmap, (PlayMode)j);
        }

        beatmap.DrainingTime = _binaryReader.ReadInt32();
        beatmap.TotalTime = _binaryReader.ReadInt32();
        beatmap.PreviewTime = _binaryReader.ReadInt32();
    }

    private void ReadStarsData(Beatmap beatmap, PlayMode playMode)
    {
        int num = _binaryReader.ReadInt32();
        if (num <= 0)
        {
            return;
        }

        if (!beatmap.ModPpStars.ContainsKey(playMode))
        {
            beatmap.ModPpStars.Add(playMode, []);
        }

        StarRating modPpStars = beatmap.ModPpStars[playMode];
        for (int j = 0; j < num; j++)
        {
            int modEnum = (int)ConditionalRead();
            float stars = (float)ConditionalRead();

            if (!modPpStars.ContainsKey(modEnum))
            {
                modPpStars.Add(modEnum, stars);
            }
            else
            {
                float star = stars;
                if (modPpStars[modEnum] < star)
                {
                    modPpStars[modEnum] = star;
                }
            }
        }
    }
    private object ConditionalRead()
    {
        switch (_binaryReader.ReadByte())
        {
            case 1:
            {
                return _binaryReader.ReadBoolean();
            }
            case 2:
            {
                return _binaryReader.ReadByte();
            }
            case 3:
            {
                return _binaryReader.ReadUInt16();
            }
            case 4:
            {
                return _binaryReader.ReadUInt32();
            }
            case 5:
            {
                return _binaryReader.ReadUInt64();
            }
            case 6:
            {
                return _binaryReader.ReadSByte();
            }
            case 7:
            {
                return _binaryReader.ReadInt16();
            }
            case 8:
            {
                return _binaryReader.ReadInt32();
            }
            case 9:
            {
                return _binaryReader.ReadInt64();
            }
            case 10:
            {
                return _binaryReader.ReadChar();
            }
            case 11:
            {
                return _binaryReader.ReadString();
            }
            case 12:
            {
                return _binaryReader.ReadSingle();
            }
            case 13:
            {
                return _binaryReader.ReadDouble();
            }
            case 14:
            {
                return _binaryReader.ReadDecimal();
            }
            case 15:
            {
                return GetDate();
            }
            case 16:
            {
                int num = _binaryReader.ReadInt32();
                return num > 0 ? _binaryReader.ReadBytes(num) : num < 0 ? null : (object)Array.Empty<byte>();

            }
            case 17:
            {
                int num = _binaryReader.ReadInt32();
                return num > 0 ? _binaryReader.ReadChars(num) : num < 0 ? null : (object)Array.Empty<char>();
            }
            case 18:
            {
                throw new NotImplementedException("Unused in db.");
            }
            default:
            {
                return null;
            }
        }
    }
    private DateTime GetDate()
    {
        long ticks = _binaryReader.ReadInt64();
        return ticks < 0L || ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks
            ? DateTime.MinValue
            : new DateTime(ticks, DateTimeKind.Utc);
    }
    private void ReadMapHeader(Beatmap beatmap)
    {
        beatmap.ArtistRoman = ReadString().Trim();
        beatmap.ArtistUnicode = ReadString().Trim();
        beatmap.TitleRoman = ReadString().Trim();
        beatmap.TitleUnicode = ReadString().Trim();
        beatmap.Creator = ReadString().Trim();
        beatmap.DiffName = ReadString().Trim();
        beatmap.Mp3Name = ReadString().Trim();
        beatmap.Md5 = ReadString().Trim();
        beatmap.OsuFileName = ReadString().Trim();

    }
    private string ReadString() => _binaryReader.ReadByte() == 11 ? _binaryReader.ReadString() : "";
    private bool DatabaseContainsData(IProgress<string> progress)
    {
        FileDate = _binaryReader.ReadInt32();
        if (FileDate < 20191105)
        {
            progress?.Report($"Outdated osu!.db version ({FileDate}). Load failed.");
            return false;
        }

        ExpectedNumberOfMapSets = _binaryReader.ReadInt32();
        progress?.Report($"Expected number of mapSets: {ExpectedNumberOfMapSets}");
        try
        {
            bool something = _binaryReader.ReadBoolean();
            DateTime a = GetDate().ToLocalTime();
            _ = _binaryReader.BaseStream.Seek(1, SeekOrigin.Current);
            Username = _binaryReader.ReadString();
            ExpectedNumOfBeatmaps = _binaryReader.ReadInt32();
            progress?.Report($"Expected number of beatmaps: {ExpectedNumOfBeatmaps}");

            if (ExpectedNumOfBeatmaps < 0)
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    protected virtual bool FileExists(string fullPath) => !string.IsNullOrEmpty(fullPath) && File.Exists(fullPath);

    public void Dispose()
    {
        _binaryReader?.Dispose();
        GC.SuppressFinalize(this);
    }
}
