using CollectionManager.Modules.FileIO.FileCollections;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.Modules.FileIO
{
    public class OsuFileIo
    {
        public OsuDatabase OsuDatabase = new OsuDatabase();
        public OsuSettingsLoader OsuSettings = new OsuSettingsLoader();
        public CollectionLoader CollectionLoader;
        public OsuPathResolver OsuPathResolver => OsuPathResolver.Instance;
        public MapCacher LoadedMaps => OsuDatabase.LoadedMaps;

        public OsuFileIo()
        {
            CollectionLoader = new CollectionLoader(OsuDatabase.LoadedMaps);
        }
    }
}