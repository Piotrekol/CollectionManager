namespace CollectionManager.Extensions.Modules.Downloader.Api;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public abstract class DownloadManager : IDisposable
{
    protected Queue<CookieAwareWebClient> Clients = new();
    private readonly LinkedList<DownloadItem> _urlsToDownload = new();
    private readonly ConcurrentQueue<FileWorkerArgs> FileOperations = new();
    private readonly Timer _urlWatcher;
    private readonly Timer _ProgressWatcher;
    private readonly string _saveLocation;

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

    private readonly Dictionary<int, DownloadProgress> downloadCheck = [];
    private bool _stopDownloads;
    public event EventHandler<DownloadProgressChangedEventArgs> ProgressUpdated;
    private static readonly object _lockingObject = new();
    public DownloadManager(string saveLocation, int downloadThreads)
    {
        _saveLocation = saveLocation;

        for (int i = 0; i < downloadThreads; i++)
        {
            CookieAwareWebClient webClient = new()
            {
                ClientId = i
            };
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
            Clients.Enqueue(webClient);
            downloadCheck.Add(i, new DownloadProgress());
        }

        //Run callback every 500ms with null as state
        _urlWatcher = new Timer(Callback, null, 0, 250);
        _ProgressWatcher = new Timer(ProgressWatcher, null, 0, 5000);

    }

    public virtual bool Login(LoginData loginData) => true;

    public static void ChangeDefaultConnectionPolicy(int maxConnectionsToSameServer) => ServicePointManager.DefaultConnectionLimit = maxConnectionsToSameServer;

    private void ProgressWatcher(object state)
    {
        lock (_lockingObject)
        {
            foreach (KeyValuePair<int, DownloadProgress> dlItemCheck in downloadCheck)
            {
                if (dlItemCheck.Value.IsStalled())
                {
                    dlItemCheck.Value.downloadItem.WebClient.CancelAsync();
                }

                dlItemCheck.Value.Process();
            }
        }

        while (!FileOperations.IsEmpty)
        {
            if (FileOperations.TryDequeue(out FileWorkerArgs args))
            {
                switch (args.action)
                {
                    case "removeTemp":
                        try
                        {
                            if (File.Exists(args.orginalLocation))
                            {
                                File.Delete(args.orginalLocation);
                            }
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
                foreach (KeyValuePair<int, DownloadProgress> dlItemCheck in downloadCheck)
                {
                    dlItemCheck.Value.downloadItem?.WebClient.CancelAsync();
                }
            }
            else
            {
                lock (_urlsToDownload)
                {
                    if (_urlsToDownload.Count > 0)
                    {
                        if (Clients.Count > 0)
                        {
                            DownloadItem downloadItem = _urlsToDownload.First.Value;
                            if (!CanDownload(downloadItem))
                            {
                                return;
                            }

                            downloadItem.DownloadSlotStatus = "Starting download...";
                            CookieAwareWebClient client = Clients.Dequeue();
                            downloadItem.DownloadSlotStatus = null;
                            _urlsToDownload.RemoveFirst();
                            client.RequestTimeout = downloadItem.RequestTimeout;
                            downloadItem.WebClient = client;
                            _ = DownloadFile(downloadItem);
                        }
                    }
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
            DownloadItem url = (DownloadItem)e.UserState;
            bool error = false;
            if (e.Cancelled)
            {
                url.DownloadAborted = true;//Progress = "download cancelled";
                error = true;
                lock (_urlsToDownload)
                {
                    _ = _urlsToDownload.AddFirst(url);
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
                            _ = Task.Run(async () =>
                            {
                                await Task.Delay(60 * 1000 * 10);
                                StopDownloads = false;
                            });
                        }

                        lock (_urlsToDownload)
                        {
                            _ = _urlsToDownload.AddFirst(url);
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

    private string GetFullLocation(string filename) => Path.Combine(_saveLocation, filename);
    private string GetFullTempLocation(string filename) => Path.Combine(_saveLocation, GetTempFilename(filename));
    private static string GetTempFilename(string path) => path + ".tmp";

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        int progress = e.ProgressPercentage;

        DownloadItem DlItem = (DownloadItem)e.UserState;
        downloadCheck[DlItem.WebClient.ClientId].bytesRecived = e.BytesReceived;
        if (DlItem.lastShownDlState != progress)
        {
            DlItem.lastShownDlState = progress;
            OnProgressUpdated(e);
        }
    }
    public DownloadItem DownloadFile(string url, string filename, string referer, object token, int requestTimeout)
    {
        DownloadItem dlItem = new() { FileName = filename, Url = url, Referer = referer, UserToken = token, RequestTimeout = requestTimeout };
        lock (_urlsToDownload)
        {
            _ = _urlsToDownload.AddLast(dlItem);
        }

        return dlItem;
    }

    protected virtual void OnProgressUpdated(DownloadProgressChangedEventArgs e)
    {
        DownloadItem dlItem = (DownloadItem)e.UserState;
        dlItem.BytesRecived = e.BytesReceived;
        dlItem.TotalBytes = e.TotalBytesToReceive;
        dlItem.ProgressPrecentage = e.ProgressPercentage;
        ProgressUpdated?.Invoke(this, e);
    }

    public void Dispose()
    {
        _ProgressWatcher?.Dispose();
        _urlWatcher?.Dispose();

        GC.SuppressFinalize(this);
    }
}
