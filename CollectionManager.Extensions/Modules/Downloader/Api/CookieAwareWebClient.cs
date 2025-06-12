namespace CollectionManager.Extensions.Modules.Downloader.Api;

using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

public class CookieAwareWebClient : WebClient
{
    public int ClientId = -1;
    public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";
    public int RequestTimeout { get; set; } = 5000;
    public CookieContainer CookieContainer { get; set; }
    private static readonly char[] separator = new[] { '=' };

    public CookieAwareWebClient(CookieContainer container)
    {
        CookieContainer = container;
    }

    public CookieAwareWebClient()
      : this(new CookieContainer())
    { }

    public void SetCookies(string cookies, string[] cookiesToIgnore, string cookieDomain)
    {
        foreach (string nameValuePair in HttpUtility.UrlDecode(cookies).Split(';'))
        {
            string[] split = nameValuePair.Split(separator, 2);
            string cookieName = split[0].Trim();

            if (cookiesToIgnore.Contains(cookieName) || split.Length is not 2)
            {
                continue;
            }

            string encodedCookieValue = HttpUtility.UrlEncode(split[1]);
            CookieContainer.Add(new Cookie(cookieName, encodedCookieValue, "/", cookieDomain));
        }
    }

    public bool IsLoggedIn(string checkUrl, string stringToFind)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(checkUrl);
        request.CookieContainer = CookieContainer;
        request.UserAgent = UserAgent;
        try
        {
            WebResponse response = request.GetResponse() as HttpWebResponse;
            using StreamReader reader = new(response.GetResponseStream());
            string responseText = reader.ReadToEnd();
            return !responseText.Contains(stringToFind, StringComparison.InvariantCultureIgnoreCase);
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

    public void Login(string loginPageAddress, string loginData)
    {
        HttpWebRequest homePageRequest = (HttpWebRequest)WebRequest.Create("https://osu.ppy.sh/home");
        homePageRequest.CookieContainer = CookieContainer;
        HttpWebResponse homeResponse = (HttpWebResponse)homePageRequest.GetResponse();
        string token = homeResponse.Cookies["XSRF-TOKEN"].Value;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        byte[] buffer = Encoding.ASCII.GetBytes("_token=" + token + "&" + loginData.ToString());
        request.ContentLength = buffer.Length;
        request.CookieContainer = CookieContainer;
        request.UserAgent = UserAgent;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(buffer, 0, buffer.Length);
        requestStream.Close();
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
        HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
        request.CookieContainer = CookieContainer;
        request.Timeout = RequestTimeout;
        request.UserAgent = UserAgent;
        request.AllowAutoRedirect = true;
        return request;
    }
}
