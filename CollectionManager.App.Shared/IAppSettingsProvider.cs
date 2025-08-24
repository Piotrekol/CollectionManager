namespace CollectionManager.App.Shared;

public interface IAppSettingsProvider
{
    void Save();
    void Load();
    void Reset();

    bool Audio_autoPlay { get; set; }
    bool Audio_playerMode { get; set; }
    float Audio_volume { get; set; }
    string BeatmapColumns { get; set; }
    string BeatmapListingPresenterSettings { get; set; }
    string CollectionColumns { get; set; }
    bool DontAskAboutReorderingCollections { get; set; }
    string DownloadManager_DownloaderSettings { get; set; }
    string Osustats_apiKey { get; set; }
    string ScoresColumns { get; set; }
    string StartupSettings { get; set; }
}