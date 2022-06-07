using System;
using System.Diagnostics;
using System.IO;
using CollectionManager.Exceptions;
using Microsoft.Win32;

namespace CollectionManager.Modules.FileIO
{
    public sealed class OsuPathResolver
    {
        public static OsuPathResolver Instance = new OsuPathResolver();
        private Process[] _processes;

        public bool OsuIsRunning
        {
            get
            {
                try
                {
                    _processes = Process.GetProcessesByName("osu!");
                    return _processes.Length > 0;
                }
                catch
                {
                    return false;
                }
            }
        }


        private void Log(string text, params string[] vals)
        {
            //
        }

        public string GetOsuDir(Func<string, bool> thisPathIsCorrect, Func<string, string> selectDirectoryDialog)
        {
            var dir = _getRunningOsuDir();
            if (dir != string.Empty)
            {
                if (thisPathIsCorrect == null)
                    return dir;

                var result = thisPathIsCorrect(dir);
                if (result)
                    return dir;
                else
                    return GetManualOsuDir(selectDirectoryDialog);
            }
            return GetManualOsuDir(selectDirectoryDialog);
        }

        public string GetOsuDir() => _getRunningOsuDir();

        private string _getRunningOsuDir()
        {
            if (OsuIsRunning)
            {
                try
                {
                    string dir = _processes[0].Modules[0].FileName;
                    dir = dir.Remove(dir.LastIndexOf('\\'));
                    return dir;
                }
                catch (Exception e) //Access denied
                {
                    Log("ERROR: could not get directory from running osu! | {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    using (RegistryKey osureg = Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon"))
                    {
                        if (osureg != null)
                        {
                            string osukey = osureg.GetValue(null).ToString();
                            var osupath = osukey.Remove(0, 1);
                            osupath = osupath.Remove(osupath.Length - 11);
                            return osupath;
                        }
                    }
                }
                catch (Exception e)
                {
                    Log("ERROR: could not get directory from registry key | {0}", e.Message);
                }

            }
            return string.Empty;
        }
        public string GetManualOsuDir(Func<string, string> selectDirectoryDialog)
        {
            var directory = selectDirectoryDialog("Where is your osu! folder located at?");
            if (!File.Exists(directory + @"\osu!.db"))
                directory = string.Empty;

            return directory;
        }

        public string SelectDirectory(string text, bool showNewFolder = false)
        {
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            ////set description and base folder for browsing

            //dialog.ShowNewFolderButton = true;
            //dialog.Description = text;
            //dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            //if (dialog.ShowDialog() == DialogResult.OK && Directory.Exists((dialog.SelectedPath)))
            //{
            //    return dialog.SelectedPath;
            //}
            return string.Empty;
        }
    }
}