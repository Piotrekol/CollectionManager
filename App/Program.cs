using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Misc;

namespace App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += (_, exArgs) =>
            {
                if (exArgs.ExceptionObject is Exception exception)
                    HandleException(exception);
            };
            Application.ThreadException += (_, exArgs) => HandleException(exArgs.Exception);
            TaskScheduler.UnobservedTaskException += (_, exArgs) => HandleException(exArgs.Exception);

            var app = new Initalizer();
            app.Run(args);
            Application.Run(app);
        }

        public static void HandleException(Exception ex)
        {
            Helpers.SetClipboardText(ex.ToString());
            MessageBox.Show(
                $"There was a problem with CollectionManager and it needs to exit, exception text was copied to your clipboard:{Environment.NewLine}{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
