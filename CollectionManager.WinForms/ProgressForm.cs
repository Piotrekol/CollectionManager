namespace GuiComponents;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;
using System;
using System.Windows.Forms;

public partial class ProgressForm : BaseForm, IProgressForm
{
    public event EventHandler AbortClicked;
    public ProgressForm()
    {
        InitializeComponent();
    }

    private const int CP_NOCLOSE_BUTTON = 0x200;
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams myCp = base.CreateParams;
            myCp.ClassStyle |= CP_NOCLOSE_BUTTON;
            return myCp;
        }
    }
    internal static IProgressForm ShowDialog(Progress<string> userProgressMessage, Progress<int> completionPercentage)
    {
        ProgressForm form = new();
        userProgressMessage.ProgressChanged += (_, message) => form.label_text.Text = message;
        completionPercentage.ProgressChanged += form.CompletionPercentage_ProgressChanged;
        form.button_cancel.Click += (_, __) => form.AbortClicked?.Invoke(form, EventArgs.Empty);

        form.Text = $"Collection Manager - Beatmap Export";

        return form;
    }

    private void CompletionPercentage_ProgressChanged(object sender, int percentage)
    {
        progressBar1.Value = percentage;
        if (percentage == progressBar1.Maximum)
        {
            button_cancel.Text = "Close";
        }
    }
}
