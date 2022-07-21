using Common;
using System;

namespace GuiComponents.Interfaces
{
    public interface IUserDialogs
    {
        bool IsThisPathCorrect(string path);
        string SelectDirectory(string text);
        string SelectDirectory(string text, bool showNewFolder = false);
        string SelectFile(string text, string types = "", string filename = "");
        string SaveFile(string title, string types = "all|*.*");
        bool YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info);
        (bool Result, bool doNotAskAgain) YesNoMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info,string doNotAskAgainText = null);
        IProgressForm ProgressForm(Progress<string> userProgressMessage, Progress<int> completionPercentage);
        void OkMessageBox(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info);
    }
}