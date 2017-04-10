namespace GuiComponents.Interfaces
{
    public interface IBeatmapListingForm : IForm
    {
        IBeatmapListingView BeatmapListingView { get; }
        ICombinedBeatmapPreviewView CombinedBeatmapPreviewView { get; }
    }
}