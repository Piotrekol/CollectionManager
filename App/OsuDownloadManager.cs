using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using App.Misc;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Common;
using Gui.Misc;
using GuiComponents.Interfaces;
using App.Properties;
using System.IO;
using App.Models;
using Common.Interfaces;
using Newtonsoft.Json;

namespace App
{
    public sealed class OsuDownloadManager
    {
        public static OsuDownloadManager Instance = new OsuDownloadManager();
        //Collections for preparing downloads
        private Beatmaps BeatmapsToDownload { get; } = new Beatmaps();
        private HashSet<int> ListedMapSetIds { get; } = new HashSet<int>();
        /// <summary>
        /// Stores all requested downloads
        /// </summary>
        public ICollection<DownloadItem> DownloadItems { get; private set; } = new List<DownloadItem>();

        private DownloadManager _mapDownloader = null;
        private Lazy<List<DownloadSource>> _downloadSources = new Lazy<List<DownloadSource>>(() =>
        {
            const string configurationFileName = "downloadSources.json";
            if (!File.Exists(configurationFileName))
                throw new NotImplementedException("download sources configuration is missing!");

            return JsonConvert.DeserializeObject<List<DownloadSource>>(File.ReadAllText(configurationFileName));
        });

        private IReadOnlyList<IDownloadSource> DownloadSources => _downloadSources.Value;
        public IDownloadSource SelectedDownloadSource { get; private set; }
        public event EventHandler DownloadItemsChanged;
        public event EventHandler<EventArgs<DownloadItem>> DownloadItemUpdated;

        public bool IsLoggedIn { get; private set; } = false;
        public string DownloadDirectory { get; set; } = string.Empty;
        public bool DownloadDirectoryIsSet => !string.IsNullOrEmpty(DownloadDirectory);
        private long _downloadId = 0;
        public bool? DownloadWithVideo { get; set; }
        public bool AskUserForSaveDirectoryAndLogin(IUserDialogs userDialogs, ILoginFormView loginForm)
        {
            if (IsLoggedIn)
                return true;

            var downloaderSettings = JsonConvert.DeserializeObject<DownloaderSettings>(Settings.Default.DownloadManager_DownloaderSettings);
            if (downloaderSettings.LoginData == null)
                downloaderSettings.LoginData = new LoginData();

            var useExistingSettings = downloaderSettings.IsValid(DownloadSources) && userDialogs.YesNoMessageBox($"Reuse last downloader settings? {Environment.NewLine}{downloaderSettings}", "DownloadManager - Reuse settings", MessageBoxType.Question);
            if (useExistingSettings)
            {
                DownloadDirectory = downloaderSettings.DownloadDirectory;
                DownloadWithVideo = downloaderSettings.DownloadWithVideo;
                return LogIn(downloaderSettings.LoginData);
            }

            DownloadDirectory = userDialogs.SelectDirectory("Select directory for saved beatmaps", true);
            if (!DownloadDirectoryIsSet)
                return false;

            DownloadWithVideo = userDialogs.YesNoMessageBox("Download beatmaps with video?", "Beatmap downloader", MessageBoxType.Question);
            var userLoginData = loginForm.GetLoginData(DownloadSources);
            if (LogIn(userLoginData))
            {
                Settings.Default.DownloadManager_DownloaderSettings = JsonConvert.SerializeObject(new DownloaderSettings
                {
                    DownloadWithVideo = DownloadWithVideo,
                    DownloadDirectory = DownloadDirectory,
                    LoginData = userLoginData
                });
            }

            return IsLoggedIn;
        }

        public void DownloadBeatmap(Beatmap beatmap)
        {
            DownloadBeatmap(beatmap, true);
        }

        public void PauseDownloads()
        {
            if (_mapDownloader != null)
                _mapDownloader.StopDownloads = true;
        }
        public void ResumeDownloads()
        {
            if (_mapDownloader != null)
                _mapDownloader.StopDownloads = false;
        }

        private void MapDownloaderOnProgressUpdated(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            var item = (DownloadItem)downloadProgressChangedEventArgs.UserState;
            if (item.ProgressPrecentage % 10 == 0)
                DownloadItemUpdated?.Invoke(this, new EventArgs<DownloadItem>(item));
        }

        internal void DownloadBeatmaps(Beatmaps selectedBeatmaps)
        {
            foreach (var selectedBeatmap in selectedBeatmaps)
            {
                this.DownloadBeatmap(selectedBeatmap, false);
            }
            DownloadBeatmap(null, true);
        }

        public bool LogIn(LoginData loginData)
        {
            if (string.IsNullOrEmpty(loginData.DownloadSource))
                return false;

            SelectedDownloadSource = DownloadSources.First(s => s.Name == loginData.DownloadSource);
            var downloaderType = Type.GetType(SelectedDownloadSource.FullyQualifiedHandlerName);
            if (downloaderType == null)
                throw new NotImplementedException($"Download manager of type \"{SelectedDownloadSource.FullyQualifiedHandlerName}\" could not be found.");

            _mapDownloader = (DownloadManager)Activator.CreateInstance(downloaderType, DownloadDirectory, SelectedDownloadSource.DownloadThreads, SelectedDownloadSource.DownloadsPerMinute, SelectedDownloadSource.DownloadsPerHour);
            _mapDownloader.ProgressUpdated += MapDownloaderOnProgressUpdated;
            if (SelectedDownloadSource.RequiresLogin)
            {
                return IsLoggedIn = loginData.IsValid() && _mapDownloader.Login(loginData);
            }

            return IsLoggedIn = true;
        }

        private void DownloadBeatmap(Beatmap beatmap, bool fireUpdateEvent)
        {
            if (beatmap != null)
            {
                BeatmapsToDownload.Add((BeatmapExtension)beatmap);
                var downloadItem = GetDownloadItem((BeatmapExtension)beatmap);
                if (downloadItem == null)
                    return;
                DownloadItems.Add(downloadItem);
                ListedMapSetIds.Add(beatmap.MapSetId);
            }
            if (fireUpdateEvent)
                DownloadItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        private DownloadItem GetDownloadItem(Beatmap beatmap)
        {
            if (beatmap.MapSetId < 1 || ListedMapSetIds.Contains(beatmap.MapSetId))
                return null;
            long currentId = ++_downloadId;
            var oszFileName = CreateFileName(beatmap);
            var downloadUrl = string.Format(SelectedDownloadSource.BaseDownloadUrl, beatmap.MapSetId) + (DownloadWithVideo != null && DownloadWithVideo.Value ? string.Empty : "?noVideo=1");

            var downloadItem = _mapDownloader.DownloadFileAsync(downloadUrl, oszFileName, "https://osu.ppy.sh/", currentId);
            downloadItem.Id = currentId;
            return downloadItem;
        }

        private string CreateFileName(Beatmap map)
        {
            var filename = map.MapSetId + " " + map.ArtistRoman + " - " + map.TitleRoman;
            return Helpers.StripInvalidFileNameCharacters(filename) + ".osz";
        }
    }
}