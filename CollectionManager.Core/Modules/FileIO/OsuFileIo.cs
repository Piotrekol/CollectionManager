namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Modules.FileIo.OsuScoresDb;
using CollectionManager.Core.Types;

public class OsuFileIo
{
    public OsuDatabase OsuDatabase;
    public OsuSettingsLoader OsuSettings = new();
    public CollectionLoader CollectionLoader;
    public OsuPathResolver OsuPathResolver => OsuPathResolver.Instance;
    public MapCacher LoadedMaps => OsuDatabase.LoadedMaps;

    public ScoresDatabaseIo ScoresLoader;
    public IScoreDataManager ScoresDatabase = new ScoresCacher();

    public OsuFileIo(Beatmap beatmapBase)
    {
        OsuDatabase = new OsuDatabase(beatmapBase, ScoresDatabase);
        CollectionLoader = new CollectionLoader(OsuDatabase.LoadedMaps);
        ScoresLoader = new ScoresDatabaseIo(ScoresDatabase);
    }
}