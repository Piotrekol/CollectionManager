#define GetStarsCombinations
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManager.Modules.FileIO.OsuDb
{
    //TODO: refactor to allow for read/write access.
    public class OsuDatabaseLoader
    {
        private readonly ILogger _logger;
        private readonly IMapDataManager _mapDataStorer;
        private FileStream _fileStream;
        private BinaryReader _binaryReader;
        private readonly Beatmap _tempBeatmap;

        public bool LoadedSuccessfully { get; private set; }

        public int MapsWithNoId { get; private set; }
        public string Username { get; private set; }
        public int FileDate { get; private set; }
        public int ExpectedNumberOfMapSets { get; private set; }
        public int ExpectedNumOfBeatmaps { get; private set; } = -1;
        public int NumberOfLoadedBeatmaps { get; private set; }

        public OsuDatabaseLoader(ILogger logger, IMapDataManager mapDataStorer, Beatmap tempBeatmap)
        {
            _tempBeatmap = tempBeatmap;
            _mapDataStorer = mapDataStorer;
            _logger = logger;
        }

        public virtual void LoadDatabase(string fullOsuDbPath)
        {
            if (FileExists(fullOsuDbPath))
            {
                _fileStream = new FileStream(fullOsuDbPath, FileMode.Open, FileAccess.Read);
                _binaryReader = new BinaryReader(_fileStream);
                if (DatabaseContainsData())
                {
                    ReadDatabaseEntries();
                }
                DestoryReader();
            }
            GC.Collect();
        }

        protected virtual void ReadDatabaseEntries()
        {
            _logger?.Log("Starting MassStoring of {0} beatmaps", ExpectedNumOfBeatmaps.ToString());

            _mapDataStorer.StartMassStoring();
            for (NumberOfLoadedBeatmaps = 0; NumberOfLoadedBeatmaps < ExpectedNumOfBeatmaps; NumberOfLoadedBeatmaps++)
            {
                if (NumberOfLoadedBeatmaps % 100 == 0)
                    _logger?.Log("Loading {0} of {1}", NumberOfLoadedBeatmaps.ToString(),
                    ExpectedNumOfBeatmaps.ToString());

                ReadNextBeatmap();
            }
            _logger?.Log("Loaded {0} beatmaps", NumberOfLoadedBeatmaps.ToString());
            _mapDataStorer.EndMassStoring();
            LoadedSuccessfully = true;
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
            if (beatmap.MapId == 0) MapsWithNoId++;

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
            _binaryReader.ReadBoolean();
            _binaryReader.ReadBoolean();
            if (FileDate < 20140609)
                beatmap.BgDim = (short)_binaryReader.ReadInt16();
            //bytes not analysed.
            _binaryReader.ReadInt32();
            _binaryReader.ReadByte();
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

            List<TimingPoint> timingPoints = new List<TimingPoint>(amountOfTimingPoints);
            for (int i = 0; i < amountOfTimingPoints; i++)
            {
                timingPoints.Add(new TimingPoint(_binaryReader.ReadDouble(), _binaryReader.ReadDouble(), _binaryReader.ReadBoolean()));
            }

            double minBpm = double.MaxValue,
                maxBpm = double.MinValue,
                currentBpmLength = 0,
                lastTime = beatmap.TotalTime;
            Dictionary<double, int> bpmTimes = new Dictionary<double, int>();
            for (var i = timingPoints.Count - 1; i >= 0; i--)
            {
                var tp = timingPoints[i];

                if (tp.InheritsBpm)
                    currentBpmLength = tp.BpmDuration;

                if (currentBpmLength == 0 || tp.Offset > lastTime || (!tp.InheritsBpm && i > 0))
                    continue;

                if (currentBpmLength < minBpm)
                    minBpm = currentBpmLength;

                if (currentBpmLength > maxBpm)
                    maxBpm = currentBpmLength;

                if (!bpmTimes.ContainsKey(currentBpmLength))
                    bpmTimes[currentBpmLength] = 0;

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
                beatmap.ModPpStars.Add(playMode, new StarRating());
            var modPpStars = beatmap.ModPpStars[playMode];
            for (int j = 0; j < num; j++)
            {
                int modEnum = (int)ConditionalRead();
                Double stars = (Double)ConditionalRead();
                if (!modPpStars.ContainsKey(modEnum))
                {
                    modPpStars.Add(modEnum, Math.Round(stars, 2));
                }
                else
                {
                    var star = Math.Round(stars, 2);
                    if (modPpStars[modEnum] < star)
                        modPpStars[modEnum] = star;
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
                        if (num > 0)
                        {
                            return _binaryReader.ReadBytes(num);
                        }
                        if (num < 0)
                        {
                            return null;
                        }
                        return new byte[0];

                    }
                case 17:
                    {
                        int num = _binaryReader.ReadInt32();
                        if (num > 0)
                        {
                            return _binaryReader.ReadChars(num);
                        }
                        if (num < 0)
                        {
                            return null;
                        }
                        return new char[0];
                    }
                case 18:
                    {
                        throw new NotImplementedException();
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
            if (ticks < 0L || ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks)
            {
                return DateTime.MinValue;
            }

            return new DateTime(ticks, DateTimeKind.Utc);
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
        private string ReadString()
        {
            if (_binaryReader.ReadByte() == 11)
            {
                return _binaryReader.ReadString();
            }
            return "";
        }
        private bool DatabaseContainsData()
        {
            FileDate = _binaryReader.ReadInt32();
            if (FileDate < 20191105)
            {
                _logger?.Log(string.Format("Outdated osu!.db version({0}). Load failed.", FileDate.ToString()));
                return false;
            }
            ExpectedNumberOfMapSets = _binaryReader.ReadInt32();
            _logger?.Log(string.Format("Expected number of mapSets: {0}", ExpectedNumberOfMapSets));
            try
            {
                bool something = _binaryReader.ReadBoolean();
                DateTime a = GetDate().ToLocalTime();
                _binaryReader.BaseStream.Seek(1, SeekOrigin.Current);
                Username = _binaryReader.ReadString();
                ExpectedNumOfBeatmaps = _binaryReader.ReadInt32();

                _logger?.Log(string.Format("Expected number of beatmaps: {0}", ExpectedNumOfBeatmaps));

                if (ExpectedNumOfBeatmaps < 0)
                {
                    return false;
                }
            }
            catch { return false; }
            return true;
        }
        private void DestoryReader()
        {
            _fileStream.Close();
            _binaryReader.Close();
            _fileStream.Dispose();
            _binaryReader.Dispose();
        }
        protected virtual bool FileExists(string fullPath)
        {
            return !string.IsNullOrEmpty(fullPath) && File.Exists(fullPath);
        }
    }
}
