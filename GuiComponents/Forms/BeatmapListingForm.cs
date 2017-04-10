using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class BeatmapListingForm : BaseForm, IBeatmapListingForm
    {
        public BeatmapListingForm()
        {
            InitializeComponent();
        }

        public IBeatmapListingView BeatmapListingView => beatmapListingView1;
        public ICombinedBeatmapPreviewView CombinedBeatmapPreviewView => combinedBeatmapPreviewView1;
    }
}
