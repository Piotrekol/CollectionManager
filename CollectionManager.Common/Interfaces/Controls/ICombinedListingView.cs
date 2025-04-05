namespace CollectionManager.Common.Interfaces.Controls;
public interface ICombinedListingView
{
    IBeatmapListingView beatmapListingView { get; }
    ICollectionListingView CollectionListingView { get; }
}