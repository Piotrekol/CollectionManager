#define GetStarsCombinations
using System;
using System.Collections.Generic;
using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.FileIO.OsuDb
{
    //TODO: refactor to allow for read/write access.
    public class OsuDatabaseLoader
    {
        private readonly ILogger _logger;
        private readonly IMapDataManager _mapDataStorer;
        private FileStream _fileStream;
        private BinaryReader _binaryReader;
        private Exception _exception;
        private readonly Beatmap _tempBeatmap;
        private bool _stopProcessing;

        public bool LoadedSuccessfully
        {
            get
            {
                if (ExpectedNumOfBeatmaps != -1)
                    return !_stopProcessing;
                return false;
            }
        }

        public int MapsWithNoId { get; private set; }
        public string Username { get; private set; }
        public int FileDate { get; private set; }
        public int ExpectedNumberOfMapSets { get; private set; }
        public int ExpectedNumOfBeatmaps { get; private set; } = -1;
        public int NumberOfLoadedBeatmaps { get; private set; }

        public OsuDatabaseLoader(ILogger logger, IMapDataManager mapDataStorer,Beatmap tempBeatmap)
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
            _mapDataStorer.StartMassStoring();
            for (NumberOfLoadedBeatmaps = 0; NumberOfLoadedBeatmaps < ExpectedNumOfBeatmaps; NumberOfLoadedBeatmaps++)
            {
                //TODO: check if it is safe to remove all try/catch _stopProcessing stuff
                if (_stopProcessing)
                {
                    throw _exception;
                }
                ReadNextBeatmap();
            }
            _mapDataStorer.EndMassStoring();
        }
        private void ReadNextBeatmap()
        {
            _tempBeatmap.InitEmptyValues();
            try
            {
                ReadMapHeader(_tempBeatmap);
                ReadMapInfo(_tempBeatmap);
                ReadTimingPoints(_tempBeatmap);
                ReadMapMetaData(_tempBeatmap);
            }
            catch (Exception e)
            {
                _exception = e;
                _stopProcessing = true;
                return;
            }
            _mapDataStorer.StoreBeatmap(_tempBeatmap);
        }

        private void ReadMapMetaData(Beatmap beatmap)
        {
            beatmap.MapId = Math.Abs(_binaryReader.ReadInt32());
            if (beatmap.MapId == 0) MapsWithNoId++;

            beatmap.MapSetId = Math.Abs(_binaryReader.ReadInt32());
            beatmap.ThreadId = Math.Abs(_binaryReader.ReadInt32());
            beatmap.MapRating = _binaryReader.ReadInt32();
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
            beatmap.BgDim = _binaryReader.ReadInt16();
            //bytes not analysed.
            if (FileDate <= 20160403)
                _binaryReader.BaseStream.Seek(4, SeekOrigin.Current);
            else
                _binaryReader.BaseStream.Seek(8, SeekOrigin.Current);
        }
        private void ReadTimingPoints(Beatmap beatmap)
        {
            int amountOfTimingPoints = _binaryReader.ReadInt32();
            double minBpm = double.MaxValue,
            maxBpm = double.MinValue;
            double bpmDelay, time;
            bool InheritsBPM;
            for (int i = 0; i < amountOfTimingPoints; i++)
            {
                bpmDelay = _binaryReader.ReadDouble();
                time = _binaryReader.ReadDouble();
                InheritsBPM = _binaryReader.ReadBoolean();
                if (InheritsBPM)
                {
                    if (60000 / bpmDelay < minBpm)
                        minBpm = 60000 / bpmDelay;
                    if (60000 / bpmDelay > maxBpm)
                        maxBpm = 60000 / bpmDelay;
                }
            }
            beatmap.MaxBpm = maxBpm;
            beatmap.MinBpm = minBpm;
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
                beatmap.ModPpStars.Add(playMode, new Dictionary<int, double>());
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
            if (ticks < 0L)
            {
                return new DateTime();
            }
            try
            {
                return new DateTime(ticks, DateTimeKind.Utc);
            }
            catch (Exception e)
            {
                _exception = e;
                _stopProcessing = true;
                return new DateTime();
            }
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
            try
            {
                if (_binaryReader.ReadByte() == 11)
                {
                    return _binaryReader.ReadString();
                }
                return "";
            }
            catch { _stopProcessing = true; return ""; }
        }
        private bool DatabaseContainsData()
        {
            FileDate = _binaryReader.ReadInt32();
            ExpectedNumberOfMapSets = _binaryReader.ReadInt32();
            _logger?.Log(string.Format("Expected number of mapSets: {0}", ExpectedNumberOfMapSets));
            try
            {
                bool something = _binaryReader.ReadBoolean();
                DateTime a = GetDate().ToLocalTime();
                _binaryReader.BaseStream.Seek(1, SeekOrigin.Current);
                Username = _binaryReader.ReadString();
                ExpectedNumOfBeatmaps = _binaryReader.ReadInt32();
                if (FileDate > 20160403)
                    _binaryReader.BaseStream.Seek(4, SeekOrigin.Current);
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
