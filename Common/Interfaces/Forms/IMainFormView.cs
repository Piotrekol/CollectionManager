using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IMainFormView :IForm
    {
        event GuiHelpers.LoadFileArgs OnLoadFile;
        ICombinedListingView CombinedListingView { get; }
        ICombinedBeatmapPreviewView CombinedBeatmapPreviewView { get; }
        IMainSidePanelView SidePanelView { get; }
        ICollectionTextView CollectionTextView { get; }
        IInfoTextView InfoTextView { get; }

    }
}