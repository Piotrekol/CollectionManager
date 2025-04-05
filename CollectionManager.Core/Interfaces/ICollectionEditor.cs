namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public interface ICollectionEditor
{
    void EditCollection(CollectionEditArgs args);
    OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps);
}