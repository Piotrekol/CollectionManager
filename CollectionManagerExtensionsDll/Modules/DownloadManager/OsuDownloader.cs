using System;
using System.Collections.Generic;
using System.Net;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager
{
    public class OsuDownloader : API.DownloadManager
    {
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
                    var response = client.Login(loginAddress, loginDataStr);
                    if (response.IndexOf("Log me on automatically each visit", StringComparison.InvariantCultureIgnoreCase) > 0)
                        return false;
                    cookies = client.CookieContainer;
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
    }
}
