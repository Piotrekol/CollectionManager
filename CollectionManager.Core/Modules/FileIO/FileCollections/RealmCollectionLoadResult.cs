namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Types;

public sealed record RealmCollectionLoadResult(OsuCollections Collections, LazerRealmSchemaVersion RealmSchemaVersion)
    : CollectionLoadResult(Collections);
