using System;
using System.Net;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public class DownloadItem
    {
        public EventHandler DownloadUpdated;
        private void OnDownloadUpdated() { DownloadUpdated?.Invoke(this, EventArgs.Empty); }
        public long Id { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Name { get { return FileName; } }
        public int RequestTimeout { get; set; }
        public string Progress
        {
            get
            {
                if (OtherError)
                    return Error;
                if (DownloadAborted)
                    return "Download cancelled";
                if (!string.IsNullOrEmpty(DownloadSlotStatus))
                    return DownloadSlotStatus;
                if (FileAlreadyExists)
                    return "File already exists";
                if (BytesRecived > 0)
                    return string.Format("{0}/{1}MB {2}%", ((BytesRecived / 1024f) / 1024f).ToString("F"),
                    ((TotalBytes / 1024f) / 1024f).ToString("F"), ProgressPrecentage);
                if (WebClient != null)
                    return "Starting download...";

                return "---";
            }
        }
        public long BytesRecived { get; set; }
        public long TotalBytes { get; set; }
        private int _progressPrecentage;
        public int ProgressPrecentage
        {
            get { return _progressPrecentage; }
            set
            {
                _progressPrecentage = value;
                OnDownloadUpdated();
            }
        }

        public bool DownloadAborted { get; set; }
        public bool FileAlreadyExists { get; set; }
        public string DownloadSlotStatus { get; set; }
        public bool OtherError { get; set; }
        public string Error { get; set; }
        public CookieAwareWebClient WebClient { get; set; }
        public int lastShownDlState { get; set; } = -1;
        public object UserToken { get; set; }
        public string Referer { get; set; }

        public void ResetErrorState()
        {
            this.Error = "";
            this.OtherError = false;
            this.DownloadAborted = false;
            this.FileAlreadyExists = false;
        }
        public override string ToString()
        {
            return "DLitem: " + this.Url + " ; " + this.FileName;
        }
    }
}
