namespace CollectionManagerApp;
using CollectionManagerApp.Misc;
using System.IO;
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

        AppDomain.CurrentDomain.UnhandledException += (_, exArgs) =>
        {
            if (exArgs.ExceptionObject is Exception exception)
            {
                HandleException(exception);
            }
        };
        Application.ThreadException += (_, exArgs) => HandleException(exArgs.Exception);
        TaskScheduler.UnobservedTaskException += (_, exArgs) => HandleException(exArgs.Exception);

        if (args.Length > 0 && !File.Exists(args[0]))
        {
            //This somewhat breaks console interaction/output can't be easily piped
            _ = AttachConsole(-1);

            return CommandLineRunner.Process(args) ? 0 : -1;
        }

        Initalizer app = new();
        _ = app.Run(args);
        Application.Run(app);

        return 0;
    }

    public static void HandleException(Exception ex)
    {
        Helpers.SetClipboardText(ex.ToString());
        _ = MessageBox.Show(
            $"There was a problem with CollectionManager. It is recommended to save your edits and restart. {Environment.NewLine}{Environment.NewLine}Exception text was copied to your clipboard:{Environment.NewLine}{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
