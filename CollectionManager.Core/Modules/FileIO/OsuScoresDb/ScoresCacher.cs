namespace CollectionManager.Core.Modules.FileIo.OsuScoresDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using System.Linq;

public class ScoresCacher : IScoreDataManager
{
    public Dictionary<string, Scores> ScoreList { get; } = [];
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
        => GetScores(map.Hash);

    public Scores GetScores(string mapHash)
        => ScoreList.FirstOrDefault(s => s.Key.Equals(mapHash, StringComparison.Ordinal)).Value;

    public void Remove(string mapHash)
    {
        if (ScoreList.TryGetValue(mapHash, out Scores value))
        {
            foreach (IReplay score in value)
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

        if (ScoreList.TryGetValue(replay.MapHash, out Scores value))
        {
            value.Add(replay);
        }
        else
        {
            ScoreList.Add(replay.MapHash, [replay]);
        }

        Scores.Add(replay);
    }

    public void UpdateBeatmapsScoreMetadata(IMapDataManager mapDataManager)
    {
        foreach (KeyValuePair<string, Scores> scoresKv in ScoreList)
        {
            string mapHash = scoresKv.Key;
            Scores scores = scoresKv.Value;

            if (scores is null || scores.Count == 0 || mapDataManager.GetByHash(mapHash) is not BeatmapExtension beatmap)
            {
                continue;
            }

            beatmap.ScoresCount = scores.Count;
            beatmap.LastScoreDate = scores.Max(s => s.Date);
        }
    }
}