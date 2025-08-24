namespace CollectionManager.App.Shared.Interfaces.Controls;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public interface ICollectionListingModel
{
    event EventHandler CollectionsChanged;
    event EventHandler SelectedCollectionsChanged;
    event EventHandler<CollectionEditArgs> CollectionEditing;
    OsuCollections GetCollections();
    OsuCollections SelectedCollections { get; set; }

    void EmitCollectionEditing(CollectionEditArgs args);
    OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps);

}