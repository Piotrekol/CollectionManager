using System;
using CollectionManager.DataTypes;

namespace App.Interfaces
{
    public interface ICollectionTextModel
    {
        event EventHandler CollectionChanged;
        void SetCollections(Collections collections);
        Collections Collections { get; }
    }
}