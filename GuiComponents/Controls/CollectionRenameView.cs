using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class CollectionRenameView : UserControl, ICollectionRenameView, ICollectionAddView
    {
        public CollectionRenameView()
        {
            InitializeComponent();

            textBox_newCollectionName.TextChanged += (s, a) => CollectionNameChanged?.Invoke(this, EventArgs.Empty);
            button_rename.Click +=Button_renameOnClick;
            button_cancel.Click += OnButton_CancelOnClick;
            ActiveControl = textBox_newCollectionName;
        }

        private void OnButton_CancelOnClick(object s, EventArgs a)
        {
            ActiveControl = textBox_newCollectionName;
            Canceled?.Invoke(this, EventArgs.Empty);
        }

        private void Button_renameOnClick(object sender, EventArgs e)
        {
            ActiveControl = textBox_newCollectionName;
            Submited?.Invoke(this, EventArgs.Empty);
        }

        private bool _isRenameView = true;
        [Description("Is this control used for renaming(true) or adding(false) new collection?"), Category("Layout")]
        public bool IsRenameView
        {
            get { return _isRenameView; }
            set
            {
                if (_isRenameView != value)
                {
                    //set control positions
                    int offset = value ? 25 : -25;

                    panel_Top.Location = new Point(panel_Top.Location.X, panel_Top.Location.Y + offset);
                    panel_bottom.Location = new Point(panel_bottom.Location.X, panel_bottom.Location.Y + offset);
                    Size = new Size(Size.Width, Size.Height + offset);

                    //change label & button text
                    button_rename.Text = value ? "Rename" : "Create";
                    label2.Text = value ? "New name:" : "Name:";
                }
                _isRenameView = value;
            }
        }

        public event EventHandler CollectionNameChanged;
        public event EventHandler Submited;
        public event EventHandler Canceled;

        public string NewCollectionName
        {
            get => textBox_newCollectionName.Text;
            set => textBox_newCollectionName.Text = value;
        }

        public string OrginalCollectionName
        {
            get
            {
                return label_orginalCollectionName.Text;
            }
            set { label_orginalCollectionName.Text = value; }
        }

        public string ErrorText
        {
            set
            {
                label_Error.Text = value;
                label_Error.Visible = !string.IsNullOrWhiteSpace(value);
            }
        }

        public bool CanSubmit
        {
            set { button_rename.Enabled = value; }
        }
    }
}
