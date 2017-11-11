using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CollectionManagerExtensionsDll.Modules.API.osu
{
    public class OsuSite
    {
        private static readonly string parsingPage = "Getting page {0} of {1}";
        private static readonly string parsingComplete = "Finished Processing of {0} pages";

        private static readonly string _baseUrl = "https://osu.ppy.sh/p";
        private static readonly string UserPpRankingUrl = _baseUrl + "/pp/?m=0&s=3&o=1&f=0&page={0}";
        private static readonly int UsersPerPage = 50;
        public delegate void LogUsernameGeneration(string logMessage, int completionPrecentage);

        private LogUsernameGeneration _logger;
        public List<string> GetUsernames(int startRank, int endRank, LogUsernameGeneration logger)
        {
            if (startRank < 0 || startRank > 9999 || endRank < 0 || endRank > 10000)
                throw new ArgumentException("Parameters were not in allowed range(0-10000)");
            _logger = logger;

            int startPage = Convert.ToInt32(Math.Floor((double)((double)startRank / (double)UsersPerPage))) + 1;
            int endPage = Convert.ToInt32(Math.Ceiling((double)((double)endRank / (double)UsersPerPage)));

            int startUserIndex = (startRank % UsersPerPage) - 1;
            startUserIndex = startUserIndex < 0 ? 0 : startUserIndex;
            int endUserIndex = (endRank - 1) % UsersPerPage;

            return GetUsernames(startPage, endPage, startUserIndex, endUserIndex);
        }

        private List<string> GetUsernames(int startPage, int endPage, int startUserIndex, int endUserIndex)
        {
            int processedPages = 0;
            int totalPages = endPage - (startPage - 1);
            var usernames = new List<string>();
            using (var client = new WebClient())
            {
                for (int i = startPage; i <= endPage; i++)
                {
                    _logger?.Invoke(string.Format(parsingPage, i, endPage), Convert.ToInt32(((float)processedPages++ / (float)totalPages) * 100));
                    var pageContents = client.DownloadString(string.Format(UserPpRankingUrl, i));
                    var pageUsernames = GetUsernamesFromPage(pageContents, (i == startPage) ? startUserIndex : 0, (i == endPage) ? endUserIndex : UsersPerPage - 1);
                    usernames.AddRange(pageUsernames);
                }
                _logger?.Invoke(string.Format(parsingComplete, totalPages), Convert.ToInt32(((float)processedPages / (float)totalPages) * 100));
            }
            return usernames;
        }

        private List<string> GetUsernamesFromPage(string pageContents, int startUserIndex, int endUserIndex)
        {
            var usernames = new List<string>();
            if (pageContents.Length > 0)
            {
                var match = Regex.Matches(pageContents, ".*?href=\'\\/u\\/(\\d+)\'>(.*)<\\/a>");
                if (match.Count > 0)
                {
                    for (int i = startUserIndex; i <= endUserIndex; i++)
                    {
                        var entry = match[i];
                        //var userId = entry.Groups[1].Value;
                        var username = entry.Groups[2].Value;
                        usernames.Add(username);
                    }
                }
            }
            return usernames;
        }
    }
}
