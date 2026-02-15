namespace CollectionManager.App.Shared.Misc;

using CollectionManager.App.Shared.Interfaces;
using CollectionManager.Extensions.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

public class UpdateChecker : IUpdateModel
{
    private const string baseGithubUrl = "https://api.github.com/repos/Piotrekol/CollectionManager";
    private const string githubUpdateUrl = baseGithubUrl + "/releases/latest";

    private readonly Lazy<FileVersionInfo> _versionInfo = new(() =>
    {
        string executableLocation = Environment.ProcessPath;

        return string.IsNullOrEmpty(executableLocation)
            ? null
            : FileVersionInfo.GetVersionInfo(executableLocation);
    });

    public bool Error { get; private set; }
    public Version OnlineVersion { get; private set; }
    public string NewVersionLink { get; private set; }
    public string CurrentProductVersion
        => _versionInfo.Value?.ProductVersion ?? "unknown";

    public Version CurrentVersion
        => field ??= _versionInfo.Value is { FileVersion: { } v }
        ? new Version(v)
        : new Version(-1, -1, -1, -1);

    public bool UpdateIsAvailable => OnlineVersion != null && OnlineVersion > CurrentVersion;

    public bool CheckForUpdates()
    {
        if (CurrentVersion.MajorRevision < 0)
        {
            Error = true;
            return false;
        }

        string data = GetStringData(githubUpdateUrl);
        if (string.IsNullOrEmpty(data))
        {
            Error = true;
            return false;
        }

        JObject json;
        try
        {
            json = JObject.Parse(data);
        }
        catch (JsonReaderException)
        {
            return false;
        }

        string newestReleaseVersion = json["tag_name"].ToString();
        OnlineVersion = new Version(newestReleaseVersion);
        NewVersionLink = json["html_url"].ToString();

        return UpdateIsAvailable;
    }

    private string GetStringData(string url)
    {
        try
        {
            using ImpatientWebClient wc = new();
            wc.Headers.Add("user-agent", $"CollectionManager_Updater_{CurrentVersion}");
            return wc.DownloadString(url);
        }
        catch
        {
            return string.Empty;
        }
    }
}