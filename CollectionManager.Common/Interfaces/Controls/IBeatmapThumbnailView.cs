namespace CollectionManager.Common.Interfaces.Controls;

using System.Drawing;

public interface IBeatmapThumbnailView
{
    Image beatmapImage { set; }
    string beatmapImageUrl { set; }
    string AR { set; }
    string CS { set; }
    string OD { set; }
    string Stars { set; }
    string BeatmapName { set; }
}