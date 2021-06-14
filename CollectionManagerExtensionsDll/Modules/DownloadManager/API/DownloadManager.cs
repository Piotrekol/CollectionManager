using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{

    public abstract class DownloadManager
    {
        protected Queue<CookieAwareWebClient> Clients = new Queue<CookieAwareWebClient>();
        readonly LinkedList<DownloadItem> _urlsToDownload = new LinkedList<DownloadItem>();
        ConcurrentQueue<FileWorkerArgs> FileOperations = new ConcurrentQueue<FileWorkerArgs>();
        private Timer _urlWatcher;
        private Timer _ProgressWatcher;
        private string _saveLocation;

        public bool StopDownloads
        {
            get => _stopDownloads;
            set
            {
                lock (_lockingObject)
                {
                    _stopDownloads = value;
                }
            }
        }

        Dictionary<int, DownloadProgress> downloadCheck = new Dictionary<int, DownloadProgress>();
        private bool _stopDownloads;
        public event EventHandler<DownloadProgressChangedEventArgs> ProgressUpdated;
        private static object _lockingObject = new object();
        public DownloadManager(string saveLocation, int downloadThreads)
        {
            _saveLocation = saveLocation;

            for (int i = 0; i < downloadThreads; i++)
            {
                var webClient = new CookieAwareWebClient();
                webClient.ClientId = i;
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                Clients.Enqueue(webClient);
                downloadCheck.Add(i, new DownloadProgress());
            }

            //Run callback every 500ms with null as state
            _urlWatcher = new Timer(Callback, null, 0, 250);
            _ProgressWatcher = new Timer(ProgressWatcher, null, 0, 5000);

        }

        public virtual bool Login(LoginData loginData)
        {
            return true;
        }

        public void ChangeDefaultConnectionPolicy(int maxConnectionsToSameServer)
        {
            ServicePointManager.DefaultConnectionLimit = maxConnectionsToSameServer;
        }

        private void ProgressWatcher(object state)
        {
            lock (_lockingObject)
            {
                foreach (var dlItemCheck in downloadCheck)
                {
                    if (dlItemCheck.Value.IsStalled())
                    {
                        dlItemCheck.Value.downloadItem.WebClient.CancelAsync();
                    }
                    dlItemCheck.Value.Process();
                }
            }
            while (FileOperations.Count > 0)
            {
                FileWorkerArgs args;
                if (FileOperations.TryDequeue(out args))
                {
                    switch (args.action)
                    {
                        case "removeTemp":
                            try
                            {
                                if (File.Exists(args.orginalLocation))
                                    File.Delete(args.orginalLocation);
                            }
                            catch (IOException) { }
                            break;
                        case "moveTemp":
                            if (File.Exists(args.orginalLocation))
                            {
                                if (!File.Exists(args.desiredLocation))
                                {
                                    File.Move(args.orginalLocation, args.desiredLocation);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private void Callback(object state)
        {
            //Main async download loop
            lock (_lockingObject)
            {
                if (StopDownloads)
                {
                    foreach (var dlItemCheck in downloadCheck)
                    {
                        dlItemCheck.Value.downloadItem?.WebClient.CancelAsync();
                    }
                }
                else
                    lock (_urlsToDownload)
                        if (_urlsToDownload.Count > 0)
                        {
                            if (Clients.Count > 0)
                            {
                                var downloadItem = _urlsToDownload.First.Value;
                                if (!CanDownload(downloadItem))
                                    return;

                                var client = Clients.Dequeue();
                                downloadItem.DownloadSlotStatus = null;
                                _urlsToDownload.RemoveFirst();
                                downloadItem.WebClient = client;
                                DownloadFile(downloadItem);
                            }
                        }
            }

        }

        public abstract bool CanDownload(DownloadItem downloadItem);

        protected virtual bool DownloadFile(DownloadItem downloadItem)
        {
            lock (_lockingObject)
            {
                string filePath = Path.Combine(_saveLocation, downloadItem.FileName);
                if (File.Exists(filePath))
                {
                    downloadItem.FileAlreadyExists = true;
                    Clients.Enqueue(downloadItem.WebClient);
                    return false;
                }
                downloadCheck[downloadItem.WebClient.ClientId].Reset();
                downloadItem.ResetErrorState();
                downloadCheck[downloadItem.WebClient.ClientId].downloadItem = downloadItem;
                downloadItem.WebClient.Headers["Referer"] = downloadItem.Referer;
                downloadItem.WebClient.DownloadFileAsync(new Uri(downloadItem.Url),
                    GetFullTempLocation(downloadItem.FileName), downloadItem);
                return true;
            }
        }

        internal class FileWorkerArgs
        {
            public string action { get; set; }
            public string orginalLocation { get; set; }
            public string desiredLocation { get; set; }
        }
        protected virtual void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lock (_lockingObject)
            {
                var url = (DownloadItem)e.UserState;
                bool error = false;
                if (e.Cancelled)
                {
                    url.DownloadAborted = true;//Progress = "download cancelled";
                    error = true;
                    lock (_urlsToDownload)
                    {
                        _urlsToDownload.AddFirst(url);
                    }
                }
                else if (e.Error != null)
                {
                    bool handled = false;
                    if (e.Error is WebException ex && ex.Response is HttpWebResponse response)
                    {
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            //deleted download
                            url.Error = "This beatmap is not available for download";
                            handled = error = true;
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            url.OtherError = true;
                            url.Error = "Download limit hit - download has been paused (next check in 10minutes)";
                            if (!StopDownloads)
                            {
                                StopDownloads = true;
                                Task.Run(async () =>
                                {
                                    await Task.Delay(60 * 1000 * 10);
                                    StopDownloads = false;
                                });
                            }

                            lock (_urlsToDownload)
                            {
                                _urlsToDownload.AddFirst(url);
                            }

                            handled = true;
                        }
                    }

                    if (!handled)
                    {
                        url.OtherError = true;
                        url.Error = "Fatal error: " + e.Error;
                        error = true;
                    }


                }

                if (error)
                {
                    string tempFileLocation = GetFullTempLocation(url.FileName);
                    FileOperations.Enqueue(new FileWorkerArgs()
                    {
                        action = "removeTemp",
                        orginalLocation = tempFileLocation
                    });
                }
                else
                {
                    string tempFileLocation = GetFullTempLocation(url.FileName);
                    string fileLocation = GetFullLocation(url.FileName);
                    FileOperations.Enqueue(new FileWorkerArgs()
                    {
                        action = "moveTemp",
                        orginalLocation = tempFileLocation,
                        desiredLocation = fileLocation
                    });

                }
                downloadCheck[url.WebClient.ClientId].Reset();
                Clients.Enqueue(url.WebClient);
            }
        }

        private string GetFullLocation(string filename)
        {
            return Path.Combine(_saveLocation, filename);
        }
        private string GetFullTempLocation(string filename)
        {
            return Path.Combine(_saveLocation, GetTempFilename(filename));
        }
        private string GetTempFilename(string path)
        {
            return path + ".tmp";
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;

            var DlItem = (DownloadItem)e.UserState;
            downloadCheck[DlItem.WebClient.ClientId].bytesRecived = e.BytesReceived;
            if (DlItem.lastShownDlState != progress)
            {
                DlItem.lastShownDlState = progress;
                OnProgressUpdated(e);
            }
        }
        public DownloadItem DownloadFileAsync(string url, string filename, string referer, object token)
        {
            var dlItem = new DownloadItem() { FileName = filename, Url = url, Referer = referer, UserToken = token };
            lock (_urlsToDownload)
                _urlsToDownload.AddLast(dlItem);
            return dlItem;
        }

        protected virtual void OnProgressUpdated(DownloadProgressChangedEventArgs e)
        {
            var dlItem = (DownloadItem)e.UserState;
            dlItem.BytesRecived = e.BytesReceived;
            dlItem.TotalBytes = e.TotalBytesToReceive;
            dlItem.ProgressPrecentage = e.ProgressPercentage;
            ProgressUpdated?.Invoke(this, e);
        }
    }
}
