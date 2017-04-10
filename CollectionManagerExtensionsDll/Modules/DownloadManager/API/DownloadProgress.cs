using System;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    internal class DownloadProgress
    {
        public long SameValue = 0;
        public long LastBytesRecived { get; set; } = -2;
        public long bytesRecived { get; set; } = -1;
        public DownloadItem downloadItem { get; set; } = null;
        public bool IsStalled()
        {
            if (downloadItem == null)
                return false;
            if (bytesRecived == LastBytesRecived)
            {
                SameValue++;
                return SameValue > 1;
            }
            SameValue = 0;
            return false;
        }

        public void Process()
        {
            LastBytesRecived = bytesRecived;
        }

        public void Reset()
        {
            LastBytesRecived = -2;
            bytesRecived = -1;
            SameValue = 0;
            downloadItem = null;
        }
    }
}