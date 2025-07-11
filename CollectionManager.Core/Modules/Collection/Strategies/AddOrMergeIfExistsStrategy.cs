namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class AddOrMergeIfExistsStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        foreach (IOsuCollection collection in args.NewCollections)
        {
            if (manager.CollectionNameExists(collection.Name))
            {
                IOsuCollection masterCollection = manager.GetCollectionByName(collection.Name);

                foreach (BeatmapExtension beatmap in collection.AllBeatmaps())
                {
                    masterCollection.AddBeatmap(beatmap);
                }
            }
            else
            {
                manager.EditCollection(CollectionEditArgs.AddCollections([collection]), true);
            }
        }
    }
}