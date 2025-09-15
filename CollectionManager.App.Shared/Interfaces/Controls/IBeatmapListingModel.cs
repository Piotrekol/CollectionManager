namespace CollectionManager.App.Shared.Interfaces.Controls;

using CollectionManager.Common;
using CollectionManager.Core.Types;

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
    IOsuCollection CurrentCollection { get; }
    void EmitBeatmapOperation(BeatmapListingAction args);
    void EmitBeatmapsDropped(object sender, Beatmaps beatmaps);
    Beatmaps GetBeatmaps();
    void SetBeatmaps(Beatmaps beatmaps);
    void SetCollection(IOsuCollection collection);
    void FilterBeatmaps(string text);
    ICommonModelFilter GetFilter();
}