namespace CollectionManager.Core.Modules.FileIo.OsuLazerDb;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

internal sealed class LazerRealmAdapter51
    : LazerRealmAdapter
{
    public override Type[] ObjectTypes { get; } =
    [
        typeof(ScoreInfo),
        typeof(BeatmapInfo),
        typeof(BeatmapSetInfo),
        typeof(BeatmapCollection),
        typeof(BeatmapMetadata),
        typeof(BeatmapDifficulty),
        typeof(BeatmapUserSettings),
        typeof(RulesetInfo),
        typeof(RealmFile),
        typeof(RealmNamedFileUsage),
        typeof(RealmUser),
    ];

    public override int CountScores(Realm realm)
        => realm.All<ScoreInfo>().Count();

    public override IEnumerable<LazerReplay> LoadScores(Realm realm)
        => realm.All<ScoreInfo>().AsEnumerable().Select(scoreInfo => scoreInfo.ToLazerReplay());

    public override int CountBeatmapSets(Realm realm)
        => realm.All<BeatmapSetInfo>().Count();

    public override IEnumerable<IEnumerable<LazerBeatmap>> LoadBeatmapSets(Realm realm, IScoreDataManager scoreDatabase)
        => realm.All<BeatmapSetInfo>().AsEnumerable()
            .Select(beatmapSetInfo => beatmapSetInfo.ToLazerBeatmaps(scoreDatabase));

    public override IEnumerable<OsuCollection> ReadCollections(Realm realm, MapCacher mapCacher)
    {
        IRealmCollection<BeatmapCollection> allLazerCollections = realm.All<BeatmapCollection>().AsRealmCollection();

        foreach (BeatmapCollection lazerCollection in allLazerCollections)
        {
            OsuCollection collection = new(mapCacher)
            {
                Name = lazerCollection.Name,
                LazerId = lazerCollection.ID
            };

            foreach (string hash in lazerCollection.BeatmapMD5Hashes)
            {
                collection.AddBeatmapByHash(hash);
            }

            yield return collection;
        }
    }

    public override void WriteCollections(Realm realm, OsuCollections collections)
    {
        realm.Write(() =>
        {
            Dictionary<Guid, BeatmapCollection> existingById = realm.All<BeatmapCollection>()
                .ToDictionary(collection => collection.ID);

            List<(Guid Id, string Name, List<string> Hashes)> desired = collections
                .Select(collection => (
                    Id: collection.LazerId != Guid.Empty ? collection.LazerId : Guid.NewGuid(),
                    collection.Name,
                    Hashes: collection.AllBeatmaps().Select(beatmap => beatmap.Md5).ToList()))
                .ToList();
            
            foreach ((Guid id, string name, List<string> hashes) in desired)
            {
                if (existingById.TryGetValue(id, out BeatmapCollection realmCollection)
                    && realmCollection.Name == name
                    && realmCollection.BeatmapMD5Hashes.SequenceEqual(hashes))
                {
                    continue; // unchanged
                }

                bool isNew = realmCollection is null;
                realmCollection ??= new BeatmapCollection { ID = id };
                realmCollection.Name = name;
                realmCollection.LastModified = DateTimeOffset.Now;
                realmCollection.BeatmapMD5Hashes.Clear();

                foreach (string hash in hashes)
                {
                    realmCollection.BeatmapMD5Hashes.Add(hash);
                }

                if (isNew)
                {
                    _ = realm.Add(realmCollection);
                }
            }

            HashSet<Guid> desiredIds = [.. desired.Select(entry => entry.Id)];

            foreach (BeatmapCollection staleCollection in existingById.Values
                .Where(collection => !desiredIds.Contains(collection.ID)))
            {
                realm.Remove(staleCollection);
            }
        });
    }
}
