using System;
using System.IO;
using System.Windows.Forms;
using Common;
using GuiComponents.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GuiComponents
{
    public class UserDialogs : IUserDialogs
    {
        public bool IsThisPathCorrect(string path)
        {
            var dialogResult = MessageBox.Show(
                    "Detected osu in: " + Environment.NewLine + path + Environment.NewLine + "Is this correct?",
                    "osu! directory", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            return dialogResult == DialogResult.Yes;

        }

        public string SelectDirectory(string text)
        {
            return SelectDirectory(text, false);
        }

        public string SelectDirectory(string text, bool showNewFolder = false)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = text
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
                return dialog.FileName;

            return string.Empty;
        }

        public string SelectFile(string text, string filters = "", string filename = "")
        {
            var dialog = new CommonOpenFileDialog
            {
                Multiselect = false,
                Title = text
            };
            if (!string.IsNullOrEmpty(filters))
            {
                var split = filters.Split(new[] { '|' }, 2);
                dialog.Filters.Add(new CommonFileDialogFilter(split[0], split[1]));
            }

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                return dialog.FileName;

            return string.Empty;
        }

        public string SaveFile(string title, string types = "all|*.*")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = types,
                Title = title
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                return saveFileDialog.FileName;
            }
            return string.Empty;
        }

        public bool YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info)
        {
            var icon = GetMessageBoxIcon(messageBoxType);

            var dialogResult = MessageBox.Show(null, text, caption, MessageBoxButtons.YesNo, icon);

            return dialogResult == DialogResult.Yes;
        }

        public (bool Result, bool doNotAskAgain) YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info,
            string doNotAskAgainText = null)
        {
            var result = YesNoForm.ShowDialog(text, caption, doNotAskAgainText);

            return (result.dialogResult == DialogResult.Yes, result.doNotAskAgain);
        }

        public IProgressForm ProgressForm(Progress<string> userProgressMessage, Progress<int> completionPercentage)
        {
            return GuiComponents.ProgressForm.ShowDialog(userProgressMessage, completionPercentage);
        }

        public void OkMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info)
        {
            var icon = GetMessageBoxIcon(messageBoxType);

            MessageBox.Show(null, text, caption, MessageBoxButtons.OK, icon);
        }

        private MessageBoxIcon GetMessageBoxIcon(MessageBoxType messageBoxType)
        {
            MessageBoxIcon icon;
            switch (messageBoxType)
            {
                case MessageBoxType.Error:
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageBoxType.Info:
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageBoxType.Question:
                    icon = MessageBoxIcon.Question;
                    break;
                case MessageBoxType.Warning:
                    icon = MessageBoxIcon.Warning;
                    break;
                default:
                    icon = MessageBoxIcon.Information;
                    break;
            }
            return icon;
        }
    }
}