namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class AddOrMergeIfExistsStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        List<IOsuCollection> argCollections = manager.GetCollectionByNames(args.CollectionNames);

        foreach (IOsuCollection collection in argCollections)
        {
            if (manager.CollectionNameExists(collection.Name))
            {
                manager.EditCollection(CollectionEditArgs.MergeCollections([collection.Name, collection.Name], collection.Name), true);
            }
            else
            {
                manager.EditCollection(CollectionEditArgs.AddCollections([collection]), true);
            }
        }
    }
}