namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Types;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

public class OsuDatabase
{
    public MapCacher LoadedMaps { get; } = new();
    private readonly OsuLazerDatabase _lazerDatabaseLoader;
    private readonly IScoreDataManager _scoresDatabase;

    public StableOsuDatabaseData StableOsuDatabaseData { get; private set; }

    public OsuDatabase(Beatmap beatmapBase, IScoreDataManager scoresDatabase)
    {
        _lazerDatabaseLoader = new OsuLazerDatabase(LoadedMaps, scoresDatabase);
        _scoresDatabase = scoresDatabase;
    }

    public bool Load(string fileDir, IProgress<string> progress, CancellationToken cancellationToken)
    {
        string fileExtension = Path.GetExtension(fileDir)?.ToLower(CultureInfo.InvariantCulture);

        switch (fileExtension)
        {
            case ".db":
                LoadStableBeatmaps(fileDir, progress, cancellationToken);
                break;
            case ".realm":
                _lazerDatabaseLoader.Load(fileDir, progress, cancellationToken);
                break;
            default:
                return false;
        }

        return true;
    }

    public void LoadStableBeatmaps(string fileDir, IProgress<string> progress, CancellationToken cancellationToken)
    {
        try
        {
            StableOsuDatabaseData = StableOsuDatabaseReader.ReadDatabase(fileDir, cancellationToken, progress);
            LoadedMaps.StoreBeatmaps(StableOsuDatabaseData.Beatmaps);
        }
        catch (Exception exception)
        {
            progress?.Report($"Something went wrong while processing beatmaps(database is corrupt or its format changed). {exception.Message}; {exception.StackTrace}");
            throw;
        }
    }
}
