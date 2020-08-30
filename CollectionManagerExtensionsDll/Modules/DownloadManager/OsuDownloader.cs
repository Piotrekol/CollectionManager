using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Newtonsoft.Json;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager
{
    public class OsuDownloader : API.DownloadManager
    {
        public DownloadThrottler DownloadThrottler { get; } = new DownloadThrottler(5, 170);
        public string QuotaCheckUrl => @"https://osu.ppy.sh/home/download-quota-check";
        private CookieAwareWebClient webClient;
        public int GetUsedQuota()
        {
            if (webClient == null)
                return -1;

            var data = webClient.DownloadString(QuotaCheckUrl);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (deserialized.ContainsKey("quota_used"))
                return Convert.ToInt32(deserialized["quota_used"]);

            return -1;
        }

        public OsuDownloader(string saveLocation, int downloadThreads) : base(saveLocation, downloadThreads)
        {
        }

        public bool Login(LoginData loginData)
        {
            var loginAddress = @"https://osu.ppy.sh/session";
            string loginDataStr = string.Format("username={0}&password={1}", Uri.EscapeDataString(loginData.Username), Uri.EscapeDataString(loginData.Password));

            CookieContainer cookies = null;
            //Take all webClients and login/set correct cookies
            List<CookieAwareWebClient> webClients = new List<CookieAwareWebClient>();
            var clientCount = this.Clients.Count;
            for (int i = clientCount; i > 0; i--)
            {
                var client = this.Clients.Dequeue();
                if (i == clientCount)
                {
                    if (!string.IsNullOrEmpty(loginData.OsuCookies))
                    {
                        client.SetCookies(loginData.OsuCookies, new[] { "_encid" }, ".ppy.sh");
                        if (!client.IsLoggedIn(@"https://osu.ppy.sh/home", "Sign in"))
                            return false;
                    }
                    else
                    {
                        if (!client.Login(loginAddress, loginDataStr))
                            return false;
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
            foreach (var client in webClients)
            {
                this.Clients.Enqueue(client);
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
            if(e.Error == null)
                DownloadThrottler.RegisterDownload();

            base.DownloadCompleted(sender, e);
        }
    }
}
