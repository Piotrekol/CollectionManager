namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Types;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class OsuPathResolver
{
    public static async Task<string> GetOsuPathAsync(Func<string, Task<bool>> thisPathIsCorrect, Func<string, Task<string>> selectDirectoryDialog)
    {
        OsuPathResult result = GetOsuOrLazerPath();

        if (string.IsNullOrWhiteSpace(result.Path))
        {
            return await GetManualOsuPathAsync(selectDirectoryDialog);
        }

        if (thisPathIsCorrect is null)
        {
            return result.Path;
        }

        bool isCorrect = await thisPathIsCorrect(result.Path);

        return isCorrect
            ? result.Path
            : await GetManualOsuPathAsync(selectDirectoryDialog);
    }

    public static OsuPathResult GetOsuOrLazerPath()
    {
        string stablePath = null;
        string lazerPath = null;

        if (TryGetRunningOsuPath(out string runningPath))
        {
            stablePath = runningPath;
        }

        if (TryGetLazerDataPath(out string dataPath))
        {
            lazerPath = dataPath;
        }

        OsuPathEntry[] registryPaths = GetOsuPathsFromRegistry();
        foreach (OsuPathEntry entry in registryPaths)
        {
            if (entry.Type == OsuType.Stable && stablePath == null)
            {
                stablePath = entry.Path;
            }
            else if (entry.Type == OsuType.Lazer && lazerPath == null)
            {
                lazerPath = entry.Path;
            }
        }

        // prioritize stable in auto detection.
        if (stablePath != null)
        {
            return new OsuPathResult(stablePath, OsuType.Stable, StablePath: stablePath, LazerPath: lazerPath);
        }

        if (lazerPath != null)
        {
            return new OsuPathResult(lazerPath, OsuType.Lazer, StablePath: stablePath, LazerPath: lazerPath);
        }

        return new OsuPathResult(string.Empty, OsuType.None, StablePath: null, LazerPath: null);
    }

    public static async Task<string> GetManualOsuPathAsync(Func<string, Task<string>> selectDirectoryDialog)
    {
        string path = await selectDirectoryDialog("Where is your osu! or lazer folder located at?");

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

        OsuPathEntry[] registryPaths = GetOsuPathsFromRegistry();
        foreach (OsuPathEntry entry in registryPaths)
        {
            if (entry.Type == OsuType.Stable && IsOsuStableDirectory(entry.Path))
            {
                path = entry.Path;
                return true;
            }
        }

        path = null;
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
    /// Attempts to retrieve osu! stable and lazer paths from windows registry.
    /// </summary>
    /// <returns></returns>
    private static OsuPathEntry[] GetOsuPathsFromRegistry()
    {
        if (!OperatingSystem.IsWindows())
        {
            return [];
        }

        List<OsuPathEntry> results = [];

        try
        {
            const string lazerKey = "osu.File.osz\\Shell\\Open\\Command";
            const string stableKey = "osustable.File.osz\\Shell\\Open\\Command";

            OsuPathEntry[] keys = [
                new(lazerKey, OsuType.Lazer),
                new(stableKey, OsuType.Stable)
            ];

            foreach (OsuPathEntry entry in keys)
            {
                using RegistryKey osuRegistryKey = Registry.ClassesRoot.OpenSubKey(entry.Path);

                if (osuRegistryKey is null)
                {
                    continue;
                }

                string keyValue = osuRegistryKey.GetValue(null).ToString();
                // format: "C:\some\path\to\osu!\or\lazer\osu!.exe" "%1"
                string exePath = keyValue.Remove(0, 1).Replace("\" \"%1\"", string.Empty);
                string path = Path.GetDirectoryName(exePath);
                if (IsOsuUserDataDirectory(path))
                {
                    results.Add(new OsuPathEntry(path, entry.Type));
                }
            }
        }
        catch (Exception)
        {
            // Ignored.
        }

        return [.. results];
    }

    private record OsuPathEntry(string Path, OsuType Type);
}
