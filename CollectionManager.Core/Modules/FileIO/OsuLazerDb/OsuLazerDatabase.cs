namespace CollectionManager.Core.Modules.FileIo.OsuLazerDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using Realms;
using System;
using System.Collections.Generic;
using System.Threading;

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
        using LazerRealm lazerRealm = OpenRealm(realmFilePath);
        LoadScores(lazerRealm.Realm, lazerRealm.Adapter, progress);
        LoadBeatmaps(lazerRealm.Realm, lazerRealm.Adapter, progress, cancellationToken);

        _scoresDatabase.UpdateBeatmapsScoreMetadata(_mapDataManager);
    }

    private void LoadScores(Realm realm, LazerRealmAdapter adapter, IProgress<string> progress)
    {
        int scoresCount = adapter.CountScores(realm);
        progress?.Report($"Loading {scoresCount} scores");
        _scoresDatabase.StartMassStoring();

        foreach (LazerReplay lazerScore in adapter.LoadScores(realm))
        {
            _scoresDatabase.Store(lazerScore);
        }

        _scoresDatabase.EndMassStoring();
        progress?.Report($"Loaded {scoresCount} scores");
    }

    private void LoadBeatmaps(Realm realm, LazerRealmAdapter realmAdapter, IProgress<string> progress, CancellationToken cancellationToken)
    {
        int beatmapSetCount = realmAdapter.CountBeatmapSets(realm);
        progress?.Report($"Loading {beatmapSetCount} beatmap sets");
        int totalBeatmapCount = 0;
        int loadedBeatmapSetCount = 0;

        try
        {
            _mapDataManager.StartMassStoring();
            cancellationToken.ThrowIfCancellationRequested();

            foreach (IEnumerable<LazerBeatmap> lazerBeatmaps in realmAdapter.LoadBeatmapSets(realm, _scoresDatabase))
            {
                foreach (LazerBeatmap lazerBeatmap in lazerBeatmaps)
                {
                    totalBeatmapCount++;
                    _mapDataManager.StoreBeatmap(lazerBeatmap);
                }

                loadedBeatmapSetCount++;

                if (loadedBeatmapSetCount % 100 == 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    progress?.Report($"Loaded {loadedBeatmapSetCount} of {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
                }
            }
        }
        finally
        {
            _mapDataManager.EndMassStoring();
        }

        progress?.Report($"Loaded {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
    }
}
