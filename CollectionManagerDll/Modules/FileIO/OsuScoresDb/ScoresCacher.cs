using System.Collections.Generic;
using System.Linq;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.FileIO.OsuScoresDb
{
    public class ScoresCacher : IScoreDataManager
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
                foreach (var score in ScoreList[mapHash])
                {
                    Scores.Remove(score);
                    ScoreHashes.Remove(score.ReplayHash);
                }
                ScoreList.Remove(mapHash);
            }
        }
        public void Store(IReplay replay)
        {
            if (ScoreHashes.Contains(replay.ReplayHash))
                return;

            ScoreHashes.Add(replay.ReplayHash);

            if (ScoreList.ContainsKey(replay.MapHash))
                ScoreList[replay.MapHash].Add(replay);
            else
                ScoreList.Add(replay.MapHash, new Scores() { replay });
            Scores.Add(replay);
        }
    }
}