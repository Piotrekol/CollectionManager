namespace CollectionManager.Common.Interfaces;

using CollectionManager.Core.Types;

public interface IOnlineCollectionList
{
    RangeObservableCollection<WebCollection> WebCollections { get; }
    RangeObservableCollection<OsuCollection> Collections { set; }
    IUserInformation UserInformation { set; }
}