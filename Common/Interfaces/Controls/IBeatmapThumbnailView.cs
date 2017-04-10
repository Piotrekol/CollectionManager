using System.Drawing;

namespace GuiComponents.Interfaces
{
    public interface IBeatmapThumbnailView
    {
        Image beatmapImage { set; }
        string AR { set; }
        string CS { set; }
        string OD { set; }
        string Stars { set; }
        string BeatmapName { set; }

    }
}