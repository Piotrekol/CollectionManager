namespace GuiComponents.Interfaces
{
    public interface ICombinedBeatmapPreviewView
    {
        IBeatmapThumbnailView BeatmapThumbnailView { get; }
        IMusicControlView MusicControlView { get; }
    }
}