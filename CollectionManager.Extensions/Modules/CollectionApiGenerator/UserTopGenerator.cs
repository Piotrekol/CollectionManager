namespace CollectionManager.Extensions.Modules.CollectionApiGenerator;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Modules.Mod;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManager.Extensions.Modules.API.osu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
    private readonly Dictionary<UserModePair, IList<ApiScore>> _scoreCache = [];
    private readonly Dictionary<BeatmapModePair, Beatmap> _beatmapCache = [];

    private class UserModePair
    {
        public UserModePair(string username, PlayMode playMode)
        {
            Username = username;
            PlayMode = playMode;
        }
        public string Username { get; }
        public PlayMode PlayMode { get; }
    }
    public class BeatmapModePair
    {
        public BeatmapModePair(int beatmapId, PlayMode playMode)
        {
            BeatmapId = beatmapId;
            PlayMode = playMode;
        }
        public int BeatmapId { get; }
        public PlayMode PlayMode { get; }
    }

    public UserTopGenerator(string osuApiKey, MapCacher mapCacher)
    {
        ArgumentNullException.ThrowIfNull(mapCacher);

        if (string.IsNullOrEmpty(osuApiKey))
        {
            throw new ArgumentException("osuApiKey is required.");
        }

        _osuApi = new OsuApi(osuApiKey);
        _mapCacher = mapCacher;
        _collectionManager = new CollectionsManager(_mapCacher.Beatmaps);
    }

    public OsuCollections GetPlayersCollections(CollectionGeneratorConfiguration cfg, LogCollectionGeneration logger, CancellationToken cancellationToken)
    {
        int totalUsernames = cfg.Usernames.Count;
        int processedCounter = 0;
        OsuCollections c = [];
        _osuApi.ApiKey = cfg.ApiKey;
        _logger = logger;
        _logger?.Invoke(StartingProcessing, 0d);
        _collectionManager.EditCollection(CollectionEditArgs.ClearCollections());
        try
        {
            foreach (string username in cfg.Usernames)
            {
                cancellationToken.ThrowIfCancellationRequested();

                OsuCollections collections = GetPlayerCollections(username,
                    cfg.CollectionNameSavePattern, cfg.Gamemode, cfg.ScoreSaveConditions, cancellationToken);

                Log(username, ParsingFinished, ++processedCounter / (double)totalUsernames * 100);

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
        {
            username = _lastUsername;
        }
        else
        {
            _lastUsername = username;
        }

        _logger?.Invoke(string.Format(ParsingUser, username, message), precentage);
    }
    private OsuCollections GetPlayerCollections(string username, string collectionNameSavePattern, int gamemode,
        ScoreSaveConditions configuration, CancellationToken cancellationToken)
    {
        _currentUserMissingMapCount = 0;
        IList<ApiScore> validScores = GetPlayerScores(username, (PlayMode)gamemode, configuration, cancellationToken);
        Dictionary<string, Beatmaps> collectionsDict = [];
        OsuCollections collections = [];
        foreach (ApiScore s in validScores)
        {
            if (configuration.IsEligibleForSaving(s))
            {
                string collectionName = CreateCollectionName(s, username, collectionNameSavePattern);
                if (collectionsDict.TryGetValue(collectionName, out Beatmaps value))
                {
                    value.Add(GetBeatmapFromId(s.BeatmapId, (PlayMode)gamemode));
                }
                else
                {
                    collectionsDict.Add(collectionName, [GetBeatmapFromId(s.BeatmapId, (PlayMode)gamemode)]);
                }
            }
        }

        foreach (KeyValuePair<string, Beatmaps> c in collectionsDict)
        {
            OsuCollection collection = new(_mapCacher) { Name = c.Key };
            foreach (Beatmap beatmap in c.Value)
            {
                collection.AddBeatmap(beatmap);
            }

            collections.Add(collection);
        }

        return collections;
    }

    private Beatmap GetBeatmapFromId(int beatmapId, PlayMode gamemode)
    {
        foreach (Core.Types.Beatmap loadedBeatmap in _mapCacher.Beatmaps)
        {
            if (loadedBeatmap.MapId == beatmapId)
            {
                return loadedBeatmap;
            }
        }

        Beatmap beatmapFromCache = _beatmapCache.FirstOrDefault(s => s.Key.BeatmapId == beatmapId & s.Key.PlayMode == gamemode).Value;
        if (beatmapFromCache != null)
        {
            return beatmapFromCache;
        }

        Beatmap result;
        _currentUserMissingMapCount++;
        do
        {
            int i = 1;
            int Cooldown = 20;
            do
            {
                Log(null, string.Format(GettingBeatmaps, _currentUserMissingMapCount));
                result = _osuApi.GetBeatmap(beatmapId, gamemode);
            } while (result == null && i++ < 5);
            if (result == null)
            {
                Log(null, string.Format(GettingBeatmapFailed, i, Cooldown));
                Thread.Sleep(Cooldown * 1000);
            }
        } while (result == null);
        _beatmapCache.Add(new BeatmapModePair(beatmapId, gamemode), result);
        return result;
    }

    private IList<ApiScore> GetPlayerScores(string username, PlayMode gamemode, ScoreSaveConditions configuration, CancellationToken cancellationToken)
    {
        Log(username, string.Format(GettingScores, 1));
        IList<ApiScore> scoresFromCache =
            _scoreCache.FirstOrDefault(s => s.Key.Username == username & s.Key.PlayMode == gamemode).Value;
        if (scoresFromCache != null)
        {
            return scoresFromCache;
        }

        List<ApiScore> egibleScores = [];
        IList<ApiScore> scores;

        do
        {
            int i = 1;
            int Cooldown = 20;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                Log(username, string.Format(GettingScores, i));
                scores = _osuApi.GetUserBest(username, gamemode);
            } while (scores == null && i++ < 5);
            if (scores == null)
            {
                Log(username, string.Format(GettingUserFailed, i, Cooldown));
                Thread.Sleep(Cooldown * 1000);
            }
        } while (scores == null);

        _scoreCache.Add(new UserModePair(username, gamemode), scores);
        foreach (ApiScore s in scores)
        {
            if (configuration.IsEligibleForSaving(s))
            {
                egibleScores.Add(s);
            }
        }

        return egibleScores;
    }
    private static readonly ModParser modParser = new();
    public static string CreateCollectionName(ApiScore score, string username, string collectionNameFormat)
    {
        try
        {
            return string.Format(collectionNameFormat, username,
                modParser.GetModsFromEnum(score.EnabledMods, true));
        }
        catch (FormatException)
        {
            return "Invalid format!";
        }
    }

    public delegate void LogCollectionGeneration(string logMessage, double precentage);
}