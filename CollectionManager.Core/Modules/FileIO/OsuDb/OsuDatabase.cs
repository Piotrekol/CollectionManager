namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Types;
using System;
using System.IO;
using System.Threading;

public class OsuDatabase
{
    public MapCacher LoadedMaps = new();
    private readonly OsuDatabaseLoader _osuDatabaseLoader;
    private readonly OsuLazerDatabase _lazerDatabaseLoader;
    private readonly IScoreDataManager _scoresDatabase;

    public string OsuFileLocation { get; private set; }
    public string SongsFolderLocation { get; set; }
    public int NumberOfBeatmapsWithoutId { get; set; }

    public bool DatabaseIsLoaded => _osuDatabaseLoader.LoadedSuccessfully;
    public string Status => ((LOsuDatabaseLoader)_osuDatabaseLoader).status;
    public int NumberOfBeatmaps => LoadedMaps.Beatmaps.Count;
    public string Username => _osuDatabaseLoader.Username;

    public OsuDatabase(Beatmap beatmapBase, IScoreDataManager scoresDatabase)
    {
        _osuDatabaseLoader = new LOsuDatabaseLoader(LoadedMaps, beatmapBase);
        _lazerDatabaseLoader = new OsuLazerDatabase(LoadedMaps, scoresDatabase);
        _scoresDatabase = scoresDatabase;
    }

    public bool Load(string fileDir, IProgress<string> progress, CancellationToken cancellationToken)
    {
        string fileExtension = Path.GetExtension(fileDir)?.ToLower();
        OsuFileLocation = fileDir;

        switch (fileExtension)
        {
            case ".db":
                _osuDatabaseLoader.LoadDatabase(fileDir, progress, cancellationToken);
                break;
            case ".realm":
                _lazerDatabaseLoader.Load(fileDir, progress, cancellationToken);
                break;
            default:
                OsuFileLocation = null;
                return false;
        }

        return true;
    }

    public void Load(string fileDir, IProgress<string> progress = null) => Load(fileDir, progress, CancellationToken.None);

}
