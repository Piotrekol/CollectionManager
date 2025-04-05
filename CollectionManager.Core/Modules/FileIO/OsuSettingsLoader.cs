namespace CollectionManager.Core.Modules.FileIo;

using System;
using System.IO;

public sealed class OsuSettingsLoader
{
    public string CustomBeatmapDirectoryLocation { get; private set; } = "Songs";

    public OsuSettingsLoader()
    {

    }
    public void Load(string osuDirectory)
    {
        string FilePath = GetConfigFilePath(osuDirectory);
        bool configFileExists = File.Exists(FilePath);
        if (configFileExists)
        {
            ReadSettings(FilePath);
        }

        string lazerFilesPath = Path.Combine(osuDirectory, "files");
        string osuSongsPath = Path.Combine(osuDirectory, "Songs");
        if (Path.IsPathRooted(osuDirectory) && Directory.Exists(lazerFilesPath))
        {
            // Assuming osu!lazer
            CustomBeatmapDirectoryLocation = lazerFilesPath;
        }
        else if (CustomBeatmapDirectoryLocation == "Songs" && Directory.Exists(osuSongsPath))
        {
            CustomBeatmapDirectoryLocation = osuSongsPath;
        }
    }
    private string GetConfigFilePath(string osuDirectory)
    {
        string filename = string.Format("osu!.{0}.cfg", StripInvalidCharacters(Environment.UserName));
        return Path.Combine(osuDirectory, filename);
    }

    private string StripInvalidCharacters(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar.ToString(), string.Empty);
        }

        return name.Replace(".", string.Empty);
    }
    private void ReadSettings(string configPath)
    {
        foreach (string cfgLine in File.ReadLines(configPath))
        {
            if (cfgLine.StartsWith("BeatmapDirectory"))
            {
                string[] splitedLines = cfgLine.Split(new[] { '=' }, 2);
                string songDirectory = splitedLines[1].Trim(' ');

                if (songDirectory != "Songs")
                {
                    CustomBeatmapDirectoryLocation = songDirectory;
                }
            }
        }
    }
}
