namespace CollectionManager.Extensions.Modules.API.osu;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public partial class OsuSite : IDisposable
{
    private static readonly CompositeFormat parsingPage = CompositeFormat.Parse("Getting page {0} of {1}");
    private static readonly CompositeFormat parsingComplete = CompositeFormat.Parse("Finished Processing of {0} pages");
    private static readonly CompositeFormat UserPpRankingUrl = CompositeFormat.Parse("https://osu.ppy.sh/rankings/osu/performance?m=0&s=3&o=1&f=0&page={0}");

    private LogUsernameGeneration _logger;
    private readonly HttpClient httpClient = new();

    [GeneratedRegex(DefaultUsersRegexFormat, RegexOptions.IgnoreCase)]
    private static partial Regex DefaultUsersRegex();
    private readonly Regex CustomUsersRegex;

    public const string DefaultUsersRegexFormat = @"<a[^>]*data-user-id=[""']?(\d+)[""']?[^>]*>(?:[^<]*<span[^>]*>(.*?)<\/span>[^<]*)*<\/a>";
    public const int UsersPerPage = 50;
    public delegate void LogUsernameGeneration(string logMessage, int completionPercentage);

    private Regex GetUsersRegex()
    {
        if (CustomUsersRegex is not null)
        {
            return CustomUsersRegex;
        }

        return DefaultUsersRegex();
    }
    public OsuSite(string usersCaptureRegex = null)
    {
        if (!string.IsNullOrWhiteSpace(usersCaptureRegex) && usersCaptureRegex is not DefaultUsersRegexFormat)
        {
            CustomUsersRegex = new Regex(usersCaptureRegex, RegexOptions.IgnoreCase);
        }
    }

    public async Task<List<string>> GetUsernamesAsync(int startRank, int endRank, LogUsernameGeneration logger, CancellationToken cancellationToken)
    {
        if (startRank < 0 || startRank > 9999 || endRank < 0 || endRank > 10000)
        {
            throw new ArgumentException("Parameters were not in allowed range(0-10000)");
        }

        _logger = logger;

        int startPage = Convert.ToInt32(Math.Floor((double)(startRank / (double)UsersPerPage))) + 1;
        int endPage = Convert.ToInt32(Math.Ceiling((double)(endRank / (double)UsersPerPage)));

        int startUserIndex = (startRank % UsersPerPage) - 1;
        startUserIndex = startUserIndex < 0 ? 0 : startUserIndex;
        int endUserIndex = (endRank - 1) % UsersPerPage;

        return await GetUsernamesAsync(startPage, endPage, startUserIndex, endUserIndex, cancellationToken);
    }

    private async Task<List<string>> GetUsernamesAsync(int startPage, int endPage, int startUserIndex, int endUserIndex, CancellationToken cancellationToken)
    {
        int processedPages = 0;
        int totalPages = endPage - (startPage - 1);
        List<string> usernames = [];

        for (int i = startPage; i <= endPage; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger?.Invoke(string.Format(CultureInfo.InvariantCulture, parsingPage, i, endPage), Convert.ToInt32(processedPages++ / (float)totalPages * 100));

            string pageContents = await httpClient.GetStringAsync(string.Format(CultureInfo.InvariantCulture, UserPpRankingUrl, i), cancellationToken);
            List<string> pageUsernames = GetUsernamesFromPage(pageContents, i == startPage ? startUserIndex : 0, i == endPage ? endUserIndex : UsersPerPage - 1);
            usernames.AddRange(pageUsernames);
        }

        _logger?.Invoke(string.Format(CultureInfo.InvariantCulture, parsingComplete, totalPages), Convert.ToInt32(processedPages / (float)totalPages * 100));

        return usernames;
    }

    private List<string> GetUsernamesFromPage(string pageContents, int startUserIndex, int endUserIndex)
    {
        List<string> usernames = [];

        if (pageContents.Length > 0)
        {
            MatchCollection match = GetUsersRegex().Matches(pageContents.ReplaceLineEndings(string.Empty));
            if (match.Count > 0)
            {
                for (int i = startUserIndex; i <= endUserIndex; i++)
                {
                    Match entry = match[i];
                    string username = entry.Groups[2].Value.Trim();
                    usernames.Add(username);
                }
            }
        }

        return usernames;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        httpClient.Dispose();
    }
}
