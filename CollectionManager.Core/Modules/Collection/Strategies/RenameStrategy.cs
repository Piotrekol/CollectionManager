namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class RenameStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        IOsuCollection collection = manager.GetCollectionByName(args.CollectionNames[0]);
        collection.Name = manager.GetValidCollectionName(args.NewName);
    }
}