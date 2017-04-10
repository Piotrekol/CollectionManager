using System;
using CollectionManager.DataTypes;
using App.Interfaces;
using CollectionManager.Modules.CollectionsManager;

namespace App.Models
{
    public class CollectionListingModel : ICollectionListingModel
    {
        public event EventHandler CollectionsChanged;
        public event EventHandler SelectedCollectionsChanged;
        public event EventHandler<CollectionEditArgs> CollectionEditing;

        private Collections _collections;
        public CollectionListingModel(Collections collections)
        {
            SetCollections(collections);
        }
        public Collections GetCollections()
        {
            return _collections;
        }

        private Collections _selectedCollections;
        public Collections SelectedCollections
        {
            get { return _selectedCollections; }
            set
            {
                _selectedCollections = value;
                SelectedCollectionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void EmitCollectionEditing(CollectionEditArgs args)
        {
            CollectionEditing?.Invoke(this, args);
        }

        public void SetCollections(Collections collections)
        {
            _collections = collections;
            _collections.CollectionChanged += _collections_CollectionChanged;

            OnCollectionsChanged();
        }

        private void _collections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCollectionsChanged();
        }

        protected virtual void OnCollectionsChanged()
        {
            CollectionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}