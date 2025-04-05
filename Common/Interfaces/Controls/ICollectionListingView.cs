using System.Collections;
using CollectionManager.DataTypes;
using Common;
using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface ICollectionListingView
    {
        event GuiHelpers.LoadFileArgs OnLoadFile;
        event GuiHelpers.CollectionReorderEventArgs OnCollectionReorder;

        string SearchText { get; }

        Collections Collections { set; }
        CollectionManager.DataTypes.ICollection SelectedCollection { get; set; }
        ArrayList SelectedCollections { get; }
        Collections HighlightedCollections { set; }

        event EventHandler SearchTextChanged;
        event EventHandler SelectedCollectionChanged;
        event EventHandler SelectedCollectionsChanged;
        event GuiHelpers.CollectionBeatmapsEventArgs BeatmapsDropped;
        event EventHandler<StringEventArgs> RightClick;
        event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

        void SetFilter(ICommonModelFilter filter);
        void SetVisibleColumns(string[] visibleColumnsAspectNames);
        void FilteringStarted();
        void FilteringFinished();
        //void OnCollectionEditing(CollectionEditArgs e);

    }
}