namespace GuiComponents.Interfaces
{
    public interface IMainFormView :IForm
    {
        ICombinedListingView CombinedListingView { get; }
        ICombinedBeatmapPreviewView CombinedBeatmapPreviewView { get; }
        IMainSidePanelView SidePanelView { get; }
        ICollectionTextView CollectionTextView { get; }
        IInfoTextView InfoTextView { get; }

    }
}