namespace CollectionManager.App.Shared.Interfaces.Controls;

using CollectionManager.Common.Interfaces;
using CollectionManager.Extensions.Modules.Downloader.Api;

public interface IDownloadManagerModel
{
    event EventHandler DownloadItemsChanged;
    event EventHandler<DownloadItem> DownloadItemUpdated;

    event EventHandler LogInStatusChanged;
    event EventHandler LogInRequest;
    event EventHandler StartDownloads;
    event EventHandler StopDownloads;
    void EmitStartDownloads();
    void EmitStopDownloads();
    void EmitLoginRequest();
    ICollection<IDownloadItem> DownloadItems { get; set; }
    bool IsLoggedIn { get; set; }
}