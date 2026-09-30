namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Types;
using System.Collections.Generic;

public class LazerCollectionHandler
    : OsuRealmReader
{
    public IEnumerable<OsuCollection> Read(string realmFilePath, MapCacher mapCacher)
    {
        using LazerRealm lazerRealm = OpenRealm(realmFilePath);

        foreach (OsuCollection collection in lazerRealm.Adapter.ReadCollections(lazerRealm.Realm, mapCacher))
        {
            yield return collection;
        }
    }

    public void Write(OsuCollections collections, string realmFilePath, LazerRealmSchemaVersion schemaVersion = LazerRealmSchemaVersion.LastLoaded)
    {
        using LazerRealm lazerRealm = OpenRealm(realmFilePath, false, schemaVersion);
        lazerRealm.Adapter.WriteCollections(lazerRealm.Realm, collections);
    }
}
