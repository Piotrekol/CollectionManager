namespace CollectionManagerApp.Interfaces.Controls;

using CollectionManager.Core.Types;

public interface ICollectionTextModel
{
    event EventHandler CollectionChanged;
    void SetCollections(OsuCollections collections);
    OsuCollections Collections { get; }
}