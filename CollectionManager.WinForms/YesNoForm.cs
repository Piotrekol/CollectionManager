namespace GuiComponents;

using CollectionManager.WinForms.Forms;
using System;
using System.Windows.Forms;

public partial class YesNoForm : BaseForm
{
    public bool DoNotAskAgain { get; private set; }
    public YesNoForm()
    {
        InitializeComponent();
    }

    internal static (DialogResult dialogResult, bool doNotAskAgain) ShowDialog(string text, string caption,
        string doNotAskAgainText)
    {
        using YesNoForm f = new();
        f.label_text.Text = text;
        f.Text = $"Collection Manager - {caption}";
        f.checkBox_doNotAskAgain.Text = doNotAskAgainText;

        return (f.ShowDialog(), f.DoNotAskAgain);
    }

    private void CheckBox_doNotAskAgain_CheckedChanged(object sender, EventArgs e) => DoNotAskAgain = checkBox_doNotAskAgain.Checked;
}
