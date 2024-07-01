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
            var dir = GetOsuOrLazerDir();
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

        public string GetOsuDir()
            => GetOsuOrLazerDir();

        private string GetOsuOrLazerDir()
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

                    return string.Empty;
                }
            }

            if (TryGetLazerDataPath(out var lazerDataPath))
            {
                return lazerDataPath;
            }

            try
            {

                if (TryGetOsuDirFromRegistry(out var path))
                {
                    return path;
                }
            }
            catch (Exception e)
            {
                Log("ERROR: could not get directory from registry key | {0}", e.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// Attempts to retrieve osu! legacy path from windows registry.
        /// </summary>
        /// <remarks>
        /// This works only with legacy osu version.<br/>
        /// Fails gracefully with lazer.<br/>
        /// </remarks>
        /// <returns></returns>
        private static bool TryGetOsuDirFromRegistry(out string path)
        {
            using RegistryKey osuRegistryKey = Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon");

            if (osuRegistryKey != null)
            {
                var osuIconPath = osuRegistryKey.GetValue(null).ToString();
                var osuPath = osuIconPath.Remove(0, 1);
                path = osuPath.Remove(osuPath.Length - 11);

                // No point in trying to make this correct for lazer,
                // since with lazer installed this registry key points to osu app instance,
                // that contains no user data.
                if (Directory.Exists(path))
                {
                    return true;
                }
            }

            path = null;

            return false;
        }

        public string GetManualOsuDir(Func<string, string> selectDirectoryDialog)
        {
            var directory = selectDirectoryDialog("Where is your osu! or lazer folder located at?");

            if (File.Exists(Path.Combine(directory, "osu!.db")) || File.Exists(Path.Combine(directory, "client.realm")))
            {
                return directory;
            }

            return string.Empty;
        }

        private static bool TryGetLazerDataPath(out string path)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "osu");

            if (Directory.Exists(path))
            {
                return true;
            }

            path = null;

            return false;
        }
    }
}