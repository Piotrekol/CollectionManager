﻿using CollectionManager.DataTypes;
using Common;
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
        event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
        event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        Beatmaps SelectedBeatmaps { get; set; }
        Beatmap SelectedBeatmap { get; set; }
        ICollection CurrentCollection { get; }
        void EmitBeatmapOperation(BeatmapListingAction args);
        void EmitBeatmapsDropped(object sender,Beatmaps beatmaps);
        Beatmaps GetBeatmaps();
        void SetBeatmaps(Beatmaps beatmaps);
        void SetCollection(ICollection collection);
        void FilterBeatmaps(string text);
        ICommonModelFilter GetFilter();
    }
}