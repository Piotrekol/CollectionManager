using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Modules.API.osu;
using StringLib;

namespace CollectionManagerExtensionsDll.Modules.CollectionGenerator
{
    public class CollectionGenerator
    {
        private MapCacher _mapCacher;
        private OsuApi _osuApi;
        private OsuWeb _osuWeb;
        private ModParser.ModParser modParser = new ModParser.ModParser();
        private Dictionary<string, Func<OnlineScore, ScoreSaveConditions, bool>> ScoreCheckers = new Dictionary<string, Func<OnlineScore, ScoreSaveConditions, bool>>();
        private readonly Dictionary<int, BeatmapExtension> _downloadedBeatmaps = new Dictionary<int, BeatmapExtension>();

        public CollectionGenerator(MapCacher mapCacher = null, string apiKey = null)
        {
            SetMapCacher(mapCacher);
            SetApiKey(apiKey);

            ScoreCheckers.Add("Pp", (score, settings) => (score.Pp >= settings.MinimumPp && score.Pp <= settings.MaximumPp));
            ScoreCheckers.Add("Acc", (score, settings) =>
            {
                var totalHits = score.Countmiss + score.Count50 + score.Count100 + score.Count300;
                var pointsOfHits = score.Count50 * 50 + score.Count100 * 100 + score.Count300 * 300;
                double acc = ((double)pointsOfHits / (double)(totalHits * 300)) * 100;
                acc = Math.Round(acc, 2);

                return (acc >= settings.MinimumAcc && acc <= settings.MaximumAcc);
            });
            ScoreCheckers.Add("Rank", (score, settings) =>
            {
                var r = score.Rank;
                switch (settings.RanksToGet)
                {
                    case RankTypes.All:
                        return true;
                    case RankTypes.SAndBetter:
                        return (r.Contains("S") || r.Contains("X"));//XH SH X S
                    case RankTypes.AAndWorse:
                        return (!(r.Contains("S") || r.Contains("X")));//A B C D
                }
                return true;
            });
        }

        public void SetMapCacher(MapCacher mapCacher)
        {
            _mapCacher = mapCacher;
        }

        public void SetApiKey(string key)
        {
            if (key != null)
                _osuApi = new OsuApi(key);
            else
                _osuApi = null;
        }

        private List<OnlineScore> GetPlayerResult(string username, int topsLimit = 100, PlayModes mode = PlayModes.Osu)
        {
            List<OnlineScore> result = null;
            for (int i = 0; i < 3; i++)
            {
                result = _osuApi.GetUserBest(username, PlayModes.Osu, topsLimit);
                if (result != null)
                    break;
            }
            return result;
        }



        public Collections Generate(List<string> usernameList, CollectionGeneratorSettings settings, PlayModes mode = PlayModes.Osu)
        {
            var collections = new Collections();

            foreach (var username in usernameList)
            {
                if (string.IsNullOrWhiteSpace(username))
                    continue;
                var result = GetPlayerResult(username, settings.NumberOfTops);
                string collectionNameWithoutMods = string.Format(settings.CollectionNameSavePattern, username);


                var collectionsToFill = GetCollectionsToSave(result, collectionNameWithoutMods, settings);
                foreach (var scoreList in collectionsToFill)
                {
                    var collection = new Collection(_mapCacher) { Name = scoreList.Key };
                    var scores = scoreList.Value;
                    foreach (var score in scores)
                    {
                        var map = GetBeatmapFromId(score.BeatmapId);
                        if (map == null)
                            continue;
                        map.UserComment += "{Pp:F0}:{Acc:0.##}% ".HaackFormat(score);
                        collection.AddBeatmap(map);
                    }
                    collections.Add(collection);
                }

            }

            if (settings.MergeCollections)
            {
                for (int i = 0; i < collections.Count; i++)
                {
                    var currentCollection = collections[i];
                    for (int j = i-1; j > 0; j--)
                    {
                        if (currentCollection.Name == collections[j].Name)
                        {//found dupe - copy maps
                            foreach (var b in collections[j].AllBeatmaps())
                            {
                                currentCollection.AddBeatmap(b);
                            }
                            collections.RemoveAt(j);
                        }
                    }
                }

            }


            return collections;
        }


        private Dictionary<string, List<OnlineScore>> GetCollectionsToSave(List<OnlineScore> result, string collectionName, CollectionGeneratorSettings settings)
        {
            var collectionsToSave = new Dictionary<string, List<OnlineScore>>();

            if (!settings.GroupByMods)
                collectionsToSave.Add(collectionName, new List<OnlineScore>());
            foreach (var score in result)
            {
                if (ScoreIsValid(score, settings.ScoreSaveConditions))
                {
                    if (settings.GroupByMods)
                    {
                        var scoreMods = modParser.GetModsFromEnum(score.EnabledMods, true);
                        var collectionNamewithMods = collectionName + scoreMods;
                        if (!collectionsToSave.ContainsKey(collectionNamewithMods))
                            collectionsToSave.Add(collectionNamewithMods, new List<OnlineScore>());
                        collectionsToSave[collectionNamewithMods].Add(score);
                    }
                    else
                    {
                        collectionsToSave[collectionName].Add(score);
                    }
                }
            }
            return collectionsToSave;
        }
        private BeatmapExtension GetBeatmapFromId(int beatmapId)
        {
            if (_mapCacher.LoadedBeatmapsMapIdDict.ContainsKey(beatmapId))
                return _mapCacher.LoadedBeatmapsMapIdDict[beatmapId];

            if (_downloadedBeatmaps.ContainsKey(beatmapId))
                return _downloadedBeatmaps[beatmapId];

            BeatmapExtension result = null;
            for (int i = 0; i < 3; i++)
            {
                result = _osuApi.GetBeatmap(beatmapId);
                if (result != null)
                    break;
            }
            if (result != null)
                _downloadedBeatmaps.Add(beatmapId, result);
            return result;
        }

        private bool ScoreIsValid(OnlineScore score, ScoreSaveConditions settings)
        {
            foreach (var s in ScoreCheckers)
            {
                if (!s.Value(score, settings))
                    return false;
            }
            return true;
        }


    }


}