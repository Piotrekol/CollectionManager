namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using Realms;
using System;

internal sealed record LazerRealm(Realm Realm, LazerRealmAdapter Adapter, LazerRealmSchemaVersion SchemaVersion)
    : IDisposable
{
    public void Dispose() => Realm.Dispose();
}
