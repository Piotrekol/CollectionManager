namespace CollectionManagerApp;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Misc;
using System.Windows.Forms;

public sealed class WinFormsInitalizer : Initalizer, IDisposable
{
    public ApplicationContext ApplicationContext { get; } = new();

    public WinFormsInitalizer(IAppSettingsProvider settings, IClipboard clipboard, IGuiComponentsProvider guiComponentsProvider)
        : base(settings, clipboard, guiComponentsProvider)
    {
    }

    public void Dispose() => ApplicationContext.Dispose();
    protected override void QuitApplication()
    {
        Properties.Settings.Default.Save();
        Application.Exit();
    }
}