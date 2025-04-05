namespace CollectionManager.Extensions.Modules.Downloader.Api;

using System;

public class DownloadProgressReportEventArgs : EventArgs
{
    public DownloadProgressReportEventArgs(long id)
    {
        Id = id;
    }
    public string Url { get; set; }
    public string FileName { get; set; }
    public long Id { get; private set; }
}
