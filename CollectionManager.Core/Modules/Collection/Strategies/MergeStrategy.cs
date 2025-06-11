namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class MergeStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        List<IOsuCollection> argCollections = manager.GetCollectionByNames(args.CollectionNames);
        string newCollectionName = args.NewName;

        if (argCollections.Count > 0)
        {
            IOsuCollection masterCollection = argCollections[0];

            for (int i = 1; i < argCollections.Count; i++)
            {
                IOsuCollection collectionToMerge = argCollections[i];

                foreach (BeatmapExtension beatmap in collectionToMerge.AllBeatmaps())
                {
                    masterCollection.AddBeatmap(beatmap);
                }

                manager.LoadedCollections.SilentRemove(collectionToMerge);
            }

            manager.LoadedCollections.SilentRemove(masterCollection);

            masterCollection.Name = manager.GetValidCollectionName(newCollectionName);
            manager.EditCollection(CollectionEditArgs.AddCollections([masterCollection]), true);
        }
    }
}