namespace CollectionManagerApp.Interfaces.Controls;

public interface IInfoTextModel
{
    int BeatmapCount { get; set; }
    int BeatmapsInCollectionsCount { get; set; }
    int MissingMapSetsCount { get; set; }
    int CollectionsCount { get; set; }
    int UnknownMapCount { get; set; }
    IUpdateModel GetUpdater();
    void EmitUpdateTextClicked();
    event EventHandler CountsUpdated;
    event EventHandler UpdateTextClicked;

}