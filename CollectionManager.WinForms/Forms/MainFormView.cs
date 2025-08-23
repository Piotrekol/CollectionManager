namespace GuiComponents.Forms;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms;
using CollectionManager.WinForms.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

public partial class MainFormView : BaseForm, IMainFormView
{
    public MainFormView()
    {
        InitializeComponent();
        splitContainer1.Paint += WindowsFormsExtensions.SplitterPaint;
        splitContainer2.Paint += WindowsFormsExtensions.SplitterPaintHorizontal;
        AllowDrop = true;
        DragEnter += Form1_DragEnter;
        DragDrop += Form1_DragDrop;
    }

    public event GuiHelpers.LoadFileArgs OnLoadFile;
    public ICombinedListingView CombinedListingView => combinedListingView1;
    public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
    public IMainSidePanelView SidePanelView => mainSidePanelView1;
    public ICollectionTextView CollectionTextView => collectionTextView1;
    public IInfoTextView InfoTextView => infoTextView1;
    public IScoresListingView ScoresListingView => scoresListingView1;

    private void Form1_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.Data.GetFormats().Any(f => f == "FileDrop"))
        {
            e.Effect = DragDropEffects.Copy;
        }
    }

    private void Form1_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetFormats().All(f => f != "FileDrop"))
        {
            return;
        }

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        OnLoadFile?.Invoke(this, files);
    }

    protected override void OnShown(EventArgs e)
    {
        Activate();
        base.OnShown(e);
    }
}