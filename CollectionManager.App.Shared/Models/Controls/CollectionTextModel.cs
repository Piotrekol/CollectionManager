namespace CollectionManager.App.Shared.Models.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Core.Types;

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