using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public static class CookieContainerExtensions
    {
        /// <summary>
        /// Get List of <see cref="Cookie"/>s contained in <see cref="CookieContainer"/> using reflection
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static List<Cookie> ToList(this CookieContainer container)
        {
            var cookies = new List<Cookie>();

            var table = (Hashtable)container.GetType().InvokeMember("m_domainTable",
                BindingFlags.NonPublic |
                BindingFlags.GetField |
                BindingFlags.Instance,
                null,
                container,
                new object[] { });

            foreach (var key in table.Keys)
            {

                Uri uri = null;

                var domain = key as string;

                if (domain == null)
                    continue;

                if (domain.StartsWith("."))
                    domain = domain.Substring(1);

                var address = string.Format("http://{0}/", domain);

                if (Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out uri) == false)
                    continue;

                foreach (Cookie cookie in container.GetCookies(uri))
                {
                    cookies.Add(cookie);
                }
            }

            return cookies;
        }
    }
}