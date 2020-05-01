using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;

namespace System.Net
{
    using System.Text;
    using System.Collections.Specialized;

    public class CookieAwareWebClient : WebClient
    {
        public int ClientId = -1;
        public bool Login(string loginPageAddress, string loginData)
        {
            var homePageRequest = (HttpWebRequest)WebRequest.Create("https://osu.ppy.sh/home");
            homePageRequest.CookieContainer = CookieContainer;
            var homeResponse = (HttpWebResponse)homePageRequest.GetResponse();
            var token = homeResponse.Cookies["XSRF-TOKEN"].Value;
            
            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var data = loginData.ToString();
            var buffer = Encoding.ASCII.GetBytes("_token=" + token + "&" + loginData.ToString());
            request.ContentLength = buffer.Length;
            request.CookieContainer = CookieContainer;
            var requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse resp && resp.StatusCode.GetHashCode() == 422)
                {
                    return false;
                }

                throw;
            }

            string responseText;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }
            response.Close();
            return !(responseText.IndexOf("Sign in", StringComparison.InvariantCultureIgnoreCase) > 0);
        }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieAwareWebClient()
          : this(new CookieContainer())
        { }

        public CookieContainer CookieContainer { get; private set; }

        public void SetCustomCookieContainer(CookieContainer CookieContainer)
        {
            this.CookieContainer = CookieContainer;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            request.Timeout = 5 * 1000;
            request.UserAgent = "CollectionManager";
            return request;
        }
    }
}
