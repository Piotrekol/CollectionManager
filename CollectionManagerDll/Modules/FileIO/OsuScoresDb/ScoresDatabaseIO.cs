using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.FileIO.OsuScoresDb
{
    public class ScoresDatabaseIo
    {
        private readonly IScoreDataStorer _scoreDataStorer;
        private FileStream _fileStream;
        private OsuBinaryReader _binaryReader;
        public int Version { get; private set; }
        public int NumberOfBeatmaps { get; private set; }
        public List<string> FileErrorList { get; private set; } = new List<string>();

        public ScoresDatabaseIo(IScoreDataStorer scoreDataStorer)
        {
            _scoreDataStorer = scoreDataStorer;
        }

        public virtual void ReadDb(string osuScoresDbPath, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            if (FileExists(osuScoresDbPath))
            {
                _fileStream = new FileStream(osuScoresDbPath, FileMode.Open, FileAccess.Read);
                _binaryReader = new OsuBinaryReader(_fileStream);
                if (DatabaseContainsData())
                {
                    _scoreDataStorer.StartMassStoring();
                    for (int i = 0; i < NumberOfBeatmaps; i++)
                    {
                        string mapHash = _binaryReader.ReadString();
                        int scoresCount = _binaryReader.ReadInt32();
                        for (int j = 0; j < scoresCount; j++)
                        {
                            _scoreDataStorer.Store(Score.ReadScore(_binaryReader, true, Version));
                        }

                        if (cancellationToken.IsCancellationRequested)
                            break;
                    }
                    _scoreDataStorer.EndMassStoring();
                }
                DestoryReader();
            }
            GC.Collect();
        }
        public void ReadReplay(string filelocation, bool minimalLoad = true)
        {
            var fs = new FileStream(filelocation, FileMode.Open, FileAccess.Read);
            using (var reader = new OsuBinaryReader(fs))
                try
                {
                    _scoreDataStorer.Store((Score)Replay.Read(reader, new Score(), minimalLoad));
                }
                catch { FileErrorList.Add(filelocation); }
        }
        public virtual void SaveDb(Dictionary<string, Scores> scores, int version, string saveLocation)
        {
            var memStream = new MemoryStream();
            var writer = new OsuBinaryWriter(memStream);
            var mapCount = scores.Keys.Count;

            writer.Write(version);
            writer.Write(mapCount);
            foreach (var score in scores)
            {
                writer.Write(score.Key);
                writer.Write(score.Value.Count);

                foreach (var s in score.Value)
                {
                    s.Write(writer);
                }
            }

            var fs = new FileStream(saveLocation, FileMode.Create, FileAccess.ReadWrite);
            memStream.WriteTo(fs);
            fs.Flush();
            fs.Close();
            writer.Close();
        }
        private bool DatabaseContainsData()
        {
            try
            {
                Version = _binaryReader.ReadInt32();
                NumberOfBeatmaps = _binaryReader.ReadInt32();
            }
            catch
            {
                return false;
            }
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