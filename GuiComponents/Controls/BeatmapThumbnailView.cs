using System.Drawing;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class BeatmapThumbnailView : UserControl, IBeatmapThumbnailView
    {
        public BeatmapThumbnailView()
        {
            InitializeComponent();
        }

        public Image beatmapImage
        {
            set
            {
                var oldImage = pictureBox1.Image;
                pictureBox1.Image = value;
                if(pictureBox1.ImageLocation==null)
                    oldImage?.Dispose();
            }
        }

        public string beatmapImageUrl
        {
            set
            {
                if (pictureBox1.Image == null)
                {
                    pictureBox1.ImageLocation = value;
                }
            }
        }

        public string BasicStats { set { Invoke((MethodInvoker)(() => { label_BasicStats.Text = value; })); } }
        public string BeatmapName { set { Invoke((MethodInvoker)(() => { label_BeatmapName.Text = value; })); } }
        public string AdditionalStatsRtf { set { Invoke((MethodInvoker)(() => { richTextBox_AdditionalStats.Rtf = value; })); } }
    }
}
