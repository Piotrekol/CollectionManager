using System;
using System.Collections.Generic;
using System.Threading;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Modules.API.osu;
using CollectionManager.Modules.ModParser;

namespace CollectionManagerExtensionsDll.Modules.CollectionGenerator
{
    public class UserTopGenerator
    {
        private readonly string StartingProcessing = "Preparing...";
        private readonly string ParsingUser = "Processing \"{0}\" | {1}";
        private readonly string GettingScores = "Getting scores from api...(try {0} of 5)";
        private readonly string GettingBeatmaps = "Getting missing beatmaps data from api... {0}";
        private readonly string ParsingFinished = "Done processing {0} users! - Close this window to add created collections";
        private readonly string GettingUserFailed = "FAILED | Waiting {1}s and trying again.";
        private readonly string GettingBeatmapFailed = "FAILED | Waiting {1}s and trying again.";
        private readonly string Aborted = "FAILED | User aborted.";
        private int _currentUserMissingMapCount;

        private readonly MapCacher _mapCacher;
        private readonly OsuApi _osuApi;
        private readonly CollectionsManager _collectionManager;
        private LogCollectionGeneration _logger;
        readonly Dictionary<string, IList<ApiScore>> _scoreCache = new Dictionary<string, IList<ApiScore>>();
        readonly Dictionary<int, Beatmap> _beatmapCache = new Dictionary<int, Beatmap>();
        
        public UserTopGenerator(string osuApiKey, MapCacher mapCacher)
        {
            if (mapCacher == null)
                throw new ArgumentNullException(nameof(mapCacher));
            if (string.IsNullOrEmpty(osuApiKey))
                throw new ArgumentException("osuApiKey is required.");
            
            _osuApi = new OsuApi(osuApiKey);
            _mapCacher = mapCacher;
            _collectionManager = new CollectionsManager(_mapCacher.Beatmaps);
        }


        public Collections GetPlayersCollections(CollectionGeneratorConfiguration cfg, LogCollectionGeneration logger)
        {
            int totalUsernames = cfg.Usernames.Count;
            int processedCounter = 0;
            var c = new Collections();
            _osuApi.ApiKey = cfg.ApiKey;
            _logger = logger;
            _logger?.Invoke(StartingProcessing, 0d);
            _collectionManager.EditCollection(CollectionEditArgs.ClearCollections());
            try
            {
                foreach (var username in cfg.Usernames)
                {
                    var collections = GetPlayerCollections(username,
                        cfg.CollectionNameSavePattern, cfg.ScoreSaveConditions);
                    Log(username, ParsingFinished,
                        ++processedCounter / (double)totalUsernames * 100);
                    _collectionManager.EditCollection(CollectionEditArgs.AddOrMergeCollections(collections));
                }

                c.AddRange(_collectionManager.LoadedCollections);
                _logger?.Invoke(string.Format(ParsingFinished, cfg.Usernames.Count), 100);

                _logger = null;
                return c;
            }
            catch (ThreadAbortException)
            {
                _logger?.Invoke(Aborted, -1d);
                return c;
            }
        }

        private string _lastUsername = "";
        private void Log(string username, string message, double precentage = -1d)
        {
            if (string.IsNullOrEmpty(username))
                username = _lastUsername;
            else
                _lastUsername = username;
            _logger?.Invoke(string.Format(ParsingUser, username, message), precentage);
        }
        private Collections GetPlayerCollections(string username, string collectionNameSavePattern,
            ScoreSaveConditions configuration)
        {
            _currentUserMissingMapCount = 0;
            var validScores = GetPlayerScores(username, configuration);
            Dictionary<string, Beatmaps> collectionsDict = new Dictionary<string, Beatmaps>();
            var collections = new Collections();
            foreach (var s in validScores)
            {
                if (configuration.IsEgibleForSaving(s))
                {
                    string collectionName = CreateCollectionName(s, username, collectionNameSavePattern);
                    if (collectionsDict.ContainsKey(collectionName))
                        collectionsDict[collectionName].Add(GetBeatmapFromId(s.BeatmapId));
                    else
                        collectionsDict.Add(collectionName, new Beatmaps() { GetBeatmapFromId(s.BeatmapId) });
                }
            }
            foreach (var c in collectionsDict)
            {
                var collection = new Collection(_mapCacher) { Name = c.Key };
                foreach (var beatmap in c.Value)
                {
                    collection.AddBeatmap(beatmap);
                }
                collections.Add(collection);
            }
            return collections;
        }

        private Beatmap GetBeatmapFromId(int beatmapId)
        {
            foreach (var loadedBeatmap in _mapCacher.Beatmaps)
            {
                if (loadedBeatmap.MapId == beatmapId)
                    return loadedBeatmap;
            }
            if (_beatmapCache.ContainsKey(beatmapId))
                return _beatmapCache[beatmapId];
            Beatmap result;
            _currentUserMissingMapCount++;
            do
            {
                int i = 1;
                int Cooldown = 20;
                do
                {
                    Log(null, string.Format(GettingBeatmaps, _currentUserMissingMapCount));
                    result = _osuApi.GetBeatmap(beatmapId);
                } while (result == null && i++ < 5);
                if (result == null)
                {
                    Log(null, string.Format(GettingBeatmapFailed, i, Cooldown));
                    Thread.Sleep(Cooldown * 1000);
                }
            } while (result == null);
            _beatmapCache.Add(beatmapId,result);
            return result;
        }
        private IList<ApiScore> GetPlayerScores(string username, ScoreSaveConditions configuration)
        {
            Log(username, string.Format(GettingScores, 1));
            if (_scoreCache.ContainsKey(username))
                return _scoreCache[username];

            List<ApiScore> egibleScores = new List<ApiScore>();
            IList<ApiScore> scores;

            do
            {
                int i = 1;
                int Cooldown = 20;
                do
                {
                    Log(username, string.Format(GettingScores, i));
                    scores = _osuApi.GetUserBest(username, PlayMode.Osu);
                } while (scores == null && i++ < 5);
                if (scores == null)
                {
                    Log(username, string.Format(GettingUserFailed, i, Cooldown));
                    Thread.Sleep(Cooldown * 1000);
                }
            } while (scores == null);

            _scoreCache.Add(username, scores);
            foreach (var s in scores)
            {
                if (configuration.IsEgibleForSaving(s))
                    egibleScores.Add(s);
            }
            return egibleScores;
        }
        private static ModParser modParser = new ModParser();
        public static string CreateCollectionName(ApiScore score, string username, string collectionNameFormat)
        {
            try
            {
                return String.Format(collectionNameFormat, username,
                    modParser.GetModsFromEnum(score.EnabledMods, true));
            }
            catch (FormatException ex)
            {
                return "Invalid format!";
            }
        }

        public delegate void LogCollectionGeneration(string logMessage, double precentage);
    }
}