namespace CollectionManager.Core.Modules.Collection.Strategies;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public class AddBeatmapsStrategy : ICollectionEditStrategy
{
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        IOsuCollection collection = manager.GetCollectionByName(args.CollectionNames[0]);

        if (collection is not null)
        {
            foreach (Beatmap beatmap in args.Beatmaps)
            {
                collection.AddBeatmap(beatmap);
            }
        }
    }
}