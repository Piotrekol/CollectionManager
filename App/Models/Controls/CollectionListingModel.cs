namespace CollectionManagerApp.Models.Controls;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;

public class CollectionListingModel : ICollectionListingModel
{
    private readonly ICollectionEditor _collectionEditor;
    public event EventHandler CollectionsChanged;
    public event EventHandler SelectedCollectionsChanged;
    public event EventHandler<CollectionEditArgs> CollectionEditing;

    private OsuCollections _collections;
    public CollectionListingModel(OsuCollections collections, ICollectionEditor collectionEditor)
    {
        _collectionEditor = collectionEditor;
        SetCollections(collections);
    }
    public OsuCollections GetCollections() => _collections;

    private OsuCollections _selectedCollections;
    public OsuCollections SelectedCollections
    {
        get => _selectedCollections;
        set
        {
            _selectedCollections = value;
            SelectedCollectionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void EmitCollectionEditing(CollectionEditArgs args) => CollectionEditing?.Invoke(this, args);

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
        => _collectionEditor.GetCollectionsForBeatmaps(beatmaps);

    public void SetCollections(OsuCollections collections)
    {
        _collections = collections;
        _collections.CollectionChanged += _collections_CollectionChanged;

        OnCollectionsChanged();
    }

    private void _collections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => OnCollectionsChanged();

    protected virtual void OnCollectionsChanged() => CollectionsChanged?.Invoke(this, EventArgs.Empty);
}