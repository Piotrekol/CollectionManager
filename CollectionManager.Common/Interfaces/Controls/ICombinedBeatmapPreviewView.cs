namespace CollectionManager.Common.Interfaces.Controls;
public interface ICombinedBeatmapPreviewView
{
    IBeatmapThumbnailView BeatmapThumbnailView { get; }
    IMusicControlView MusicControlView { get; }
}