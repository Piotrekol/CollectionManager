namespace GuiComponents;

using CollectionManager.WinForms.Forms;
using System;
using System.Windows.Forms;

public sealed partial class OkForm : BaseForm
{
    private const string OkButtonText = "OK";
    private const string DefaultDoNotInformAgainText = "Don't inform me again in this session";

    public bool DoNotAskAgainInThisSession { get; private set; }

    private DateTime? _autoCloseAtUtc;
    private Timer? _autoCloseTimer;

    public OkForm()
    {
        InitializeComponent();
    }

    internal static (DialogResult dialogResult, bool doNotAskAgainInThisSession) ShowDialog(string text, string caption, TimeSpan? autoCloseAfter = null, string? doNotInformAgainText = null)
    {
        using OkForm f = new();
        f.PrepareForm(text, caption, autoCloseAfter, doNotInformAgainText);

        return (f.ShowDialog(), f.DoNotAskAgainInThisSession);
    }

    private void PrepareForm(string text, string caption, TimeSpan? autoCloseAfter, string? doNotInformAgainText)
    {
        label_text.Text = text;
        Text = $"Collection Manager - {caption}";
        button_Ok.Text = OkButtonText;

        checkBox_doNotAskAgain.Text = string.IsNullOrWhiteSpace(doNotInformAgainText)
            ? DefaultDoNotInformAgainText
            : doNotInformAgainText;

        checkBox_doNotAskAgain.Checked = false;
        DoNotAskAgainInThisSession = false;
        if (autoCloseAfter is null)
        {
            return;
        }

        if (autoCloseAfter.Value < TimeSpan.Zero)
        {
            autoCloseAfter = TimeSpan.Zero;
        }

        _autoCloseAtUtc = DateTime.UtcNow + autoCloseAfter.Value;
        _autoCloseTimer = new Timer { Interval = 250 };
        _autoCloseTimer.Tick += (_, _) => UpdateAutoCloseCountdownAndMaybeClose();

        FormClosed += (_, _) =>
        {
            _autoCloseTimer?.Stop();
            _autoCloseTimer?.Dispose();
            _autoCloseTimer = null;
        };

        Shown += (_, _) =>
        {
            UpdateAutoCloseCountdownAndMaybeClose();
            _autoCloseTimer?.Start();
        };
    }

    private void UpdateAutoCloseCountdownAndMaybeClose()
    {
        if (_autoCloseAtUtc is null)
        {
            return;
        }

        int secondsLeft = (int)Math.Ceiling((_autoCloseAtUtc.Value - DateTime.UtcNow).TotalSeconds);
        if (secondsLeft <= 0)
        {
            DialogResult = DialogResult.OK;
            Close();
            return;
        }

        button_Ok.Text = $"{OkButtonText} ({secondsLeft}s)";
    }

    private void CheckBox_doNotAskAgain_CheckedChanged(object sender, EventArgs e)
    {
        DoNotAskAgainInThisSession = checkBox_doNotAskAgain.Checked;

        if (_autoCloseTimer is null)
        {
            return;
        }

        _autoCloseTimer.Stop();
        _autoCloseTimer.Dispose();
        _autoCloseTimer = null;
        _autoCloseAtUtc = null;

        button_Ok.Text = OkButtonText;
    }
}
