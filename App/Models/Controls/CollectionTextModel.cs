namespace CollectionManagerApp.Models.Controls;

using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;

public class CollectionTextModel : ICollectionTextModel
{
    public event EventHandler CollectionChanged;
    public void SetCollections(OsuCollections collections) => Collections = collections;

    private OsuCollections _collections;

    public OsuCollections Collections
    {
        get => _collections;
        set
        {
            _collections = value;
            OnCollectionChanged();
        }
    }

    protected virtual void OnCollectionChanged() => CollectionChanged?.Invoke(this, EventArgs.Empty);
}