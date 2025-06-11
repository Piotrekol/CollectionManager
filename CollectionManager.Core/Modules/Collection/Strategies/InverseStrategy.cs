namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;

public class InverseStrategy : ICollectionEditStrategy
{
    private readonly MapCacher _mapCacher;

    public InverseStrategy(MapCacher mapCacher)
    {
        _mapCacher = mapCacher;
    }

    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        List<IOsuCollection> argCollections = manager.GetCollectionByNames(args.CollectionNames);
        OsuCollection targetCollection = new(_mapCacher) { Name = args.NewName };
        IEnumerable<BeatmapExtension> beatmaps = _mapCacher.Beatmaps.AsEnumerable().Cast<BeatmapExtension>();

        foreach (IOsuCollection collection in argCollections)
        {
            beatmaps = beatmaps.Except(collection.AllBeatmaps(), new CollectionBeatmapComparer());
        }

        foreach (BeatmapExtension beatmap in beatmaps)
        {
            targetCollection.AddBeatmap(beatmap);
        }

        manager.EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
    }
}