namespace CollectionManager.Core.Modules.FileIo;
using Realms;
using Realms.Exceptions;
using System.Text.RegularExpressions;

public partial class OsuRealmReader
{
    private const ulong _lastValidatedRealmSchemaVersion = 48;
    [GeneratedRegex("(\\d+)(?!.*\\d)")]
    private static partial Regex LastNumberRegex();

    protected static Realm GetRealm(string realmFilePath, bool readOnly = true)
    {
        RealmConfiguration config = new(realmFilePath)
        {
            IsReadOnly = readOnly,
            SchemaVersion = _lastValidatedRealmSchemaVersion
        };

        try
        {
            return Realm.GetInstance(config);
        }
        catch (RealmException exception)
        {
            const string RealmFileVersionMismatchMessage = "because it has a file format version";

            if (exception.Message.Contains(RealmFileVersionMismatchMessage))
            {
                throw new RealmNotValidatedException($"Opening osu!lazer database failed. Consider reporting this on github. {exception.Message}");
            }

            Match numberMatch = LastNumberRegex().Match(exception.Message);
            string schemaVersionOrMessage = numberMatch.Success
                ? numberMatch.Value
                : exception.Message;

            throw new RealmNotValidatedException($"Opening osu!lazer database failed. " +
                $"Expected schema version: '{_lastValidatedRealmSchemaVersion}', " +
                $"got: '{schemaVersionOrMessage}'. Consider reporting this on github.");
        }
    }
}
