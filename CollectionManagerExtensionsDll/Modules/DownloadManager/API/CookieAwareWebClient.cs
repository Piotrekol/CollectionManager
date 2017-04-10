using System.IO;

namespace System.Net
{
    using System.Text;
    using System.Collections.Specialized;

    public class CookieAwareWebClient : WebClient
    {
        public int ClientId = -1;
        public string Login(string loginPageAddress, string loginData)
        {
            CookieContainer container;

            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var data = loginData.ToString();
            var buffer = Encoding.ASCII.GetBytes(loginData.ToString());
            request.ContentLength = buffer.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            container = request.CookieContainer = new CookieContainer();

            var response = request.GetResponse();
            string ResponseText = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                ResponseText = sr.ReadToEnd();
            }
            response.Close();
            CookieContainer = container;
            return ResponseText;
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
            return request;
        }
    }
}