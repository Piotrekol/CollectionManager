using System.Collections.Generic;
using CollectionManager.DataTypes;

namespace GuiComponents.Interfaces
{
    public interface IOnlineCollectionList
    {
        RangeObservableCollection<WebCollection> Collections { get; }
    }
}