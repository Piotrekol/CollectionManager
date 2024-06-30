using Realms;
using System;
using System.Linq;

namespace CollectionManager.Modules.FileIO;

public class OsuRealmReader
{
    public const ulong LAST_VALIDATED_REALM_VERSION = 41;

    protected static Realm GetRealm(string realmFilePath, bool readOnly = true, ulong schemaVersion = 0)
    {
        var config = new RealmConfiguration(realmFilePath)
        {
            IsReadOnly = readOnly,
            SchemaVersion = schemaVersion
        };

        try
        {
            return Realm.GetInstance(config);
        }
        catch (Exception ex) when (schemaVersion == 0)
        {
            // TODO: There has to be a better way to get current schema version...
            var expectedSchemaVersionRaw = ex.Message?.Split(' ').Last().TrimEnd('.');

            if (ulong.TryParse(expectedSchemaVersionRaw, out var expectedSchemaVersion) && expectedSchemaVersion > 0)
                return GetRealm(realmFilePath, readOnly, expectedSchemaVersion);

            throw;
        }
    }
}
