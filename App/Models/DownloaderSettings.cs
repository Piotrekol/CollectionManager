namespace CollectionManagerApp.Models;

using CollectionManager.Common.Interfaces;
using CollectionManager.Extensions.Modules.Downloader.Api;
using System.IO;

public class DownloaderSettings
{
    public string DownloadDirectory { get; set; }
    public bool? DownloadWithVideo { get; set; }
    public LoginData LoginData { get; set; }

    public bool IsValid(IReadOnlyList<IDownloadSource> downloadSources)
    {
        bool hasValidLoginData = LoginData.IsValid();
        if (!hasValidLoginData)
        {
            hasValidLoginData = !downloadSources.FirstOrDefault(s => s.Name == LoginData.DownloadSource)?.RequiresLogin ?? false;
        }

        return !string.IsNullOrWhiteSpace(DownloadDirectory)
               && Directory.Exists(DownloadDirectory)
               && DownloadWithVideo.HasValue
               && hasValidLoginData;
    }

    public override string ToString() => $"Directory: \"{DownloadDirectory}\"{Environment.NewLine}" +
               $"With video: {DownloadWithVideo}{Environment.NewLine}" +
               $"{LoginData}";
}