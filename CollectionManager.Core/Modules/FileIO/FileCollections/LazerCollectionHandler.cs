namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.Collections.Generic;

public class LazerCollectionHandler
    : OsuRealmReader
{
    public IEnumerable<OsuCollection> Read(string realmFilePath, MapCacher mapCacher)
    {
        using Realm localRealm = GetRealm(realmFilePath);
        IRealmCollection<BeatmapCollection> allLazerCollections = localRealm.All<BeatmapCollection>().AsRealmCollection();

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

    public void Write(OsuCollections collections, string realmFilePath)
    {
        using Realm localRealm = GetRealm(realmFilePath, false);

        localRealm.Write(() =>
        {
            localRealm.RemoveRange(localRealm.All<BeatmapCollection>());

            foreach (IOsuCollection cmCollection in collections)
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
