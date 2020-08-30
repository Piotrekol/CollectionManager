using System;
using System.Drawing;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class InfoTextView : UserControl, IInfoTextView
    {
        public InfoTextView()
        {
            InitializeComponent();

            label_UpdateText.Click += (s, a) => { UpdateTextClicked?.Invoke(this, EventArgs.Empty); };
        }

        public bool ColorUpdateText
        {
            set
            {
                if (value)
                {
                    label_UpdateText.Cursor = Cursors.Hand;
                    label_UpdateText.ForeColor = Color.Crimson;
                    label_UpdateText.Font = new Font(label_UpdateText.Font, FontStyle.Bold);
                }
                else
                {
                    label_UpdateText.Cursor = DefaultCursor;
                    label_UpdateText.ForeColor = DefaultForeColor;
                    label_UpdateText.Font = new Font(label_UpdateText.Font, FontStyle.Regular);
                }
            }
        }

        public string UpdateText { set { label_UpdateText.Text = value; } }
        public string CollectionManagerStatus { set { label_CollectionManagerStatus.Text = value; } }

        public event EventHandler UpdateTextClicked;
    }
}
