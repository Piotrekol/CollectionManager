using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.Modules.FileIO.FileCollections
{
    public class CollectionLoader
    {
        private readonly MapCacher _mapCacher;
        private OsdbCollectionHandler OsdbCollectionHandler = new OsdbCollectionHandler(null);
        private OsuCollectionHandler OsuCollectionHandler = new OsuCollectionHandler(null);

        public CollectionLoader(MapCacher mapCacher)
        {
            _mapCacher = mapCacher;
        }

        public Collections LoadOsuCollection(string fileLocation)
        {
            return OsuCollectionHandler.LoadCollections(fileLocation, _mapCacher);
        }

        public Collections LoadOsdbCollections(string fileLocation)
        {
            return OsdbCollectionHandler.ReadOsdb(fileLocation, _mapCacher);
        }

        public void SaveOsuCollection(Collections collections, string saveLocation)
        {
            OsuCollectionHandler.SaveCollections(collections, saveLocation);
        }
        public void SaveOsdbCollection(Collections collections, string saveLocation, string editorUsername = "N/A")
        {
            OsdbCollectionHandler.WriteOsdb(collections, saveLocation, editorUsername);
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
                default:
                    return null;
            }
        }

        public void SaveCollection(Collections collections,string fileLocation)
        {
            var ext = Path.GetExtension(fileLocation);
            switch (ext.ToLower())
            {
                case ".db":
                    SaveOsuCollection(collections,fileLocation);
                    break;
                case ".osdb":
                    SaveOsdbCollection(collections,fileLocation);
                    break;
                default:
                    return;
            }
        }
    }
}