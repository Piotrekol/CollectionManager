using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class CombinedBeatmapPreviewView : UserControl, ICombinedBeatmapPreviewView
    {
        public CombinedBeatmapPreviewView()
        {
            InitializeComponent();
        }

        public IBeatmapThumbnailView BeatmapThumbnailView => beatmapThumbnailView1;
        public IMusicControlView MusicControlView => musicControlView1;
    }
}
