namespace CollectionManager.Core.Modules.FileIo.OsuLazerDb;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
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
        using Realm localRealm = GetRealm(realmFilePath);
        LoadScores(localRealm, progress);
        LoadBeatmaps(localRealm, progress, cancellationToken);

        _scoresDatabase.UpdateBeatmapsScoreMetadata(_mapDataManager);
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

    private void LoadBeatmaps(Realm realm, IProgress<string> progress, CancellationToken cancellationToken)
    {
        List<BeatmapSetInfo> allLazerBeatmapSets = realm.All<BeatmapSetInfo>().ToList();
        int beatmapSetCount = allLazerBeatmapSets.Count;
        progress?.Report($"Loading {beatmapSetCount} beatmap sets");
        _mapDataManager.StartMassStoring();
        int totalBeatmapCount = 0;

        for (int i = 0; i < allLazerBeatmapSets.Count; i++)
        {
            BeatmapSetInfo beatmapSetInfo = allLazerBeatmapSets[i];
            IEnumerable<LazerBeatmap> lazerBeatmaps = beatmapSetInfo.ToLazerBeatmaps(_scoresDatabase);

            foreach (LazerBeatmap lazerBeatmap in lazerBeatmaps)
            {
                totalBeatmapCount++;
                _mapDataManager.StoreBeatmap(lazerBeatmap);
            }

            if (i % 100 == 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                progress?.Report($"Loaded {i} of {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
            }
        }

        _mapDataManager.EndMassStoring();
        progress?.Report($"Loaded {beatmapSetCount} beatmap sets ({totalBeatmapCount} beatmaps)");
    }
}
