namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;

public class DifferenceStrategy : ICollectionEditStrategy
{
    private readonly MapCacher _mapCacher;

    public DifferenceStrategy(MapCacher mapCacher)
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
            beatmaps = beatmaps.Concat(collection.AllBeatmaps());
        }

        List<string> differenceMd5 = beatmaps.GroupBy(x => x.Md5).Where(group => group.Count() == 1).Select(group => group.Key).ToList();
        List<int> differenceMapId = beatmaps.GroupBy(x => x.MapId).Where(group => group.Count() == 1).Select(group => group.Key).ToList();

        foreach (BeatmapExtension beatmap in beatmaps)
        {
            if (differenceMd5.Contains(beatmap.Md5) || differenceMapId.Contains(beatmap.MapId))
            {
                targetCollection.AddBeatmap(beatmap);
            }
        }

        manager.EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
    }
}