namespace CollectionManager.WinForms;

using CollectionManager.WinForms.Forms;

public partial class TextBoxForm : BaseForm
{
    public TextBoxForm()
    {
        InitializeComponent();
    }

    internal static void ShowDialog(string text, string caption)
    {
        using TextBoxForm form = new()
        {
            Text = $"Collection Manager - {caption}"
        };

        form.richTextBox_contents.Text = text;
        _ = form.ShowDialog();
    }
}
