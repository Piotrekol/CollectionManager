namespace Common.Interfaces
{
    public interface IDownloadSource
    {
        /// <summary>
        /// Download source name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Short-ish description about this source
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Url template used for generating download links, {0} is replaced with mapId
        /// </summary>
        string BaseDownloadUrl { get; set; }
        /// <summary>
        /// Should downloads be throttled?
        /// </summary>
        bool ThrottleDownloads { get; set; }
        /// <summary>
        /// How many beatmaps user can download in a minute window
        /// </summary>
        int DownloadsPerMinute { get; set; }
        /// <summary>
        /// How many beatmaps user can download in a hourly window
        /// </summary>
        int DownloadsPerHour { get; set; }
        /// <summary>
        /// How many beatmaps user can download at the same time
        /// </summary>
        int DownloadThreads { get; set; }
        /// <summary>
        /// Full path to manager class for this source. "namespace, className"
        /// </summary>
        string FullyQualifiedHandlerName { get; set; }
        /// <summary>
        /// Does this source require user to be logged in?
        /// </summary>
        bool RequiresLogin { get; set; }
        /// <summary>
        /// Should site cookies be required instead of user and password for logging in?
        /// </summary>
        bool UseCookiesLogin { get; set; }
    }
}