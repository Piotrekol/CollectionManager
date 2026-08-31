namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.IO;

public class CollectionLoader
{
    private readonly MapCacher _mapCacher;
    private readonly OsuCollectionHandler OsuCollectionHandler = new(null);
    private readonly LazerCollectionHandler LazerCollectionHandler = new();

    public CollectionLoader(MapCacher mapCacher)
    {
        _mapCacher = mapCacher;
    }

    public OsuCollections LoadOsuCollection(string fileLocation) => OsuCollectionHandler.LoadCollections(fileLocation, _mapCacher);

    public RealmCollectionLoadResult LoadOsuLazerCollection(string fileLocation)
    {
        OsuCollections collections = [.. LazerCollectionHandler.Read(fileLocation, _mapCacher)];
        return new(collections, OsuRealmReader.LastLoadedSchemaVersion);
    }

    public DbCollectionLoadResult LoadOsdbCollections(string fileLocation)
        => OsdbCollectionHandler.ReadOsdb(fileLocation, _mapCacher);

    public void SaveOsuCollection(OsuCollections collections, string saveLocation) => OsuCollectionHandler.SaveCollections(collections, saveLocation);

    public void SaveOsdbCollection(OsuCollections collections, string saveLocation, string editorUsername = "N/A") => OsdbCollectionHandler.WriteOsdb(collections, saveLocation, editorUsername);

    public void SaveOsuLazerCollection(OsuCollections collections, string saveLocation)
        => SaveOsuLazerCollection(collections, saveLocation, LazerRealmSchemaVersion.LastLoaded);

    public void SaveOsuLazerCollection(OsuCollections collections, string saveLocation, LazerRealmSchemaVersion schemaVersion)
        => LazerCollectionHandler.Write(collections, saveLocation, schemaVersion);
    public CollectionLoadResult LoadCollection(string fileLocation)
    {
        string ext = Path.GetExtension(fileLocation);

        return ext.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            ".db" => new DbCollectionLoadResult(LoadOsuCollection(fileLocation) ?? [], OsuCollectionHandler.LastfileDate),
            ".osdb" => LoadOsdbCollections(fileLocation),
            ".realm" => LoadOsuLazerCollection(fileLocation),
            _ => throw new InvalidOperationException($"Provided file path did not contain valid file extension. filePath: `{fileLocation}`"),
        };
    }

    public void SaveCollection(OsuCollections collections, string filePath, LazerRealmSchemaVersion schemaVersion = LazerRealmSchemaVersion.LastLoaded)
    {
        string ext = Path.GetExtension(filePath);

        switch (ext.ToLower(System.Globalization.CultureInfo.CurrentCulture))
        {
            case ".db":
                SaveOsuCollection(collections, filePath);
                break;
            case ".osdb":
                SaveOsdbCollection(collections, filePath);
                break;
            case ".realm":
                SaveOsuLazerCollection(collections, filePath, schemaVersion);
                break;
            default:
                throw new InvalidOperationException($"Provided file path did not contain valid file extension. filePath: `{filePath}`");
        }
    }

    public OsuCollections LoadDefaultCollection(string osuDirectory)
        => LoadCollections(Path.Combine(osuDirectory, "collection.db"), Path.Combine(osuDirectory, "client.realm"));

    public OsuCollections LoadCollections(params string[] fileLocations)
    {
        if (fileLocations == null || fileLocations.Length == 0 || fileLocations.Any(string.IsNullOrWhiteSpace))
        {
            return null;
        }

        OsuCollections collections = [];

        foreach (string fileLocation in fileLocations.Where(File.Exists))
        {
            collections.AddRange(LoadCollection(fileLocation).Collections);
        }

        return collections;
    }
}