namespace GuiComponents.Interfaces
{
    public interface ICombinedListingView 
    {
        IBeatmapListingView beatmapListingView { get; }
        ICollectionListingView CollectionListingView { get; }
    }
}