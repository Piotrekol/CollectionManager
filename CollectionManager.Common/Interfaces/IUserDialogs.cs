namespace CollectionManager.Common.Interfaces;

using CollectionManager.Common.Interfaces.Forms;
using System;

public interface IUserDialogs
{
    Task<bool> IsThisPathCorrectAsync(string path);
    Task<string> SelectDirectoryAsync(string text);
    Task<string> SelectDirectoryAsync(string text, bool showNewFolder = false);
    Task<string> SelectFileAsync(string text, string types = "", string filename = "");
    Task<string> SaveFileAsync(string title, string types = "all|*.*");
    Task<bool> YesNoMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info);
    Task<(bool Result, bool doNotAskAgain)> YesNoMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info, string doNotAskAgainText = null);
    Task<IProgressForm> CreateProgressFormAsync(Progress<string> userProgressMessage, Progress<int> completionPercentage);
    Task OkMessageBoxAsync(string text, string caption, MessageBoxType messageBoxType = MessageBoxType.Info);
    Task TextMessageBoxAsync(string text, string caption);
}
