namespace CollectionManager.Core.Modules.FileIo.OsuScoresDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using System.Linq;

public class ScoresCacher : IScoreDataManager
{
    public Dictionary<string, Scores> ScoreList = [];
    public Scores Scores { get; private set; } = [];
    public HashSet<string> ScoreHashes { get; private set; } = [];
    public void StartMassStoring()
    {
    }

    public void EndMassStoring()
    {
    }

    public void Clear()
    {
        ScoreList.Clear();
        Scores.Clear();
        ScoreHashes.Clear();
    }

    public Scores GetScores(Beatmap map)
        => GetScores(map.Md5);

    public Scores GetScores(string mapHash)
        => ScoreList.FirstOrDefault(s => s.Key.Equals(mapHash)).Value;

    public void Remove(string mapHash)
    {
        if (ScoreList.ContainsKey(mapHash))
        {
            foreach (IReplay score in ScoreList[mapHash])
            {
                _ = Scores.Remove(score);
                _ = ScoreHashes.Remove(score.ReplayHash);
            }

            _ = ScoreList.Remove(mapHash);
        }
    }
    public void Store(IReplay replay)
    {
        if (ScoreHashes.Contains(replay.ReplayHash))
        {
            return;
        }

        _ = ScoreHashes.Add(replay.ReplayHash);

        if (ScoreList.ContainsKey(replay.MapHash))
        {
            ScoreList[replay.MapHash].Add(replay);
        }
        else
        {
            ScoreList.Add(replay.MapHash, [replay]);
        }

        Scores.Add(replay);
    }
}