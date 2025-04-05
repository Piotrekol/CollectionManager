namespace CollectionManager.Extensions.Modules.Downloader;

using CollectionManager.Extensions.Modules.Downloader.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

public class OsuDownloader : DownloadManager
{
    public DownloadThrottler DownloadThrottler { get; private set; }
    public string QuotaCheckUrl => @"https://osu.ppy.sh/home/download-quota-check";
    private CookieAwareWebClient webClient;
    public int GetUsedQuota()
    {
        if (webClient == null)
        {
            return -1;
        }

        string data = webClient.DownloadString(QuotaCheckUrl);
        Dictionary<string, object> deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        if (deserialized.ContainsKey("quota_used"))
        {
            return Convert.ToInt32(deserialized["quota_used"]);
        }

        return -1;
    }

    public OsuDownloader(string saveLocation, int downloadThreads, int downloadsPerMinute, int downloadsPerHour) : base(saveLocation, downloadThreads)
    {
        DownloadThrottler = new DownloadThrottler(downloadsPerMinute, downloadsPerHour);
    }

    public override bool Login(LoginData loginData)
    {
        string loginAddress = @"https://osu.ppy.sh/session";
        string loginDataStr = string.Format("username={0}&password={1}", Uri.EscapeDataString(loginData.Username), Uri.EscapeDataString(loginData.Password));

        CookieContainer cookies = null;
        //Take all webClients and login/set correct cookies
        List<CookieAwareWebClient> webClients = [];
        int clientCount = Clients.Count;
        for (int i = clientCount; i > 0; i--)
        {
            CookieAwareWebClient client = Clients.Dequeue();
            if (i == clientCount)
            {
                if (!string.IsNullOrEmpty(loginData.SiteCookies))
                {
                    client.SetCookies(loginData.SiteCookies, new[] { "_encid" }, ".ppy.sh");
                    if (!client.IsLoggedIn(@"https://osu.ppy.sh/rankings/osu/country", "sign in / register"))
                    {
                        return false;
                    }
                }
                else
                {
                    client.Login(loginAddress, loginDataStr);
                    if (!client.IsLoggedIn(@"https://osu.ppy.sh/rankings/osu/country", "sign in / register"))
                    {
                        return false;
                    }
                }

                cookies = client.CookieContainer;
                webClient = client;
            }
            else
            {
                client.SetCustomCookieContainer(cookies);
            }

            webClients.Add(client);
        }
        //Add webClients to Queue again
        foreach (CookieAwareWebClient client in webClients)
        {
            Clients.Enqueue(client);
        }

        return true;
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
