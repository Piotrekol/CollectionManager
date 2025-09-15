namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.WinForms;
using System.Windows.Forms;

public partial class CombinedListingView : UserControl, ICombinedListingView
{
    public CombinedListingView()
    {
        InitializeComponent();
        splitContainer1.Paint += WindowsFormsExtensions.SplitterPaint;
    }

    public IBeatmapListingView beatmapListingView => beatmapListingView1;
    public ICollectionListingView CollectionListingView => collectionListingView1;
}
