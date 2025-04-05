namespace CollectionManager.Common.Interfaces.Controls;

using System;
using System.Collections.Generic;

public interface IDownloadManagerView
{
    event EventHandler DownloadToggleClick;
    event EventHandler Disposed;

    bool DownloadButtonIsEnabled { set; }
    string DownloadButtonText { set; }
    void SetDownloadItems(ICollection<IDownloadItem> downloadItems);
    void UpdateDownloadItem(IDownloadItem downloadItem);
}