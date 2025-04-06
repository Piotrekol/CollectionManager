namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces.Controls;
using System;
using System.Windows.Forms;

public partial class CollectionTextView : UserControl, ICollectionTextView
{

    public event EventHandler SaveTypeChanged;
    public event EventHandler IsVisibleChanged;

    public void SetListTypes(Array types) => comboBox_textType.DataSource = types;

    public string GeneratedText { set => textBox1.Text = value; }
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

    public bool IsVisible => Visible;

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == (Keys.Control | Keys.A))
        {
            textBox1.SelectAll();
            e.Handled = e.SuppressKeyPress = true;
        }
    }

    private void CollectionTextView_VisibleChanged(object sender, EventArgs e) => IsVisibleChanged?.Invoke(this, EventArgs.Empty);
}
