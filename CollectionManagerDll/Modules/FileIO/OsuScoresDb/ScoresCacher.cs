using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.FileIO.OsuScoresDb
{
    public class ScoresCacher : IScoreDataStorer
    {
        public Dictionary<string, Scores> ScoreList = new Dictionary<string, Scores>();
        public Scores Scores { get; private set; } = new Scores();
        public HashSet<string> ScoreHashes { get; private set; } = new HashSet<string>();
        public void StartMassStoring()
        {
        }

        public void EndMassStoring()
        {
        }

        public List<Score> GetScores(Beatmap map)
        {
            return GetScores(map.Md5);
        }

        public List<Score> GetScores(string mapHash)
        {
            var scores = new List<Score>();
            if (ScoreList.ContainsKey(mapHash))
                scores.AddRange(ScoreList[mapHash]);
            return scores;
        }

        public void Remove(string mapHash)
        {
            if (ScoreList.ContainsKey(mapHash))
            {
                foreach (var score in ScoreList[mapHash])
                {
                    Scores.Remove(score);
                    ScoreHashes.Remove(score.ReplayHash);
                }
                ScoreList.Remove(mapHash);
            }
        }
        public void Store(Score score)
        {
            if (ScoreHashes.Contains(score.ReplayHash))
                return;
            ScoreHashes.Add(score.ReplayHash);

            if (ScoreList.ContainsKey(score.MapHash))
                ScoreList[score.MapHash].Add(score);
            else
                ScoreList.Add(score.MapHash, new Scores() { score });
            Scores.Add(score);
        }
    }
}