namespace CollectionManager.Common.Interfaces.Controls;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using System;
using System.Collections;

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
    event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

    void SetCurrentPlayMode(PlayMode playMode);
    void SetCurrentMods(Mods mods);
    void SetBeatmaps(IEnumerable beatmaps);
    void SetVisibleColumns(string[] visibleColumnsAspectNames);
    void SetFilter(ICommonModelFilter filter);
    void FilteringStarted();
    void FilteringFinished();
    void ClearSelection();
    void SelectNextOrFirst();
}