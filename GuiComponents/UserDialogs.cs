using System;
using System.IO;
using System.Windows.Forms;
using Common;
using GuiComponents.Interfaces;

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
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //set description and base folder for browsing

            dialog.ShowNewFolderButton = true;
            dialog.Description = text;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (dialog.ShowDialog() == DialogResult.OK && Directory.Exists((dialog.SelectedPath)))
            {
                return dialog.SelectedPath;
            }
            return string.Empty;
        }
        public string SelectFile(string text, string types = "", string filename = "")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = types; //"Collection database (*.db)|*.db";
            openFileDialog.FileName = filename; //"collection.db";
            openFileDialog.Multiselect = false;
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                return openFileDialog.FileName;
            else
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

            var dialogResult = MessageBox.Show(null, caption, text, MessageBoxButtons.YesNo,icon);

            return dialogResult == DialogResult.Yes;
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
                default:
                    icon = MessageBoxIcon.Information;
                    break;
            }
            return icon;
        }
    }
}