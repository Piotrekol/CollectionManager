using System.Drawing;

namespace GuiComponents.Interfaces
{
    public interface IBeatmapThumbnailView
    {
        Image beatmapImage { set; }
        string beatmapImageUrl { set; }
        string BasicStats { set; }
        string BeatmapName { set; }
        string AdditionalStatsRtf { set; }

    }
}