namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms;
using CollectionManager.WinForms.Forms;

public partial class BeatmapListingForm : BaseForm, IBeatmapListingForm
{
    public BeatmapListingForm()
    {
        InitializeComponent();
        splitContainer1.Paint += Helpers.SplitterPaint;
        splitContainer2.Paint += Helpers.SplitterPaintHorizontal;
    }

    public IBeatmapListingView BeatmapListingView => beatmapListingView1;
    public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
    public IScoresListingView ScoresListingView => scoresListingView1;
}
