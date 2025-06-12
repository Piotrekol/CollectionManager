namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;

public class DuplicateStrategy : ICollectionEditStrategy
{
    private readonly MapCacher _mapCacher;

    public DuplicateStrategy(MapCacher mapCacher)
    {
        _mapCacher = mapCacher;
    }

    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        IOsuCollection sourceCollection = manager.GetCollectionByName(args.CollectionNames[0]);
        OsuCollection newCollection = new(_mapCacher) { Name = args.NewName };

        foreach (BeatmapExtension beatmap in sourceCollection.AllBeatmaps())
        {
            newCollection.AddBeatmap(beatmap);
        }

        manager.EditCollection(CollectionEditArgs.AddCollections([newCollection]), true);
    }
}