using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;

namespace App.Interfaces
{
    public interface ICollectionEditor
    {
        void EditCollection(CollectionEditArgs args);
        Collections GetCollectionsForBeatmaps(Beatmaps beatmaps);
    }
}