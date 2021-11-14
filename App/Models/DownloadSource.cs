using Common.Interfaces;

namespace App.Models
{
    public class DownloadSource : IDownloadSource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Referer { get; set; }
        public string BaseDownloadUrl { get; set; }
        public bool ThrottleDownloads { get; set; }
        public int DownloadsPerMinute { get; set; }
        public int DownloadsPerHour { get; set; }
        public int DownloadThreads { get; set; }
        public string FullyQualifiedHandlerName { get; set; }
        public bool RequiresLogin { get; set; }
        public bool UseCookiesLogin { get; set; }
        public int RequestTimeout { get; set; }
    }
}