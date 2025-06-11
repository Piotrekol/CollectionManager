namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;

public class IntersectStrategy : ICollectionEditStrategy
{
    private readonly MapCacher _mapCacher;

    public IntersectStrategy(MapCacher mapCacher)
    {
        _mapCacher = mapCacher;
    }

    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        List<IOsuCollection> argCollections = manager.GetCollectionByNames(args.CollectionNames);
        OsuCollection targetCollection = new(_mapCacher) { Name = args.NewName };
        IOsuCollection mainCollection = argCollections[0];
        argCollections.RemoveAt(0);
        IEnumerable<BeatmapExtension> beatmaps = mainCollection.AllBeatmaps();

        foreach (IOsuCollection collection in argCollections)
        {
            beatmaps = beatmaps.Intersect(collection.AllBeatmaps(), new CollectionBeatmapComparer()).ToList();
        }

        foreach (BeatmapExtension beatmap in beatmaps)
        {
            targetCollection.AddBeatmap(beatmap);
        }

        manager.EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
    }
}