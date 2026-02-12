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

    public bool Load(string filePath, IProgress<string> progress, CancellationToken cancellationToken)
    {
        string fileExtension = Path.GetExtension(filePath)?.ToLower(CultureInfo.InvariantCulture);

        switch (fileExtension)
        {
            case ".db":
                LoadStableBeatmaps(filePath, progress, cancellationToken);
                break;
            case ".realm":
                _lazerDatabaseLoader.Load(filePath, progress, cancellationToken);
                break;
            default:
                throw new InvalidOperationException($"Provided file path did not contain valid file extension. filePath: `{filePath}`");
        }

        return true;
    }

    private void LoadStableBeatmaps(string fileDir, IProgress<string> progress, CancellationToken cancellationToken)
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
