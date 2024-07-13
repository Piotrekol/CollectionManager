using CollectionManager.DataTypes;
using CollectionManager.Extensions;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Linq;
using System.Threading;

namespace CollectionManager.Modules.FileIO.OsuLazerDb;
public sealed class OsuLazerDatabase
    : OsuRealmReader
{
    private readonly IMapDataManager _mapDataManager;
    private readonly IScoreDataManager _scoresDatabase;
    private readonly LazerBeatmap _baseBeatmap = new();

    public OsuLazerDatabase(IMapDataManager mapDataManager, IScoreDataManager scoresDatabase)
    {
        _mapDataManager = mapDataManager;
        _scoresDatabase = scoresDatabase;
    }

    public void Load(string realmFilePath, IProgress<string> progress, CancellationToken cancellationToken)
    {
        using Realm localRealm = GetRealm(realmFilePath);
        LoadScores(localRealm, progress);
        LoadBeatmaps(localRealm, progress);
    }

    private void LoadScores(Realm realm, IProgress<string> progress)
    {
        IQueryable<ScoreInfo> allLazerScores = realm.All<ScoreInfo>();
        int scoresCount = allLazerScores.Count();
        progress?.Report($"Loading {scoresCount} scores");
        _scoresDatabase.StartMassStoring();

        foreach (ScoreInfo lazerScore in allLazerScores)
        {
            _scoresDatabase.Store(lazerScore.ToLazerReplay());
        }

        _scoresDatabase.EndMassStoring();
        progress?.Report($"Loaded {scoresCount} scores");
    }

    private void LoadBeatmaps(Realm realm, IProgress<string> progress)
    {
        IQueryable<BeatmapInfo> allLazerBeatmaps = realm.All<BeatmapInfo>();
        int beatmapsCount = allLazerBeatmaps.Count();
        progress?.Report($"Loading {beatmapsCount} beatmaps");
        _mapDataManager.StartMassStoring();

        foreach (BeatmapInfo beatmapInfo in allLazerBeatmaps)
        {
            _mapDataManager.StoreBeatmap(beatmapInfo.ToLazerBeatmap(_scoresDatabase));
        }

        _mapDataManager.EndMassStoring();
        progress?.Report($"Loaded {beatmapsCount} beatmaps");
    }
}
