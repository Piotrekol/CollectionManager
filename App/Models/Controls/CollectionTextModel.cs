using System;
using CollectionManager.DataTypes;
using App.Interfaces;

namespace App.Models
{
    public class CollectionTextModel: ICollectionTextModel
    {
        public event EventHandler CollectionChanged;
        public void SetCollections(Collections collections)
        {
            Collections = collections;
        }

        private Collections _collections;

        public Collections Collections
        {
            get
            {
                return _collections;
            }
            set
            {
                _collections = value;
                OnCollectionChanged();
            }
        }

        protected virtual void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}