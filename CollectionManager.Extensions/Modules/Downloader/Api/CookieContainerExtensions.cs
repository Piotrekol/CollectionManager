namespace CollectionManager.Extensions.Modules.Downloader.Api;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

public static class CookieContainerExtensions
{
    /// <summary>
    /// Get List of <see cref="Cookie"/>s contained in <see cref="CookieContainer"/> using reflection
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static List<Cookie> ToList(this CookieContainer container)
    {
        List<Cookie> cookies = [];

        Hashtable table = (Hashtable)container.GetType().InvokeMember("m_domainTable",
            BindingFlags.NonPublic |
            BindingFlags.GetField |
            BindingFlags.Instance,
            null,
            container,
            new object[] { });

        foreach (object key in table.Keys)
        {

            if (key is not string domain)
            {
                continue;
            }

            if (domain.StartsWith("."))
            {
                domain = domain.Substring(1);
            }

            string address = string.Format("http://{0}/", domain);

            if (Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out Uri uri) == false)
            {
                continue;
            }

            foreach (Cookie cookie in container.GetCookies(uri))
            {
                cookies.Add(cookie);
            }
        }

        return cookies;
    }
}