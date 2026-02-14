namespace CollectionManager.Core.Modules.FileIo.FileCollections;

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

    public OsuCollections LoadOsuLazerCollection(string fileLocation)
    {
        OsuCollections collections = [.. LazerCollectionHandler.Read(fileLocation, _mapCacher)];
        return collections;
    }

    public OsuCollections LoadOsdbCollections(string fileLocation)
    {
        OsuCollections collections = [.. OsdbCollectionHandler.ReadOsdb(fileLocation, _mapCacher)];
        return collections;
    }

    public void SaveOsuCollection(OsuCollections collections, string saveLocation) => OsuCollectionHandler.SaveCollections(collections, saveLocation);

    public void SaveOsdbCollection(OsuCollections collections, string saveLocation, string editorUsername = "N/A") => OsdbCollectionHandler.WriteOsdb(collections, saveLocation, editorUsername);

    public void SaveOsuLazerCollection(OsuCollections collections, string saveLocation) => LazerCollectionHandler.Write(collections, saveLocation);

    public OsuCollections LoadCollection(string fileLocation)
    {
        string ext = Path.GetExtension(fileLocation);

        return ext.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            ".db" => LoadOsuCollection(fileLocation),
            ".osdb" => LoadOsdbCollections(fileLocation),
            ".realm" => LoadOsuLazerCollection(fileLocation),
            _ => null,
        };
    }

    public void SaveCollection(OsuCollections collections, string filePath)
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
                SaveOsuLazerCollection(collections, filePath);
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
            collections.AddRange(LoadCollection(fileLocation));
        }

        return collections;
    }
}