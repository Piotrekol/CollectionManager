using System;
using System.Collections.Generic;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;

namespace GuiComponents.Interfaces
{
    public interface IDownloadManagerView
    {
        event EventHandler DownloadToggleClick;
        event EventHandler Disposed;

        bool DownloadButtonIsEnabled { set; }
        string DownloadButtonText { set; }
        void SetDownloadItems(ICollection<DownloadItem> downloadItems);
        void UpdateDownloadItem(DownloadItem downloadItem);
    }
}