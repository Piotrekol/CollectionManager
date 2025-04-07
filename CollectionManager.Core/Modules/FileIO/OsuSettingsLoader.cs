namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Extensions;
using System;
using System.IO;

public sealed class OsuSettingsLoader
{
    private const string _defaultOsuSongsConfigEntryValue = "Songs";
    private static readonly char[] _separator = ['='];

    public string CustomBeatmapDirectoryLocation { get; private set; } = _defaultOsuSongsConfigEntryValue;

    public OsuSettingsLoader()
    {

    }

    public void Load(string osuDirectory)
    {
        CustomBeatmapDirectoryLocation = _defaultOsuSongsConfigEntryValue;

        string configFilePath = GetConfigFilePath(osuDirectory);

        _ = TryGetSongsDirectory(configFilePath, out string songsDirectory);
        string[] potentialDirectories = [
            songsDirectory, // custom stable directory set in config
            Path.Combine(osuDirectory, _defaultOsuSongsConfigEntryValue), // stable directory
            Path.Combine(osuDirectory, "files") // lazer directory
            ];

        foreach (string directory in potentialDirectories)
        {
            if (Directory.Exists(directory))
            {
                CustomBeatmapDirectoryLocation = directory;

                return;
            }
        }
    }

    private static string GetConfigFilePath(string osuDirectory)
    {
        string sanitizedUsername = Environment.UserName.StripInvalidFileNameCharacters().Replace(".", string.Empty);
        string filename = $"osu!.{sanitizedUsername}.cfg";

        return Path.Combine(osuDirectory, filename);
    }

    private static bool TryGetSongsDirectory(string configPath, out string songsDirectory)
    {
        songsDirectory = null;

        if (!File.Exists(configPath))
        {
            return false;
        }

        foreach (string cfgLine in File.ReadLines(configPath))
        {
            if (cfgLine.StartsWith("BeatmapDirectory", StringComparison.InvariantCulture))
            {
                string[] splitLines = cfgLine.Split(_separator, 2);

                if (splitLines.Length < 2)
                {
                    continue;
                }

                songsDirectory = splitLines[1].Trim(' ');

                return true;
            }
        }

        return false;
    }
}
