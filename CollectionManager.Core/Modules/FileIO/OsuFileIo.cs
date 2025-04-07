namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Modules.FileIo.OsuScoresDb;
using CollectionManager.Core.Types;

public class OsuFileIo : IDisposable
{
    public OsuDatabase OsuDatabase { get; }
    public OsuSettingsLoader OsuSettings { get; } = new();
    public CollectionLoader CollectionLoader { get; }
    public MapCacher LoadedMaps => OsuDatabase.LoadedMaps;

    public ScoresDatabaseIo ScoresLoader { get; }
    public IScoreDataManager ScoresDatabase { get; } = new ScoresCacher();

    public OsuFileIo(Beatmap beatmapBase)
    {
        OsuDatabase = new OsuDatabase(beatmapBase, ScoresDatabase);
        CollectionLoader = new CollectionLoader(OsuDatabase.LoadedMaps);
        ScoresLoader = new ScoresDatabaseIo(ScoresDatabase);
    }

    public void Dispose()
    {
        ScoresLoader?.Dispose();
        GC.SuppressFinalize(this);
    }
}