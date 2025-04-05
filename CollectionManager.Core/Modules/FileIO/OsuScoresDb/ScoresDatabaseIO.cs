namespace CollectionManager.Core.Modules.FileIo.OsuScoresDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class ScoresDatabaseIo
{
    private readonly IScoreDataManager _scoreDataStorer;
    private FileStream _fileStream;
    private OsuBinaryReader _binaryReader;
    public int Version { get; private set; }
    public int NumberOfBeatmaps { get; private set; }
    public List<string> FileErrorList { get; private set; } = [];

    public ScoresDatabaseIo(IScoreDataManager scoreDataStorer)
    {
        _scoreDataStorer = scoreDataStorer;
    }

    public virtual void ReadDb(string osuScoresDbPath, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (FileExists(osuScoresDbPath))
        {
            _fileStream = new FileStream(osuScoresDbPath, FileMode.Open, FileAccess.Read);
            _binaryReader = new OsuBinaryReader(_fileStream);
            if (DatabaseContainsData())
            {
                _scoreDataStorer.StartMassStoring();
                for (int i = 0; i < NumberOfBeatmaps; i++)
                {
                    _ = _binaryReader.ReadString();
                    int scoresCount = _binaryReader.ReadInt32();
                    for (int j = 0; j < scoresCount; j++)
                    {
                        _scoreDataStorer.Store(Score.ReadScore(_binaryReader, true, Version));
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                _scoreDataStorer.EndMassStoring();
            }

            DestoryReader();
        }

        GC.Collect();
    }
    public void ReadReplay(string filelocation, bool minimalLoad = true)
    {
        FileStream fs = new(filelocation, FileMode.Open, FileAccess.Read);
        using OsuBinaryReader reader = new(fs);
        try
        {
            _scoreDataStorer.Store((Score)Replay.Read(reader, new Score(), minimalLoad));
        }
        catch
        {
            FileErrorList.Add(filelocation);
        }
    }
    public virtual void SaveDb(Dictionary<string, Scores> scores, int version, string saveLocation)
    {
        MemoryStream memStream = new();
        OsuBinaryWriter writer = new(memStream);
        int mapCount = scores.Keys.Count;

        writer.Write(version);
        writer.Write(mapCount);
        foreach (KeyValuePair<string, Scores> score in scores)
        {
            writer.Write(score.Key);
            writer.Write(score.Value.Count);

            foreach (IReplay s in score.Value)
            {
                s.Write(writer);
            }
        }

        FileStream fs = new(saveLocation, FileMode.Create, FileAccess.ReadWrite);
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
    protected virtual bool FileExists(string fullPath) => !string.IsNullOrEmpty(fullPath) && File.Exists(fullPath);

}