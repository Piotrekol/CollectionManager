using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.Modules.FileIO.FileCollections
{
    public class CollectionLoader
    {
        private readonly MapCacher _mapCacher;
        private readonly OsdbCollectionHandler OsdbCollectionHandler = new(null);
        private readonly OsuCollectionHandler OsuCollectionHandler = new(null);
        private readonly LazerCollectionHandler LazerCollectionHandler = new();

        public CollectionLoader(MapCacher mapCacher)
        {
            _mapCacher = mapCacher;
        }

        public Collections LoadOsuCollection(string fileLocation)
        {
            return OsuCollectionHandler.LoadCollections(fileLocation, _mapCacher);
        }

        public Collections LoadOsuLazerCollection(string fileLocation)
        {
            var collections = new Collections();
            collections.AddRange(LazerCollectionHandler.Read(fileLocation, _mapCacher));
            return collections;
        }

        public Collections LoadOsdbCollections(string fileLocation)
        {
            var collections = new Collections();
            collections.AddRange(OsdbCollectionHandler.ReadOsdb(fileLocation, _mapCacher));
            return collections;
        }

        public void SaveOsuCollection(Collections collections, string saveLocation)
        {
            OsuCollectionHandler.SaveCollections(collections, saveLocation);
        }

        public void SaveOsdbCollection(Collections collections, string saveLocation, string editorUsername = "N/A")
        {
            OsdbCollectionHandler.WriteOsdb(collections, saveLocation, editorUsername);
        }

        public void SaveOsuLazerCollection(Collections collections, string saveLocation)
        {
            LazerCollectionHandler.Write(collections, saveLocation);
        }

        public Collections LoadCollection(string fileLocation)
        {
            var ext = Path.GetExtension(fileLocation);

            switch (ext.ToLower())
            {
                case ".db":
                    return LoadOsuCollection(fileLocation);
                case ".osdb":
                    return LoadOsdbCollections(fileLocation);
                case ".realm":
                    return LoadOsuLazerCollection(fileLocation);
                default:
                    return null;
            }
        }

        public void SaveCollection(Collections collections, string fileLocation)
        {
            var ext = Path.GetExtension(fileLocation);

            switch (ext.ToLower())
            {
                case ".db":
                    SaveOsuCollection(collections, fileLocation);
                    break;
                case ".osdb":
                    SaveOsdbCollection(collections, fileLocation);
                    break;
                case ".realm":
                    SaveOsuLazerCollection(collections, fileLocation);
                    break;
                default:
                    return;
            }
        }
    }
}