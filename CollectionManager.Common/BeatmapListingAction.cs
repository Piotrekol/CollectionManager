namespace CollectionManager.Common;

public enum BeatmapListingAction
{
    DeleteBeatmapsFromCollection, //Remove maps from current collection 
    DownloadBeatmaps, // Open download links using (?)default browser
    DownloadBeatmapsManaged, // Download selected beatmaps using internal downloader
    OpenBeatmapPages, // Open beatmap pages in (?)default browser
    OpenInOsu, // Open selected beatmap in osu! via osu:// URI scheme
    OpenBeatmapFolder, //Open currently selected beatmap folder(s?)
    CopyBeatmapsAsText, //Copy text representation of selected beatmaps
    CopyBeatmapsAsUrls, //Copy map links of selected beatmaps
    PullWholeMapSet, //Finds all mapsets from selected maps and adds them to current collection
    ExportBeatmapSets, //Compress selected mapsets back to .osz archives
}