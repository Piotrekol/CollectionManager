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
    public bool IsThisPathCorrect(string path)
    {
        DialogResult dialogResult = MessageBox.Show(
                "Detected osu in: " + Environment.NewLine + path + Environment.NewLine + "Is this correct?",
                "osu! directory", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
        return dialogResult == DialogResult.Yes;

    }

    public string SelectDirectory(string text) => SelectDirectory(text, false);

    public string SelectDirectory(string text, bool showNewFolder = false)
    {
        CommonOpenFileDialog dialog = new()
        {
            IsFolderPicker = true,
            Title = text
        };
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
        {
            return dialog.FileName;
        }

        return string.Empty;
    }
    private readonly char[] separator = new[] { '|' };

    public string SelectFile(string text, string types = "", string filename = "")
    {
        CommonOpenFileDialog dialog = new()
        {
            Multiselect = false,
            Title = text
        };
        if (!string.IsNullOrEmpty(types))
        {
            string[] split = types.Split(separator, 2);
            dialog.Filters.Add(new CommonFileDialogFilter(split[0], split[1]));
        }

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            return dialog.FileName;
        }

        return string.Empty;
    }

    public string SaveFile(string title, string types = "all|*.*")
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = types,
            Title = title
        };
        _ = saveFileDialog.ShowDialog();
        if (saveFileDialog.FileName != "")
        {
            return saveFileDialog.FileName;
        }

        return string.Empty;
    }

    public bool YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info)
    {
        MessageBoxIcon icon = GetMessageBoxIcon(messageBoxType);

        DialogResult dialogResult = MessageBox.Show(null, text, caption, MessageBoxButtons.YesNo, icon);

        return dialogResult == DialogResult.Yes;
    }

    public (bool Result, bool doNotAskAgain) YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info,
        string doNotAskAgainText = null)
    {
        (DialogResult dialogResult, bool doNotAskAgain) = YesNoForm.ShowDialog(text, caption, doNotAskAgainText);

        return (dialogResult == DialogResult.Yes, doNotAskAgain);
    }

    public IProgressForm ProgressForm(Progress<string> userProgressMessage, Progress<int> completionPercentage) => GuiComponents.ProgressForm.ShowDialog(userProgressMessage, completionPercentage);

    public void OkMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info)
    {
        MessageBoxIcon icon = GetMessageBoxIcon(messageBoxType);

        _ = MessageBox.Show(null, text, caption, MessageBoxButtons.OK, icon);
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