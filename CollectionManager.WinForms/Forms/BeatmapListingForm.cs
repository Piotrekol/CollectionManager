namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class BeatmapListingForm : BaseForm, IBeatmapListingForm
{
    public BeatmapListingForm()
    {
        InitializeComponent();
    }

    public IBeatmapListingView BeatmapListingView => beatmapListingView1;
    public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
}
