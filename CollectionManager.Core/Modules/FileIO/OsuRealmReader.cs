namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using Realms;
using Realms.Exceptions;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public partial class OsuRealmReader
{
    private const string RealmFileVersionMismatchMessage = "because it has a file format version";

    private static readonly LazerRealmAdapter BaseAdapter = new LazerRealmAdapter51();

    /// <summary>
    /// Supported osu!lazer realm schema versions.
    /// </summary>
    /// <remarks>
    /// When adding new version, if realm:<br/>
    /// Only added tables/properties - reuse <see cref="BaseAdapter"/>;<br/>
    /// Removed/renamed any property - copy the changed model classes to
    /// RealmModels/vYY/, and add <code>LazerRealmAdapterVYY: LazerRealmAdapter51</code> with overriden ObjectTypes.
    /// </remarks>
    private static readonly (LazerRealmSchemaVersion Version, LazerRealmAdapter Adapter)[] SupportedSchemaVersions =
    [
        (LazerRealmSchemaVersion.V51, BaseAdapter),
        (LazerRealmSchemaVersion.V52, BaseAdapter),
    ];

    private static LazerRealmSchemaVersion _lastLoadedSchemaVersion = LazerRealmSchemaVersion.Latest;

    internal static LazerRealmSchemaVersion LastLoadedSchemaVersion => _lastLoadedSchemaVersion;

    [GeneratedRegex("(\\d+)(?!.*\\d)")]
    private static partial Regex LastNumberRegex();

    internal static LazerRealm OpenRealm(
        string realmFilePath,
        bool readOnly = true,
        LazerRealmSchemaVersion targetSchemaVersion = LazerRealmSchemaVersion.LastLoaded)
    {
        if (!readOnly && !File.Exists(realmFilePath))
        {
            return OpenRealmForNewFile(realmFilePath, targetSchemaVersion);
        }

        RealmException lastSchemaException = null;

        foreach ((LazerRealmSchemaVersion realmSchemaVersion, LazerRealmAdapter realmAdapter) in SupportedSchemaVersions)
        {
            RealmConfiguration config = new(realmFilePath)
            {
                IsReadOnly = readOnly,
                SchemaVersion = (ulong)realmSchemaVersion,
                Schema = realmAdapter.ObjectTypes,
            };

            try
            {
                return CreateLazerRealm(config, realmAdapter, realmSchemaVersion);
            }
            catch (RealmMismatchedConfigException)
            {
                throw new RealmNotValidatedException(
                    $"Opening osu!lazer database failed. '{realmFilePath}' is already open in this process " +
                    $"with a different configuration - finish or dispose that operation first (e.g. a load still in progress).");
            }
            catch (RealmException exception)
            {
                if (exception.Message.Contains(RealmFileVersionMismatchMessage))
                {
                    throw new RealmNotValidatedException($"Opening osu!lazer database failed. Consider reporting this on github. {exception.Message}");
                }

                lastSchemaException = exception;
            }
        }

        Match numberMatch = LastNumberRegex().Match(lastSchemaException.Message);
        string schemaVersionOrMessage = numberMatch.Success
            ? numberMatch.Value
            : lastSchemaException.Message;
        string supportedVersions = string.Join(", ", SupportedSchemaVersions.Select(entry => (ulong)entry.Version));

        throw new RealmNotValidatedException($"Opening osu!lazer database failed. " +
            $"Supported schema versions: '{supportedVersions}', " +
            $"got: '{schemaVersionOrMessage}'. Consider reporting this on github.");
    }

    private static LazerRealm OpenRealmForNewFile(string realmFilePath, LazerRealmSchemaVersion targetSchemaVersion)
    {
        LazerRealmSchemaVersion resolvedSchemaVersion = ResolveNewFileSchemaVersion(targetSchemaVersion, _lastLoadedSchemaVersion);

        (LazerRealmSchemaVersion schemaVersion, LazerRealmAdapter adapter) = SupportedSchemaVersions
            .Single(entry => entry.Version == resolvedSchemaVersion);

        RealmConfiguration config = new(realmFilePath)
        {
            IsReadOnly = false,
            SchemaVersion = (ulong)schemaVersion,
            Schema = adapter.ObjectTypes,
        };

        return CreateLazerRealm(config, adapter, schemaVersion);
    }

    private static LazerRealmSchemaVersion ResolveNewFileSchemaVersion(LazerRealmSchemaVersion targetSchemaVersion, LazerRealmSchemaVersion lastLoadedSchemaVersion)
        => targetSchemaVersion switch
        {
            LazerRealmSchemaVersion.LastLoaded when lastLoadedSchemaVersion is >= 0 => lastLoadedSchemaVersion,
            LazerRealmSchemaVersion.LastLoaded or LazerRealmSchemaVersion.Latest => SupportedSchemaVersions[^1].Version,
            _ => targetSchemaVersion,
        };

    private static LazerRealm CreateLazerRealm(
        RealmConfiguration config,
        LazerRealmAdapter adapter,
        LazerRealmSchemaVersion realmSchemaVersion)
    {
        LazerRealm lazerRealm = new(Realm.GetInstance(config), adapter, realmSchemaVersion);
        _lastLoadedSchemaVersion = realmSchemaVersion;

        return lazerRealm;
    }
}
