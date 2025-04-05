namespace CollectionManager.Common.Interfaces.Controls;

using CollectionManager.Core.Types;
using System.Collections;
using IOsuCollection = Core.Types.IOsuCollection;

public interface ICollectionListingView
{
    event GuiHelpers.LoadFileArgs OnLoadFile;
    event GuiHelpers.CollectionReorderEventArgs OnCollectionReorder;

    string SearchText { get; }

    OsuCollections Collections { set; }
    IOsuCollection SelectedCollection { get; set; }
    ArrayList SelectedCollections { get; }
    OsuCollections HighlightedCollections { set; }

    event EventHandler SearchTextChanged;
    event EventHandler SelectedCollectionChanged;
    event EventHandler SelectedCollectionsChanged;
    event GuiHelpers.CollectionBeatmapsEventArgs BeatmapsDropped;
    event EventHandler<string> RightClick;
    event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

    void SetFilter(ICommonModelFilter filter);
    void SetVisibleColumns(string[] visibleColumnsAspectNames);
    void FilteringStarted();
    void FilteringFinished();
    //void OnCollectionEditing(CollectionEditArgs e);

}