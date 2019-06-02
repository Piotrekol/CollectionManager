using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Modules.API.osustats;

namespace GuiComponents.Interfaces
{
    public interface IOnlineCollectionList
    {
        RangeObservableCollection<WebCollection> WebCollections { get; }
        RangeObservableCollection<Collection> Collections { set; }
        UserInformation UserInformation { set; }
    }
}