namespace CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Common.Interfaces.Controls;
public interface IMainFormView : IForm
{
    event GuiHelpers.LoadFileArgs OnLoadFile;
    ICombinedListingView CombinedListingView { get; }
    ICombinedBeatmapPreviewView CombinedBeatmapPreviewView { get; }
    IMainSidePanelView SidePanelView { get; }
    ICollectionTextView CollectionTextView { get; }
    IInfoTextView InfoTextView { get; }
    IScoresListingView ScoresListingView { get; }
}