namespace CollectionManager.App.Shared.Presenters.Controls;

public class BeatmapListingPresenterSettings
{
    public string[] VisibleColumns { get; set; } = [];
    public string GroupBy { get; set; } = "";
    public bool Collapsed { get; set; }
}