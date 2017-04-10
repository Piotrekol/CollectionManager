using System;
using System.Collections.Generic;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Gui.Misc;

namespace App.Interfaces
{
    public interface IDownloadManagerModel
    {
        event EventHandler DownloadItemsChanged;
        event EventHandler<EventArgs<DownloadItem>> DownloadItemUpdated;

        event EventHandler LogInStatusChanged;
        event EventHandler LogInRequest;
        event EventHandler StartDownloads;
        event EventHandler StopDownloads;
        void EmitStartDownloads();
        void EmitStopDownloads();
        void EmitLoginRequest();
        ICollection<DownloadItem> DownloadItems { get; set; }
        bool IsLoggedIn { get; set; }
    }
}