using Realms;
using Realms.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace CollectionManager.Modules.FileIO;

public class OsuRealmReader
{
    private const ulong _lastValidatedRealmSchemaVersion = 46;
    private static readonly Regex _lastNumber = new("(\\d+)(?!.*\\d)", RegexOptions.Compiled);

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
            Match numberMatch = _lastNumber.Match(exception.Message);
            string schemaVersionOrMessage = numberMatch.Success 
                ? numberMatch.Value 
                : exception.Message;

            throw new RealmNotValidatedException($"Opening osu!lazer database failed." +
                $" Expected schema version: '{_lastValidatedRealmSchemaVersion}'," +
                $" got: '{schemaVersionOrMessage}'. Consider reporting this on github.");
        }
    }

    public class RealmNotValidatedException(string message)
        : Exception(message)
    {
    }
}
