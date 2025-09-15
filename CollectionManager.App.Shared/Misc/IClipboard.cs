namespace CollectionManager.App.Shared.Misc;

using System.Collections.Specialized;

public interface IClipboard
{
    void SetFileDropList(StringCollection paths);
    void SetText(string text);
    string GetText();
}