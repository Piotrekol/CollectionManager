namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class RemoveStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        foreach (string collectionName in args.CollectionNames)
        {
            IOsuCollection collection = manager.GetCollectionByName(collectionName);

            if (collection is not null)
            {
                manager.LoadedCollections.SilentRemove(collection);
            }
        }
    }
}