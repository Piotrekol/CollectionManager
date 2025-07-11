namespace CollectionManager.Common.Interfaces.Forms;

using CollectionManager.Common.Interfaces.Controls;

public interface IBeatmapListingForm : IForm
{
    IBeatmapListingView BeatmapListingView { get; }
    ICombinedBeatmapPreviewView CombinedBeatmapPreviewView { get; }
    IScoresListingView ScoresListingView { get; }
}