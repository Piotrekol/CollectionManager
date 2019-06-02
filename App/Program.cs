using System;
using System.Windows.Forms;

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
            var app = new Initalizer();
            app.Run(args);
            Application.Run(app);
        }
    }
}
