using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using App.Misc;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Modules.DownloadManager;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Common;
using Gui.Misc;
using GuiComponents.Interfaces;
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

        private OsuDownloader _osuDownloader = null;

        public event EventHandler DownloadItemsChanged;
        public event EventHandler<EventArgs<DownloadItem>> DownloadItemUpdated;

        public bool IsLoggedIn { get; private set; } = false;
        public string DownloadDirectory { get; set; } = "";
        public bool DownloadDirectoryIsSet => DownloadDirectory != "";
        private long _downloadId = 0;
        private const string BaseDownloadUrl = "https://osu.ppy.sh/beatmapsets/{0}/download";
        public bool? DownloadWithVideo { get; set; }

        private String CacheFile = "cache.json";
        public bool AskUserForSaveDirectoryAndLogin(IUserDialogs userDialogs, ILoginFormView loginForm)
        {
            Dictionary<String, String> dict = null;
            if (File.Exists(this.CacheFile))
            {
                using (StreamReader file = File.OpenText(this.CacheFile))
                    dict = (Dictionary<string,string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(file.ReadToEnd());
                SetDownloadDirectory(dict["DownloadDirectory"]);
                DownloadWithVideo = Convert.ToBoolean(dict["DownloadWithVideo"]);
                LogIn(new LoginData() { Password = "5252353252", Username = "634636346242", OsuCookies = dict["Cookies"] });
                if(IsLoggedIn)
                    return true;
            }
            dict = new Dictionary<string, string>();
            if (!DownloadDirectoryIsSet)
            {
                SetDownloadDirectory(userDialogs.SelectDirectory("Select directory for saved beatmaps", true));
                if (!DownloadDirectoryIsSet)
                    return false;
                dict.Add("DownloadDirectory", this.DownloadDirectory);
            }

            if (!DownloadWithVideo.HasValue)
            {
                DownloadWithVideo = userDialogs.YesNoMessageBox("Download beatmaps with video?", "Beatmap downloader",
                    MessageBoxType.Question);
                dict.Add("DownloadWithVideo", DownloadWithVideo.ToString());
            }
            if (!IsLoggedIn)
            {
		LoginData data = loginForm.GetLoginData();
                LogIn(data);
                if (!IsLoggedIn)
                    return false;
                dict.Add("Cookies", data.OsuCookies);
            }
            using(StreamWriter file = File.CreateText(this.CacheFile))
                file.Write(JsonConvert.SerializeObject(dict));

            return true;
        }
        public void DownloadBeatmap(Beatmap beatmap)
        {
            DownloadBeatmap(beatmap, true);
        }

        public void PauseDownloads()
        {
            if(_osuDownloader!=null)
                _osuDownloader.StopDownloads = true;
        }
        public void ResumeDownloads()
        {
            if (_osuDownloader != null)
                _osuDownloader.StopDownloads = false;
        }

        public void SetDownloadDirectory(string path)
        {
            if (DownloadDirectory != "")
                throw new NotImplementedException("Changing of download directory while it has been set before is not supported.");
            DownloadDirectory = path;
            _osuDownloader = new OsuDownloader(DownloadDirectory, 3);
            _osuDownloader.ProgressUpdated += OsuDownloaderOnProgressUpdated;
        }

        private void OsuDownloaderOnProgressUpdated(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
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

        public void LogIn(LoginData loginData)
        {
            if (loginData != null && loginData.isValid())
            {
                IsLoggedIn = _osuDownloader.Login(loginData);
            }
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
            var downloadUrl = string.Format(BaseDownloadUrl, beatmap.MapSetId) + (DownloadWithVideo != null && DownloadWithVideo.Value ? string.Empty : "?noVideo=1");

            var downloadItem = _osuDownloader.DownloadFileAsync(downloadUrl, oszFileName, "https://osu.ppy.sh/", currentId);
            downloadItem.Id = currentId;
            return downloadItem;
        }

        private string CreateFileName(Beatmap map)
        {
            var filename = map.MapSetId + " " + map.ArtistRoman + " - " + map.TitleRoman;
            return Helpers.StripInvalidCharacters(filename) + ".osz";
        }
    }
}