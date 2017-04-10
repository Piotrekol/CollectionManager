using System;

namespace GuiComponents.Interfaces
{
    public interface IInfoTextView
    {
        bool UpdateTextIsClickable { set; }
        string UpdateText { set; }
        string BeatmapLoaded { set; }
        string CollectionsLoaded { set; }
        string BeatmapsInCollections { set; }
        string BeatmapsMissing { set; }

        event EventHandler UpdateTextClicked;
    }
}