namespace CollectionManager.App.Winforms;

using CollectionManager.App.Shared.Misc;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(int dwProcessId);

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static int Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
#pragma warning disable WFO5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        Application.SetColorMode(SystemColorMode.Dark);
#pragma warning restore WFO5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        AppDomain.CurrentDomain.UnhandledException += (_, exArgs) =>
        {
            if (exArgs.ExceptionObject is Exception exception)
            {
                HandleException(exception);
            }
        };
        Application.ThreadException += (_, exArgs) => HandleException(exArgs.Exception);
        TaskScheduler.UnobservedTaskException += (_, exArgs) => HandleException(exArgs.Exception);

        SettingsProvider settingsProvider = new(Properties.Settings.Default);
        WinFormsClipboard clipboard = new();
        WinFormsGuiComponentsProvider guiComponentsProvider = new();
        WinFormsInitalizer app = new(settingsProvider, clipboard, guiComponentsProvider);
        _ = app.RunGui(args);
        Application.Run(app.ApplicationContext);

        return 0;
    }

    public static void HandleException(Exception ex)
    {
        Helpers.SetClipboardText(ex.ToString());
        _ = MessageBox.Show(
            $"There was a problem with CollectionManager. It is recommended to save your edits and restart. {Environment.NewLine}{Environment.NewLine}Exception text was copied to your clipboard:{Environment.NewLine}{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}