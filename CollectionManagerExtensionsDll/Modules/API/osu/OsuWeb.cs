using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace CollectionManagerExtensionsDll.Modules.API.osu
{
    public class OsuWeb
    {
        private Regex matchUsersRegex = new Regex(@".*?href='\/u\/(\d+)'>(.*)<\/a>", RegexOptions.Compiled);
        public const string PpRankingUrl = @"https://osu.ppy.sh/p/pp/?m=0&s=3&o=1&f=0&page={0}";//0-page
        public const string CountryRankingUrl = @"https://osu.ppy.sh/p/pp/?c={1}&m=0&s=3&o=1&f=&page={0}";//0-page, 1-country

        private string GetPageUrl(int pageNumber, string country = null)
        {
            if (country == null)
                return string.Format(PpRankingUrl, pageNumber);
            return string.Format(CountryRankingUrl, pageNumber, country);
        }
        

        public List<string> GetUsernames(string country)
        {
            return GetUsernames(0, 10000, country);
        }

        public List<string> GetUsernames()
        {
            return GetUsernames(0, 10000);
        }
        public List<string> GetUsernames(int startRank, int endRank, string country = null)
        {
            var r = new List<string>();

            var pages = getPageRangeToProcess(startRank, endRank);
            var startPage = pages.Item1;
            var endPage = pages.Item2;
            //int usernamesToProcess = (endRank - startRank) + 1;
            using (var client = new WebClient())
            {
                for (int i = startPage.PageNumber; i <= endPage.PageNumber; i++)
                {
                    var pageContents = client.DownloadString(GetPageUrl(i, country));
                    bool hadMatches = ProcessPage(r, pageContents, i, pages);

                    if (!hadMatches)//Current page did not contain any records.
                        break;
                }
            }

            return r;
        }
        private bool ProcessPage(List<string> currentUsernames, string pageContents, int pageNumber, Tuple<PageRangeResult, PageRangeResult> pages)
        {
            int startUsernameIndex = 0;
            int endUsernameIndex = 50;
            if (pageNumber == pages.Item1.PageNumber)
                startUsernameIndex = pages.Item1.UserNumber;
            if (pageNumber == pages.Item2.PageNumber)
                endUsernameIndex = pages.Item2.UserNumber;

            if (pageContents.Length > 0)
            {
                var match = matchUsersRegex.Matches(pageContents);
                if (match.Count > 0)
                {
                    for (int i = startUsernameIndex; i < endUsernameIndex; i++)
                    {
                        var entry = match[i];
                        var userId = entry.Groups[1].Value;
                        var username = entry.Groups[2].Value;
                        currentUsernames.Add(username);
                    }
                    return true;
                }
            }
            return false;
        }


        private class PageRangeResult
        {
            public int PageNumber { get; set; } = 0;
            public int UserNumber { get; set; } = 0;
        }
        private Tuple<PageRangeResult, PageRangeResult> getPageRangeToProcess(int startRank, int endRank)
        {
            int UsersPerPage = 50;
            var startPage = new PageRangeResult();
            var endPage = new PageRangeResult();
            startPage.PageNumber = Convert.ToInt32(Math.Floor((double)((double)startRank / (double)UsersPerPage))) + 1;
            endPage.PageNumber = Convert.ToInt32(Math.Floor((double)((double)endRank / (double)UsersPerPage))) + 1;

            startPage.UserNumber = startRank % UsersPerPage - 1;
            if (startPage.UserNumber < 0)
                startPage.UserNumber = 0;
            endPage.UserNumber = endRank % UsersPerPage;
            return new Tuple<PageRangeResult, PageRangeResult>(startPage, endPage);
        }
    }
}