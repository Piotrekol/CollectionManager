using System;
using System.Net;

namespace CollectionManagerExtensionsDll.Utils
{
    public class ImpatientWebClient:WebClient
    {
        private int _timeout = 10000;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">After how many miliseconds stalled request should raise exception</param>
        public ImpatientWebClient(int timeout=5000) : base()
        {
            _timeout = timeout;
        }
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = _timeout;//Default time is 100s ...
            return w;
        }
    }
}