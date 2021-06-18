using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Common.Interfaces;

namespace App.Models
{
    public class DownloaderSettings
    {
        public string DownloadDirectory { get; set; }
        public bool? DownloadWithVideo { get; set; }
        public LoginData LoginData { get; set; }

        public bool IsValid(IReadOnlyList<IDownloadSource> downloadSources)
        {
            var hasValidLoginData = LoginData.IsValid();
            if (!hasValidLoginData)
                hasValidLoginData = !downloadSources.FirstOrDefault(s => s.Name == LoginData.DownloadSource)?.RequiresLogin ?? false;

            return !string.IsNullOrWhiteSpace(DownloadDirectory)
                   && Directory.Exists(DownloadDirectory)
                   && DownloadWithVideo.HasValue
                   && hasValidLoginData;
        }

        public override string ToString()
        {
            return $"Directory: \"{DownloadDirectory}\"{Environment.NewLine}" +
                   $"With video: {DownloadWithVideo}{Environment.NewLine}" +
                   $"{LoginData}";
        }
    }
}