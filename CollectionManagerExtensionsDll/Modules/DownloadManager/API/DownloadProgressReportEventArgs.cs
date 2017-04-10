using System;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public class DownloadProgressReportEventArgs :EventArgs
    {
        public DownloadProgressReportEventArgs(long id)
        {
            this.Id = id;
        }
        public string Url { get; set; }
        public string FileName { get; set; }
        public long Id { get; private set; }
    }
}
