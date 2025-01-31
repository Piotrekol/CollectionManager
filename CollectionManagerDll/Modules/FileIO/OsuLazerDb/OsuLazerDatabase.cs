using CollectionManager.DataTypes;
using CollectionManager.Extensions;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace CollectionManager.Modules.FileIO.OsuLazerDb;

public sealed class OsuLazerDatabase
    : OsuRealmReader
{
    private readonly IMapDataManager _mapDataManager;
    private readonly IScoreDataManager _scoresDatabase;

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
        var allLazerBeatmapSets = realm.All<BeatmapSetInfo>().ToList();
        int beatmapSetCount = allLazerBeatmapSets.Count;
        progress?.Report($"Loading {beatmapSetCount} beatmap sets");
        _mapDataManager.StartMassStoring();
        var totalBeatmapCount = 0;

        for (int i = 0; i < allLazerBeatmapSets.Count; i++)
        {
            var beatmapSetInfo = allLazerBeatmapSets[i];
            IEnumerable<LazerBeatmap> lazerBeatmaps = beatmapSetInfo.ToLazerBeatmaps(_scoresDatabase);

            foreach (var lazerBeatmap in lazerBeatmaps)
            {
                totalBeatmapCount++;
                _mapDataManager.StoreBeatmap(lazerBeatmap);
            }

            if (i % 100 == 0)
            {
                progress?.Report($"Loaded {i} of {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
            }
        }

        _mapDataManager.EndMassStoring();
        progress?.Report($"Loaded {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
    }
}
