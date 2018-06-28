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

        public string AR { set { Invoke((MethodInvoker)(() => { label_AR.Text = value; })); } }
        public string CS { set { Invoke((MethodInvoker)(() => { label_CS.Text = value; })); } }
        public string OD { set { Invoke((MethodInvoker)(() => { label_OD.Text = value; })); } }
        public string Stars { set { Invoke((MethodInvoker)(() => { label_Stars.Text = value; })); } }
        public string BeatmapName { set { Invoke((MethodInvoker)(() => { label_BeatmapName.Text = value; })); } }
    }
}
