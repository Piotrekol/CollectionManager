using System;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;

namespace App.Interfaces
{
    public interface ICollectionListingModel
    {
        event EventHandler CollectionsChanged;
        event EventHandler SelectedCollectionsChanged;
        event EventHandler<CollectionEditArgs> CollectionEditing;
        Collections GetCollections();
        Collections SelectedCollections { get; set; }

        void EmitCollectionEditing(CollectionEditArgs args);
        Collections GetCollectionsForBeatmaps(Beatmaps beatmaps);

    }
}