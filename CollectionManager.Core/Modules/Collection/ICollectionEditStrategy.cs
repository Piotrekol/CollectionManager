namespace CollectionManager.Core.Modules.Collection;

public interface ICollectionEditStrategy
{
    void Execute(CollectionsManager manager, CollectionEditArgs args);
}
