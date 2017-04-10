using System;
using System.Collections.Generic;
using App.Interfaces;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Gui.Misc;

namespace App.Models
{
    public class DownloadManagerModel : IDownloadManagerModel
    {
        private readonly OsuDownloadManager _osuDownloadManager;
        public event EventHandler DownloadItemsChanged;
        public event EventHandler<EventArgs<DownloadItem>> DownloadItemUpdated;
        public event EventHandler LogInStatusChanged;
        public event EventHandler LogInRequest;
        public event EventHandler StartDownloads;
        public event EventHandler StopDownloads;
        public void EmitStartDownloads()
        {
            StartDownloads?.Invoke(this, EventArgs.Empty);
            _osuDownloadManager?.ResumeDownloads();
        }

        public void EmitStopDownloads()
        {
            StopDownloads?.Invoke(this, EventArgs.Empty);
            _osuDownloadManager?.PauseDownloads();
        }

        public void EmitLoginRequest()
        {
            LogInRequest?.Invoke(this, EventArgs.Empty);
        }

        private ICollection<DownloadItem> _downloadItems;

        public ICollection<DownloadItem> DownloadItems
        {
            get
            {
                return _downloadItems;
            }
            set
            {
                _downloadItems = value;
                DownloadItemsChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        private bool _isLoggedIn;

        public DownloadManagerModel(OsuDownloadManager osuDownloadManager)
        {
            _osuDownloadManager = osuDownloadManager;
            _downloadItems = _osuDownloadManager.DownloadItems;
            _osuDownloadManager.DownloadItemsChanged += OsuDownloadManagerOnDownloadItemsChanged;
            _osuDownloadManager.DownloadItemUpdated += OsuDownloadManagerOnDownloadItemUpdated;
        }

        private void OsuDownloadManagerOnDownloadItemUpdated(object sender, EventArgs<DownloadItem> eventArgs)
        {
            DownloadItemUpdated?.Invoke(this, eventArgs);
        }

        private void OsuDownloadManagerOnDownloadItemsChanged(object sender, EventArgs eventArgs)
        {
            DownloadItems = _osuDownloadManager.DownloadItems;
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                LogInStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}