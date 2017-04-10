 using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class MainFormView : BaseForm, IMainFormView
    {
        public MainFormView()
        {
            InitializeComponent();
            splitContainer1.Paint += Helpers.SplitterPaint;
        }

        public ICombinedListingView CombinedListingView => combinedListingView1;
        public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
        public IMainSidePanelView SidePanelView => mainSidePanelView1;
        public ICollectionTextView CollectionTextView => collectionTextView1;
        public IInfoTextView InfoTextView => infoTextView1;
    }
}
