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
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public sealed partial class UserTopGenerator : IDisposable
{
    private readonly string StartingProcessing = "Preparing...";
    private readonly string ParsingUser = "Processing \"{0}\" | {1}";
    private readonly string GettingScores = "Getting scores from api...(try {0} of 5)";
    private readonly string GettingBeatmaps = "Getting missing beatmaps data from api... {0}";
    private readonly string ParsingFinished = "Done processing {0} users! - Close this window to add created collections";
    private readonly string GettingUserFailed = "FAILED | Waiting {1}s and trying again.";
    private readonly string GettingBeatmapFailed = "FAILED | Waiting {1}s and trying again.";
    private readonly string Aborted = "FAILED | User aborted.";

    private readonly MapCacher _mapCacher;
    private readonly OsuApi _osuApi;
    private readonly CollectionsManager _collectionManager;
    private readonly Dictionary<UserModePair, IList<ApiScore>> _scoreCache = [];
    private readonly Dictionary<BeatmapModePair, Beatmap> _beatmapCache = [];

    private LogCollectionGeneration _logger;
    private int _currentUserMissingMapCount;
    private string _lastLogUsername = string.Empty;

    public UserTopGenerator(string osuApiKey, MapCacher mapCacher)
    {
        ArgumentNullException.ThrowIfNull(mapCacher);

        if (string.IsNullOrEmpty(osuApiKey))
        {
            throw new ArgumentException("osuApiKey is required.");
        }

        _osuApi = new OsuApi(osuApiKey);
        _mapCacher = mapCacher;
        _collectionManager = new CollectionsManager(_mapCacher);
    }

    public async Task<OsuCollections> GetPlayersCollectionsAsync(CollectionGeneratorConfiguration configuration, LogCollectionGeneration logger, CancellationToken cancellationToken)
    {
        int totalUsernames = configuration.Usernames.Count;
        int processedCounter = 0;
        OsuCollections newCollections = [];
        _osuApi.ApiKey = configuration.ApiKey;
        _logger = logger;
        _logger?.Invoke(StartingProcessing, 0d);
        _collectionManager.EditCollection(CollectionEditArgs.ClearCollections());

        try
        {
            foreach (string username in configuration.Usernames)
            {
                cancellationToken.ThrowIfCancellationRequested();

                OsuCollections collections = await GetPlayerCollectionsAsync(username, configuration.CollectionNameSavePattern, (PlayMode)configuration.Gamemode, configuration.ScoreSaveConditions, cancellationToken);

                Log(username, ParsingFinished, ++processedCounter / (double)totalUsernames * 100);

                _collectionManager.EditCollection(CollectionEditArgs.AddOrMergeCollections(collections));
            }

            newCollections.AddRange(_collectionManager.LoadedCollections);
            _logger?.Invoke(string.Format(CultureInfo.InvariantCulture, ParsingFinished, configuration.Usernames.Count, cancellationToken), 100);

            return newCollections;
        }
        catch (OperationCanceledException)
        {
            _logger?.Invoke(Aborted, -1d);

            throw;
        }
        finally
        {
            _logger = null;
        }
    }

    private void Log(string username, string message, double precentage = -1d)
    {
        if (string.IsNullOrEmpty(username))
        {
            username = _lastLogUsername;
        }
        else
        {
            _lastLogUsername = username;
        }

        _logger?.Invoke(string.Format(CultureInfo.InvariantCulture, ParsingUser, username, message), precentage);
    }
    private async Task<OsuCollections> GetPlayerCollectionsAsync(string username, string collectionNameSavePattern, PlayMode gamemode, ScoreSaveConditions configuration, CancellationToken cancellationToken)
    {
        _currentUserMissingMapCount = 0;
        IList<ApiScore> validScores = await GetPlayerScores(username, gamemode, configuration, cancellationToken);
        Dictionary<string, Beatmaps> collectionsDict = [];
        OsuCollections collections = [];

        foreach (ApiScore s in validScores)
        {
            if (!configuration.IsEligibleForSaving(s, gamemode))
            {
                continue;
            }

            string collectionName = CreateCollectionName(s, username, collectionNameSavePattern);

            if (collectionsDict.TryGetValue(collectionName, out Beatmaps beatmaps))
            {
                beatmaps.Add(await GetBeatmapFromId(s.BeatmapId, gamemode, cancellationToken));
            }
            else
            {
                collectionsDict.Add(collectionName, [await GetBeatmapFromId(s.BeatmapId, gamemode, cancellationToken)]);
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

    private async Task<Beatmap> GetBeatmapFromId(int beatmapId, PlayMode gamemode, CancellationToken cancellationToken)
    {
        foreach (Beatmap loadedBeatmap in _mapCacher.Beatmaps)
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
                cancellationToken.ThrowIfCancellationRequested();

                Log(null, string.Format(CultureInfo.InvariantCulture, GettingBeatmaps, _currentUserMissingMapCount));
                result = _osuApi.GetBeatmap(beatmapId, gamemode);
            } while (result == null && i++ < 5);

            if (result == null)
            {
                Log(null, string.Format(CultureInfo.InvariantCulture, GettingBeatmapFailed, i, Cooldown));
                await Task.Delay(Cooldown * 1000, cancellationToken);
            }
        } while (result == null);

        _beatmapCache.Add(new BeatmapModePair(beatmapId, gamemode), result);

        return result;
    }

    private async Task<IList<ApiScore>> GetPlayerScores(string username, PlayMode gamemode, ScoreSaveConditions configuration, CancellationToken cancellationToken)
    {
        Log(username, string.Format(CultureInfo.InvariantCulture, GettingScores, 1));
        IList<ApiScore> scoresFromCache = _scoreCache.FirstOrDefault(s => s.Key.Username == username & s.Key.PlayMode == gamemode).Value;

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

                Log(username, string.Format(CultureInfo.InvariantCulture, GettingScores, i));
                scores = _osuApi.GetUserBest(username, gamemode);
            } while (scores == null && i++ < 5);

            if (scores == null)
            {
                Log(username, string.Format(CultureInfo.InvariantCulture, GettingUserFailed, i, Cooldown));
                await Task.Delay(Cooldown * 1000, cancellationToken);
            }
        } while (scores == null);

        _scoreCache.Add(new UserModePair(username, gamemode), scores);

        foreach (ApiScore s in scores)
        {
            if (configuration.IsEligibleForSaving(s, gamemode))
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
            return string.Format(CultureInfo.InvariantCulture, collectionNameFormat, username, modParser.GetModsFromEnum(score.EnabledMods, true));
        }
        catch (FormatException)
        {
            return "Invalid format!";
        }
    }

    public void Dispose() => _osuApi.Dispose();

    public delegate void LogCollectionGeneration(string logMessage, double precentage);
}