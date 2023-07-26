using System;
using System.Collections;
using System.Collections.Generic;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
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
        event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
        event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        void SetCurrentPlayMode(PlayMode playMode);
        void SetCurrentMods(Mods mods);
        void SetBeatmaps(IEnumerable beatmaps);
        void SetFilter(IModelFilter filter);
        void FilteringStarted();
        void FilteringFinished();
        void ClearSelection();
        void SelectNextOrFirst();
        void ClearCustomFieldDefinitions();
        void SetCustomFieldDefinitions(IEnumerable<CustomFieldDefinition> customFieldDefinitions);
    }
}