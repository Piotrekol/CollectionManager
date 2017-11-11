using System;

namespace GuiComponents.Interfaces
{
    public interface IMainSidePanelView
    {
        event EventHandler LoadCollection;
        event EventHandler LoadDefaultCollection;
        event EventHandler ClearCollections;
        event EventHandler SaveCollections;
        event EventHandler SaveInvidualCollections;
        event EventHandler ListAllMaps;
        event EventHandler ListMissingMaps;
        event EventHandler ShowBeatmapListing;
        event EventHandler ShowDownloadManager;
        event EventHandler DownloadAllMissing;
        event EventHandler GenerateCollections;

    }
}