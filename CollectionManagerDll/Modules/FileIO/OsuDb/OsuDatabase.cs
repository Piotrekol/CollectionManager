using CollectionManager.DataTypes;
using System;
using System.Threading;

namespace CollectionManager.Modules.FileIO.OsuDb
{
    public class OsuDatabase
    {
        public MapCacher LoadedMaps = new MapCacher();
        private readonly OsuDatabaseLoader _databaseLoader;
        public string OsuFileLocation { get; private set; }
        public string SongsFolderLocation { get; set; }
        public int NumberOfBeatmapsWithoutId { get; set; }

        public bool DatabaseIsLoaded => _databaseLoader.LoadedSuccessfully;
        public string Status => ((LOsuDatabaseLoader)_databaseLoader).status;
        public int NumberOfBeatmaps => LoadedMaps.Beatmaps.Count;
        public string Username => _databaseLoader.Username;

        public OsuDatabase(Beatmap beatmapBase)
        {
            _databaseLoader = new LOsuDatabaseLoader(LoadedMaps, beatmapBase);
        }

        public void Load(string fileDir, IProgress<string> progress, CancellationToken cancellationToken)
        {
            OsuFileLocation = fileDir;
            _databaseLoader.LoadDatabase(fileDir, progress, cancellationToken);
        }

        public void Load(string fileDir, IProgress<string> progress = null) => Load(fileDir, progress, CancellationToken.None);

    }
}
