namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class AddStrategy : ICollectionEditStrategy
{
    private int _collectionLoadId;

    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        List<string> collectionNames = [];

        foreach (IOsuCollection collection in args.NewCollections)
        {
            string name = manager.GetValidCollectionName(collection.Name, collectionNames);

            collection.Name = name;
            collection.Id = _collectionLoadId++;
            collectionNames.Add(name);
        }

        manager.LoadedCollections.AddRange(args.NewCollections);
    }
}