namespace Common
{
    public enum BeatmapListingAction
    {
        DeleteBeatmapsFromCollection, //Remove maps from current collection 
        DownloadBeatmaps, // Open download links using (?)default browser
        DownloadBeatmapsManaged, // Download selected beatmaps using internal downloader
        OpenBeatmapPages, // Open beatmap pages in (?)default browser
        CopyBeatmapsAsText, //Copy text representation of selected beatmaps
        CopyBeatmapsAsUrls //Copy map links of selected beatmaps
    }
}