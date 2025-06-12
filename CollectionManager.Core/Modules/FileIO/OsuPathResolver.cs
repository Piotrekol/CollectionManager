namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Types;
using Microsoft.Win32;
using System;
using System.IO;

public sealed class OsuPathResolver
{
    public static string GetOsuPath(Func<string, bool> thisPathIsCorrect, Func<string, string> selectDirectoryDialog)
    {
        string path = GetOsuOrLazerPath();

        if (string.IsNullOrWhiteSpace(path))
        {
            return GetManualOsuPath(selectDirectoryDialog);
        }

        if (thisPathIsCorrect is null)
        {
            return path;
        }

        bool result = thisPathIsCorrect(path);

        return result
            ? path
            : GetManualOsuPath(selectDirectoryDialog);
    }

    public static string GetOsuOrLazerPath()
    {
        if (TryGetRunningOsuPath(out string path))
        {
            return path;
        }

        if (TryGetLazerDataPath(out path))
        {
            return path;
        }

        if (TryGetOsuPathFromRegistry(out path))
        {
            return path;
        }

        return string.Empty;
    }

    public static string GetManualOsuPath(Func<string, string> selectDirectoryDialog)
    {
        string path = selectDirectoryDialog("Where is your osu! or lazer folder located at?");

        return IsOsuUserDataDirectory(path)
            ? path
            : string.Empty;
    }

    public static bool TryGetStablePath(out string path)
    {
        bool isRunning = TryGetRunningOsuPath(out path);
        if (isRunning && IsOsuStableDirectory(path))
        {
            return true;
        }

        if (TryGetOsuPathFromRegistry(out path, OsuType.Stable) && IsOsuStableDirectory(path))
        {
            return true;
        }

        return false;
    }

    public static bool TryGetLazerDataPath(out string path)
    {
        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "osu");

        if (Directory.Exists(path) && IsOsuLazerDataDirectory(path))
        {
            return true;
        }

        path = null;

        return false;
    }

    public static bool TryGetRunningOsuPath(out string path)
    {
        Process[] processes = null;
        try
        {
            processes = Process.GetProcessesByName("osu!");
        }
        catch
        {
            // Ignored.
        }

        if (processes is null || processes.Length is 0)
        {
            path = null;
            return false;
        }

        foreach (Process process in processes)
        {
            try
            {
                path = process.Modules[0].FileName;
                path = path.Remove(path.LastIndexOf('\\'));
                if (IsOsuUserDataDirectory(path))
                {
                    return true;
                }
            }
            catch
            {
                // Ignored.
            }
        }

        path = null;
        return false;
    }

    public static bool IsOsuUserDataDirectory(string directory) => IsOsuStableDirectory(directory) || IsOsuLazerDataDirectory(directory);

    public static bool IsOsuStableDirectory(string directory) => File.Exists(Path.Combine(directory, "osu!.db"));

    public static bool IsOsuLazerDataDirectory(string directory) => File.Exists(Path.Combine(directory, "client.realm"));

    /// <summary>
    /// Attempts to retrieve osu! stable or lazer path from windows registry.
    /// </summary>
    /// <returns></returns>
    private static bool TryGetOsuPathFromRegistry(out string path, OsuType osuType = OsuType.Any)
    {
        if (!OperatingSystem.IsWindows())
        {
            path = null;
            return false;
        }

        try
        {
            const string lazerKey = "osu.File.osz\\Shell\\Open\\Command";
            const string stableKey = "osustable.File.osz\\Shell\\Open\\Command";

            string[] keys = osuType switch
            {
                OsuType.Any => [lazerKey, stableKey],
                OsuType.Stable => [stableKey],
                OsuType.Lazer => [lazerKey],
                OsuType unknown => throw new InvalidOperationException($"OsuType {unknown} is not valid.")
            };

            foreach (string key in keys)
            {
                using RegistryKey osuRegistryKey = Registry.ClassesRoot.OpenSubKey(key);

                if (osuRegistryKey is null)
                {
                    continue;
                }

                string keyValue = osuRegistryKey.GetValue(null).ToString();
                // format: "C:\some\path\to\osu!\or\lazer\osu!.exe" "%1"
                string exePath = keyValue.Remove(0, 1).Replace("\" \"%1\"", string.Empty);
                path = Path.GetDirectoryName(exePath);
                if (IsOsuUserDataDirectory(path))
                {
                    return true;
                }
            }
        }
        catch (Exception)
        {
            // Ignored.
        }

        path = null;
        return false;
    }
}
