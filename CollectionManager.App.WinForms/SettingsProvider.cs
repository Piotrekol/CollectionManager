namespace CollectionManager.App.Winforms;

using CollectionManager.App.Shared;
using CollectionManager.App.Winforms.Properties;
using System;

internal class SettingsProvider : IAppSettingsProvider
{
    private readonly Settings settings;

    internal SettingsProvider(Settings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public bool Audio_autoPlay
    {
        get => settings.Audio_autoPlay;
        set => settings.Audio_autoPlay = value;
    }

    public bool Audio_playerMode
    {
        get => settings.Audio_playerMode;
        set => settings.Audio_playerMode = value;
    }

    public float Audio_volume
    {
        get => settings.Audio_volume;
        set => settings.Audio_volume = value;
    }

    public string BeatmapColumns
    {
        get => settings.BeatmapColumns;
        set => settings.BeatmapColumns = value;
    }

    public string BeatmapListingPresenterSettings
    {
        get => settings.BeatmapListingPresenterSettings;
        set => settings.BeatmapListingPresenterSettings = value;
    }

    public string CollectionColumns
    {
        get => settings.CollectionColumns;
        set => settings.CollectionColumns = value;
    }

    public bool DontAskAboutReorderingCollections
    {
        get => settings.DontAskAboutReorderingCollections;
        set => settings.DontAskAboutReorderingCollections = value;
    }

    public string DownloadManager_DownloaderSettings
    {
        get => settings.DownloadManager_DownloaderSettings;
        set => settings.DownloadManager_DownloaderSettings = value;
    }

    public string Osustats_apiKey
    {
        get => settings.Osustats_apiKey;
        set => settings.Osustats_apiKey = value;
    }

    public string ScoresColumns
    {
        get => settings.ScoresColumns;
        set => settings.ScoresColumns = value;
    }

    public string StartupSettings
    {
        get => settings.StartupSettings;
        set => settings.StartupSettings = value;
    }

    public void Load() => settings.Reload();

    public void Reset() => settings.Reset();

    public void Save() => settings.Save();
}
