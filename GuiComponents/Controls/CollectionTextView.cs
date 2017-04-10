using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class CollectionTextView : UserControl, ICollectionTextView
    {

        public event EventHandler SaveTypeChanged;
        public void SetListTypes(Array types)
        {
            comboBox_textType.DataSource = types;
        }

        public string GeneratedText { set { textBox1.Text = value; } }
        public CollectionTextView()
        {
            InitializeComponent();
            textBox1.KeyDown += textBox1_KeyDown;
            comboBox_textType.SelectedIndexChanged += delegate
            {
                SaveTypeChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public string SelectedSaveType => comboBox_textType.SelectedValue.ToString();
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.A))
            {
                textBox1.SelectAll();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

    }
}
