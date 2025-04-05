namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces.Controls;
using System.Windows.Forms;

public partial class CombinedBeatmapPreviewView : UserControl, ICombinedBeatmapPreviewView
{
    public CombinedBeatmapPreviewView()
    {
        InitializeComponent();
    }

    public IBeatmapThumbnailView BeatmapThumbnailView => beatmapThumbnailView1;
    public IMusicControlView MusicControlView => musicControlView1;
}
