namespace CollectionManager.App.Shared.Misc;

using CollectionManager.App.Shared.Models;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Downloader.Api;
using CollectionManager.Extensions.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

public sealed class OsuDownloadManager
{
    public static OsuDownloadManager Instance = new();
    //Collections for preparing downloads
    private Beatmaps BeatmapsToDownload { get; } = [];
    private HashSet<int> ListedMapSetIds { get; } = [];
    /// <summary>
    /// Stores all requested downloads
    /// </summary>
    public ICollection<IDownloadItem> DownloadItems { get; private set; } = [];

    private DownloadManager _mapDownloader;
    private readonly Lazy<List<DownloadSource>> _downloadSources = new(() =>
    {
        string configLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "downloadSources.json");
        return !File.Exists(configLocation)
            ? throw new FileNotFoundException("download sources configuration is missing!")
            : JsonConvert.DeserializeObject<List<DownloadSource>>(File.ReadAllText(configLocation));
    });

    private IReadOnlyList<IDownloadSource> DownloadSources => _downloadSources.Value;
    public IDownloadSource SelectedDownloadSource { get; private set; }
    public event EventHandler DownloadItemsChanged;
    public event EventHandler<DownloadItem> DownloadItemUpdated;

    public bool IsLoggedIn { get; private set; }
    public string DownloadDirectory { get; set; } = string.Empty;
    public bool DownloadDirectoryIsSet => !string.IsNullOrEmpty(DownloadDirectory);
    private long _downloadId;
    public bool? DownloadWithVideo { get; set; }

    public async Task<bool> AskUserForSaveDirectoryAndLoginAsync(IUserDialogs userDialogs, ILoginFormView loginForm)
    {
        const string loginFailedMessage = "Login failed. Ensure that your login/password or cookies are correct";

        if (IsLoggedIn)
        {
            return true;
        }

        DownloaderSettings downloaderSettings = JsonConvert.DeserializeObject<DownloaderSettings>(Initalizer.Settings.DownloadManager_DownloaderSettings);
        downloaderSettings.LoginData ??= new LoginData();

        bool useExistingSettings = downloaderSettings.IsValid(DownloadSources)
            && await userDialogs.YesNoMessageBoxAsync($"Reuse last downloader settings? {Environment.NewLine}{downloaderSettings}", "DownloadManager - Reuse settings", MessageBoxType.Question);

        if (useExistingSettings)
        {
            DownloadDirectory = downloaderSettings.DownloadDirectory;
            DownloadWithVideo = downloaderSettings.DownloadWithVideo;

            if (TryLogIn(downloaderSettings.LoginData))
            {
                return true;
            }

            await userDialogs.OkMessageBoxAsync(loginFailedMessage, "Error", MessageBoxType.Error);

            return false;
        }

        DownloadDirectory = await userDialogs.SelectDirectoryAsync("Select directory for saved beatmaps", true);
        if (!DownloadDirectoryIsSet)
        {
            return false;
        }

        DownloadWithVideo = await userDialogs.YesNoMessageBoxAsync("Download beatmaps with video?", "Beatmap downloader", MessageBoxType.Question);
        LoginData userLoginData = loginForm.GetLoginData(DownloadSources);
        if (TryLogIn(userLoginData))
        {
            Initalizer.Settings.DownloadManager_DownloaderSettings = JsonConvert.SerializeObject(new DownloaderSettings
            {
                DownloadWithVideo = DownloadWithVideo,
                DownloadDirectory = DownloadDirectory,
                LoginData = userLoginData
            });
        }
        else
        {
            await userDialogs.OkMessageBoxAsync(loginFailedMessage, "Error", MessageBoxType.Error);
        }

        return IsLoggedIn;
    }

    public void DownloadBeatmap(Beatmap beatmap) => DownloadBeatmap(beatmap, true);

    public void PauseDownloads()
    {
        if (_mapDownloader != null)
        {
            _mapDownloader.StopDownloads = true;
        }
    }
    public void ResumeDownloads()
    {
        if (_mapDownloader != null)
        {
            _mapDownloader.StopDownloads = false;
        }
    }

    private void MapDownloaderOnProgressUpdated(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
    {
        DownloadItem item = (DownloadItem)downloadProgressChangedEventArgs.UserState;
        if (item.ProgressPrecentage % 10 == 0)
        {
            DownloadItemUpdated?.Invoke(this, item);
        }
    }

    internal void DownloadBeatmaps(Beatmaps selectedBeatmaps)
    {
        foreach (Beatmap selectedBeatmap in selectedBeatmaps)
        {
            DownloadBeatmap(selectedBeatmap, false);
        }

        DownloadBeatmap(null, true);
    }

    private bool TryLogIn(LoginData loginData)
    {
        if (string.IsNullOrEmpty(loginData.DownloadSource))
        {
            return false;
        }

        SelectedDownloadSource = DownloadSources.First(s => s.Name == loginData.DownloadSource);
        Type downloaderType = Type.GetType(SelectedDownloadSource.FullyQualifiedHandlerName);
        if (downloaderType == null)
        {
            throw new NotImplementedException($"Download manager of type \"{SelectedDownloadSource.FullyQualifiedHandlerName}\" could not be found.");
        }

        _mapDownloader = (DownloadManager)Activator.CreateInstance(downloaderType, DownloadDirectory, SelectedDownloadSource.DownloadThreads, SelectedDownloadSource.DownloadsPerMinute, SelectedDownloadSource.DownloadsPerHour);
        _mapDownloader.ProgressUpdated += MapDownloaderOnProgressUpdated;
        return SelectedDownloadSource.RequiresLogin ? (IsLoggedIn = loginData.IsValid() && _mapDownloader.Login(loginData)) : (IsLoggedIn = true);
    }

    private void DownloadBeatmap(Beatmap beatmap, bool fireUpdateEvent)
    {
        if (beatmap != null)
        {
            BeatmapsToDownload.Add((BeatmapExtension)beatmap);
            DownloadItem downloadItem = GetDownloadItem((BeatmapExtension)beatmap);
            if (downloadItem == null)
            {
                return;
            }

            DownloadItems.Add(downloadItem);
            _ = ListedMapSetIds.Add(beatmap.MapSetId);
        }

        if (fireUpdateEvent)
        {
            DownloadItemsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private DownloadItem GetDownloadItem(Beatmap beatmap)
    {
        if (beatmap.MapSetId < 1 || ListedMapSetIds.Contains(beatmap.MapSetId))
        {
            return null;
        }

        long currentId = ++_downloadId;
        string oszFileName = beatmap.OszFileName();
        string downloadUrl = string.Format(SelectedDownloadSource.BaseDownloadUrl, beatmap.MapSetId) + (DownloadWithVideo != null && DownloadWithVideo.Value ? string.Empty : "?noVideo=1");

        DownloadItem downloadItem = _mapDownloader.DownloadFile(downloadUrl, oszFileName, string.Format(SelectedDownloadSource.Referer, beatmap.MapSetId), currentId, SelectedDownloadSource.RequestTimeout);
        downloadItem.Id = currentId;
        return downloadItem;
    }
}