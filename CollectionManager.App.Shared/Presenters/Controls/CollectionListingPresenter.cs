namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Misc;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using System.IO;
using SortOrder = Core.Enums.SortOrder;

public class CollectionListingPresenter
{
    private readonly ICollectionListingView _view;

    private OsuCollections _collections;
    private readonly ICollectionListingModel _model;
    private readonly IUserDialogs _userDialogs;

    public OsuCollections Collections
    {
        get => _collections;
        set
        {
            _collections = value;
            IOsuCollection selectedCollection = _view.SelectedCollection;
            _view.Collections = value;
            if (_collections.Contains(selectedCollection))
            {
                _view.SelectedCollection = selectedCollection;
            }
        }
    }
    public CollectionListingPresenter(ICollectionListingView view, ICollectionListingModel model, IUserDialogs userDialogs)
    {
        _view = view;
        _view.RightClick += _view_RightClick;
        _view.SelectedCollectionsChanged += ViewOnSelectedCollectionsChanged;
        _view.BeatmapsDropped += ViewOnBeatmapsDropped;
        _view.OnCollectionReorder += ViewOnCollectionReorder;
        _model = model;
        _userDialogs = userDialogs;
        _model.CollectionsChanged += ModelOnCollectionsChanged;
        Collections = _model.GetCollections();
    }

    private void ViewOnCollectionReorder(object sender, OsuCollections collections, OsuCollection targetCollection, bool placeBefore, string sortColumn, SortOrder sortOrder)
    {
        if (!Initalizer.Settings.DontAskAboutReorderingCollections)
        {
            (bool Result, bool doNotAskAgain) = _userDialogs.YesNoMessageBox($"Reordering collections will rename all loaded collections, proceed?", "Reordering", MessageBoxType.Question,
                    "Don't ask me again");
            Initalizer.Settings.DontAskAboutReorderingCollections = doNotAskAgain;
            Initalizer.Settings.Save();
            if (!Result)
            {
                return;
            }
        }

        _model.EmitCollectionEditing(CollectionEditArgs.ReorderCollections(collections.Names, targetCollection.Name, placeBefore, sortColumn, sortOrder));
    }

    private void ViewOnBeatmapsDropped(object sender, Beatmaps args, string collectionName) => _model.EmitCollectionEditing(CollectionEditArgs.AddBeatmaps(collectionName, args));

    private void ViewOnSelectedCollectionsChanged(object sender, EventArgs eventArgs)
    {
        OsuCollections selectedCollections = [];
        foreach (object collection in _view.SelectedCollections)
        {
            selectedCollections.Add((OsuCollection)collection);
        }

        _model.SelectedCollections = selectedCollections;
    }

    private void _view_RightClick(object sender, string action)
    {
        OsuCollections selectedCollections = _model.SelectedCollections;
        CollectionEditArgs args;
        switch (action)
        {
            case "Delete":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionEditArgs.RemoveCollections(selectedCollections.Names);
                break;
            case "Merge":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionEditArgs.MergeCollections(selectedCollections.Names, $"{selectedCollections[0].Name}_Merged");
                break;
            case "Intersect":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionEditArgs.IntersectCollections(selectedCollections.Names, $"{selectedCollections[0].Name}_Intersection");
                break;
            case "Inverse":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionEditArgs.InverseCollections(selectedCollections.Names, $"{selectedCollections[0].Name}_Inverse");
                break;
            case "Difference":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionEditArgs.DifferenceCollections(selectedCollections.Names, $"{selectedCollections[0].Name}_Difference");
                break;
            case "Create":
                args = CollectionEditArgs.AddCollections(null);
                break;
            case "Rename":
                if (_view.SelectedCollection == null)
                {
                    return;
                }

                args = CollectionEditArgs.RenameCollection(_view.SelectedCollection, null);
                break;
            case "Duplicate":
                if (_view.SelectedCollection == null)
                {
                    return;
                }

                args = CollectionEditArgs.DuplicateCollection(_view.SelectedCollection.Name, $"{_view.SelectedCollection.Name}_Duplicated");
                break;
            case "Export":
                if (selectedCollections == null)
                {
                    return;
                }

                args = CollectionExportEditArgs.ExportBeatmaps(selectedCollections.Names);
                break;
            case "Copy":
                if (selectedCollections == null)
                {
                    return;
                }

                string tempFolder = Path.Combine(Path.GetTempPath(), "CMcollections");
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }

                _ = Directory.CreateDirectory(tempFolder);
                string fileName = selectedCollections[0].Name.StripInvalidFileNameCharacters("_");
                string tempLocation = Path.Combine(tempFolder, $"{fileName}.osdb");
                Initalizer.OsuFileIo.CollectionLoader.SaveOsdbCollection(selectedCollections, tempLocation);
                Helpers.SetFileDropList([tempLocation]);
                return;
            default:
                return;
        }

        _model.EmitCollectionEditing(args);
    }

    private void ModelOnCollectionsChanged(object sender, EventArgs eventArgs) => Collections = _model.GetCollections();
}
