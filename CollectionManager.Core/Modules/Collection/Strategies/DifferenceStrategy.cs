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

        Dictionary<string, HashSet<int>> collectionsPerKey = [];
        Dictionary<string, BeatmapExtension> samplePerKey = [];

        for (int i = 0; i < argCollections.Count; i++)
        {
            foreach (BeatmapExtension beatmap in argCollections[i].AllBeatmaps())
            {
                string key = BeatmapIdentityComparer.KeyOf(beatmap);

                if (collectionsPerKey.TryGetValue(key, out HashSet<int>? collections))
                {
                    _ = collections.Add(i);
                }
                else
                {
                    collectionsPerKey[key] = [i];
                    samplePerKey[key] = beatmap;
                }
            }
        }

        foreach (KeyValuePair<string, HashSet<int>> entry in collectionsPerKey)
        {
            if (entry.Value.Count == 1)
            {
                targetCollection.AddBeatmap(samplePerKey[entry.Key]);
            }
        }

        manager.EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
    }
}