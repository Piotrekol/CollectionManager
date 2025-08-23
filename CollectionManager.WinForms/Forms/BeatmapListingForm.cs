namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms;
using CollectionManager.WinForms.Forms;
using System.ComponentModel;

public partial class BeatmapListingForm : BaseForm, IBeatmapListingForm
{
    public BeatmapListingForm()
    {
        InitializeComponent();
        splitContainer1.Paint += WindowsFormsExtensions.SplitterPaint;
        splitContainer2.Paint += WindowsFormsExtensions.SplitterPaintHorizontal;
    }

    public IBeatmapListingView BeatmapListingView => beatmapListingView1;
    public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
    public IScoresListingView ScoresListingView => scoresListingView1;

    protected override void OnClosing(CancelEventArgs e)
    {
        (BeatmapListingView as IDisposable)?.Dispose();
        (CombinedBeatmapPreviewView as IDisposable)?.Dispose();
        (ScoresListingView as IDisposable)?.Dispose();
    }
}
