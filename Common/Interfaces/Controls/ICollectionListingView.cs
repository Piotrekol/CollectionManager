using System;
using System.Collections;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
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

        void SetFilter(IModelFilter filter);
        void FilteringStarted();
        void FilteringFinished();
        //void OnCollectionEditing(CollectionEditArgs e);

    }
}