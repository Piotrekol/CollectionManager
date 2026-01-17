namespace CollectionManager.WinForms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using Common;
using GuiComponents;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows.Forms;

public class UserDialogs : IUserDialogs
{
    private readonly char[] _fileFilterSeparator = ['|'];
    private static readonly HashSet<string> _sessionSuppressedDialogs = [];

    public Task<bool> IsThisPathCorrectAsync(string path)
    {
        DialogResult dialogResult = MessageBox.Show(
                "Detected osu in: " + Environment.NewLine + path + Environment.NewLine + "Is this correct?",
                "osu! directory", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

        return Task.FromResult(dialogResult == DialogResult.Yes);
    }

    public Task<string> SelectDirectoryAsync(string text) => SelectDirectoryAsync(text, false);

    public Task<string> SelectDirectoryAsync(string text, bool showNewFolder = false)
    {
        CommonOpenFileDialog dialog = new()
        {
            IsFolderPicker = true,
            Title = text
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
        {
            return Task.FromResult(dialog.FileName);
        }

        return Task.FromResult(string.Empty);
    }

    public Task<string> SelectFileAsync(string text, string types = "", string filename = "")
    {
        CommonOpenFileDialog dialog = new()
        {
            Multiselect = false,
            Title = text
        };
        if (!string.IsNullOrEmpty(types))
        {
            string[] split = types.Split(_fileFilterSeparator, 2);
            dialog.Filters.Add(new CommonFileDialogFilter(split[0], split[1]));
        }

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            return Task.FromResult(dialog.FileName);
        }

        return Task.FromResult(string.Empty);
    }

    public Task<string> SaveFileAsync(string title, string types = "all|*.*")
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = types,
            Title = title
        };
        _ = saveFileDialog.ShowDialog();
        if (saveFileDialog.FileName != "")
        {
            return Task.FromResult(saveFileDialog.FileName);
        }

        return Task.FromResult(string.Empty);
    }

    public Task<bool> YesNoMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info)
    {
        MessageBoxIcon icon = GetMessageBoxIcon(messageBoxType);

        DialogResult dialogResult = MessageBox.Show(null, text, caption, MessageBoxButtons.YesNo, icon);

        return Task.FromResult(dialogResult == DialogResult.Yes);
    }

    public Task<(bool Result, bool doNotAskAgain)> YesNoMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info,
        string doNotAskAgainText = null)
    {
        (DialogResult dialogResult, bool doNotAskAgain) = YesNoForm.ShowDialog(text, caption, doNotAskAgainText);

        return Task.FromResult((dialogResult == DialogResult.Yes, doNotAskAgain));
    }

    public Task<IProgressForm> CreateProgressFormAsync(Progress<string> userProgressMessage, Progress<int> completionPercentage)
        => Task.FromResult(GuiComponents.ProgressForm.ShowDialog(userProgressMessage, completionPercentage));

    public Task<bool> OkMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info, TimeSpan? autoCloseAfter = null, string? doNotInformAgainText = null)
    {
        MessageBoxIcon icon = GetMessageBoxIcon(messageBoxType);

        string suppressionId = $"OK|{messageBoxType}|{caption}|{text}";
        if (_sessionSuppressedDialogs.Contains(suppressionId))
        {
            return Task.FromResult(false);
        }

        if (autoCloseAfter is null)
        {
            _ = MessageBox.Show(null, text, caption, MessageBoxButtons.OK, icon);

            return Task.FromResult(true);
        }

        (DialogResult _, bool suppressInSession) = OkForm.ShowDialog(text, caption, autoCloseAfter.Value, doNotInformAgainText);
        if (suppressInSession)
        {
            _ = _sessionSuppressedDialogs.Add(suppressionId);
        }

        return Task.FromResult(true);
    }

    public Task TextMessageBoxAsync(string text, string caption)
    {
        TextBoxForm.ShowDialog(text, caption);

        return Task.CompletedTask;
    }

    private static MessageBoxIcon GetMessageBoxIcon(MessageBoxType messageBoxType)
        => messageBoxType switch
        {
            MessageBoxType.Error => MessageBoxIcon.Error,
            MessageBoxType.Info => MessageBoxIcon.Information,
            MessageBoxType.Question => MessageBoxIcon.Question,
            MessageBoxType.Warning => MessageBoxIcon.Warning,
            _ => MessageBoxIcon.Information,
        };
}