﻿namespace Common
{
    public enum BeatmapListingAction
    {
        DeleteBeatmapsFromCollection, //Remove maps from current collection 
        DownloadBeatmaps, // Open download links using (?)default browser
        DownloadBeatmapsManaged, // Download selected beatmaps using internal downloader
        OpenBeatmapPages, // Open beatmap pages in (?)default browser
        OpenBeatmapFolder, //Open currently selected beatmap folder(s?)
        CopyBeatmapsAsText, //Copy text representation of selected beatmaps
        CopyBeatmapsAsUrls, //Copy map links of selected beatmaps
        PullWholeMapSet, //Finds all mapsets from selected maps and adds them to current collection
        ExportBeatmapSets, //Compress selected mapsets back to .osz archives
    }
}