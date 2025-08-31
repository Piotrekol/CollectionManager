namespace CollectionManager.App.Winforms;

using CollectionManager.App.Shared.Misc;
using System.Collections.Specialized;
using Clipboard = System.Windows.Clipboard;

internal class WinFormsClipboard : IClipboard
{
    public string GetText() => Clipboard.GetText();
    public void SetFileDropList(StringCollection paths) => Clipboard.SetFileDropList(paths);

    public void SetText(string text) => Clipboard.SetText(text);
}