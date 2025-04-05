namespace CollectionManager.Extensions.Modules.Downloader;

using CollectionManager.Extensions.Modules.Downloader.Api;
using System.ComponentModel;

public class GenericMapDownloader : DownloadManager
{
    public DownloadThrottler DownloadThrottler { get; private set; }

    public GenericMapDownloader(string saveLocation, int downloadThreads, int downloadsPerMinute, int downloadsPerHour) : base(saveLocation, downloadThreads)
    {
        DownloadThrottler = new DownloadThrottler(downloadsPerMinute, downloadsPerHour);
    }

    public override bool CanDownload(DownloadItem downloadItem)
    {
        if (DownloadThrottler.CanDownload())
        {
            downloadItem.DownloadSlotStatus = null;
            return true;
        }

        downloadItem.DownloadSlotStatus = DownloadThrottler.GetStatus();
        return false;
    }

    protected override void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error == null)
        {
            DownloadThrottler.RegisterDownload();
        }

        base.DownloadCompleted(sender, e);
    }
}
