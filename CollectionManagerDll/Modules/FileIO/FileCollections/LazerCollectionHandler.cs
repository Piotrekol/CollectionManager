using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Collections.Generic;

namespace CollectionManager.Modules.FileIO.FileCollections;

public class LazerCollectionHandler
    : OsuRealmReader
{
    public IEnumerable<Collection> Read(string realmFilePath, MapCacher mapCacher)
    {
        using Realm localRealm = GetRealm(realmFilePath);
        IRealmCollection<BeatmapCollection> allLazerCollections = localRealm.All<BeatmapCollection>().AsRealmCollection();

        foreach (BeatmapCollection lazerCollection in allLazerCollections)
        {
            Collection collection = new(mapCacher)
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

    public void Write(Collections collections, string realmFilePath)
    {
        using Realm localRealm = GetRealm(realmFilePath, false);

        localRealm.Write(() =>
        {
            localRealm.RemoveRange(localRealm.All<BeatmapCollection>());

            foreach (ICollection cmCollection in collections)
            {
                BeatmapCollection realmCollection = new()
                {
                    ID = Guid.NewGuid(),
                    Name = cmCollection.Name
                };

                foreach (BeatmapExtension beatmap in cmCollection.AllBeatmaps())
                {
                    realmCollection.BeatmapMD5Hashes.Add(beatmap.Md5);
                }

                _ = localRealm.Add(realmCollection);
            }
        });
    }
}
