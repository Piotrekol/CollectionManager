using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Net
{
    public class CookieAwareWebClient : WebClient
    {
        public int ClientId = -1;
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36";
        public int RequestTimeout { get; set; } = 5000;
        public void SetCookies(string cookies, string[] cookiesToIgnore, string cookieDomain)
        {
            foreach (var nameValuePair in HttpUtility.UrlDecode(cookies).Split(';'))
            {
                var split = nameValuePair.Split('=');
                if (cookiesToIgnore.Contains(split[0].Trim()))
                    continue;
                CookieContainer.Add(new Cookie(split[0].Trim(), split[1], "/", cookieDomain));
            }
        }

        public bool IsLoggedIn(string checkUrl, string stringToFind)
        {
            var request = (HttpWebRequest)WebRequest.Create(checkUrl);
            request.CookieContainer = CookieContainer;
            request.UserAgent = UserAgent;
            try
            {
                WebResponse response = request.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    return responseText.IndexOf(stringToFind, StringComparison.InvariantCultureIgnoreCase) == -1;
                }
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse resp && resp.StatusCode.GetHashCode() == 422)
                {
                    return false;
                }

                throw;
            }
        }

        public bool Login(string loginPageAddress, string loginData)
        {
            var homePageRequest = (HttpWebRequest)WebRequest.Create("https://osu.ppy.sh/home");
            homePageRequest.CookieContainer = CookieContainer;
            var homeResponse = (HttpWebResponse)homePageRequest.GetResponse();
            var token = homeResponse.Cookies["XSRF-TOKEN"].Value;

            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var buffer = Encoding.ASCII.GetBytes("_token=" + token + "&" + loginData.ToString());
            request.ContentLength = buffer.Length;
            request.CookieContainer = CookieContainer;
            request.UserAgent = UserAgent;
            var requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            return IsLoggedIn("https://osu.ppy.sh/home", "Sign in");
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
            request.Timeout = RequestTimeout;
            request.UserAgent = UserAgent;
            request.AllowAutoRedirect = true;
            return request;
        }
    }
}
