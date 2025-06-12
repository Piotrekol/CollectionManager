namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;

public class ClearStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
        => manager.LoadedCollections.Clear();
}