using System;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using Gui.Misc;

namespace App.Interfaces
{
    public interface IBeatmapListingModel
    {
        event EventHandler BeatmapsChanged;
        event EventHandler FilteringStarted;
        event EventHandler FilteringFinished;

        event EventHandler SelectedBeatmapChanged;
        event EventHandler SelectedBeatmapsChanged;

        event EventHandler OpenBeatmapPages;
        event EventHandler DownloadBeatmaps;
        event EventHandler DownloadBeatmapsManaged;
        event EventHandler DeleteBeatmapsFromCollection;
        event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        Beatmaps SelectedBeatmaps { get; set; }
        Beatmap SelectedBeatmap { get; set; }
        Collection CurrentCollection { get; }
        void EmitOpenBeatmapPages();
        void EmitDownloadBeatmaps();
        void EmitDownloadBeatmapsManaged();
        void EmitDeleteBeatmapsFromCollection();
        void EmitBeatmapsDropped(object sender,Beatmaps beatmaps);
        Beatmaps GetBeatmaps();
        void SetBeatmaps(Beatmaps beatmaps);
        void SetCollection(Collection collection);
        void FilterBeatmaps(string text);
        IModelFilter GetFilter();
    }
}