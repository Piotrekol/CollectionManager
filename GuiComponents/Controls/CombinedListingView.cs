using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class CombinedListingView : UserControl, ICombinedListingView
    {
        public CombinedListingView()
        {
            InitializeComponent();
            splitContainer1.Paint += Helpers.SplitterPaint;
        }

        public IBeatmapListingView beatmapListingView => beatmapListingView1;
        public ICollectionListingView CollectionListingView => collectionListingView1;
    }
}
