using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;

namespace CollectionManager.Modules.FileIO.FileCollections;

public class LazerCollectionHandler
    : OsuRealmReader
{
    public IEnumerable<Collection> Read(string realmFilePath, MapCacher mapCacher)
    {
        using var localRealm = GetRealm(realmFilePath);
        var allLazerCollections = localRealm.All<BeatmapCollection>().AsRealmCollection();

        foreach (var lazerCollection in allLazerCollections)
        {
            var collection = new Collection(mapCacher)
            {
                Name = lazerCollection.Name,
                LazerId = lazerCollection.ID
            };

            foreach (var hash in lazerCollection.BeatmapMD5Hashes)
            {
                collection.AddBeatmapByHash(hash);
            }

            yield return collection;
        }
    }

    public void Write(Collections collections, string realmFilePath)
    {
        // TODO: only allow for writes for validated schemaVersion(s)?
        // TODO: backups..
        using var localRealm = GetRealm(realmFilePath, false);

        localRealm.Write(() =>
        {
            localRealm.RemoveRange(localRealm.All<BeatmapCollection>());

            foreach (var ourCollection in collections)
            {
                var realmCollection = new BeatmapCollection
                {
                    ID = Guid.NewGuid(),
                    Name = ourCollection.Name
                };

                foreach (var beatmap in ourCollection.AllBeatmaps())
                {
                    realmCollection.BeatmapMD5Hashes.Add(beatmap.Md5);
                }

                localRealm.Add(realmCollection);
            }
        });
    }

}
