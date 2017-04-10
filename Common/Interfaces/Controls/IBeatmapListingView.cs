using System;
using System.Collections;
using System.Collections.Generic;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IBeatmapListingView
    {
        string SearchText { get; }
        string ResultText { get; set; }
        Beatmap SelectedBeatmap { get; }
        Beatmaps SelectedBeatmaps { get; }

        event EventHandler SearchTextChanged;
        event EventHandler SelectedBeatmapChanged;
        event EventHandler SelectedBeatmapsChanged;
        event EventHandler OpenBeatmapPages;
        event EventHandler DownloadBeatmaps;
        event EventHandler DownloadBeatmapsManaged;
        event EventHandler DeleteBeatmapsFromCollection;
        event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        void SetBeatmaps(IEnumerable beatmaps);
        void SetFilter(IModelFilter filter);
        void FilteringStarted();
        void FilteringFinished();
        void ClearSelection();
        void SelectNextOrFirst();
    }
}