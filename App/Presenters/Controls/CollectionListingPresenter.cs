using System;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using App.Interfaces;
using GuiComponents.Interfaces;
using System.IO;
using System.Windows.Forms;
using System.Collections.Specialized;
using App.Misc;
using Common;
using App.Properties;

namespace App.Presenters.Controls
{
    public class CollectionListingPresenter
    {
        ICollectionListingView _view;

        private Collections _collections;
        private readonly ICollectionListingModel _model;
        private readonly IUserDialogs _userDialogs;

        public Collections Collections
        {
            get
            {
                return _collections;
            }
            set
            {
                _collections = value;
                var selectedCollection = _view.SelectedCollection;
                _view.Collections = value;
                if (_collections.Contains(selectedCollection))
                    _view.SelectedCollection = selectedCollection;
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

        private void ViewOnCollectionReorder(object sender, Collections collections, Collection targetCollection, bool placeBefore)
        {
            if (!Settings.Default.DontAskAboutReorderingCollections)
            {
                var result = _userDialogs.YesNoMessageBox($"Reordering collections will rename all loaded collections, proceed?", "Reordering", MessageBoxType.Question,
                        "Don't ask me again");
                Settings.Default.DontAskAboutReorderingCollections = result.doNotAskAgain;
                Settings.Default.Save();
                if (!result.Result)
                    return;
            }

            _model.EmitCollectionEditing(CollectionEditArgs.ReorderCollections(collections, targetCollection, placeBefore));
        }

        private void ViewOnBeatmapsDropped(object sender, Beatmaps args, string collectionName)
        {
            _model.EmitCollectionEditing(CollectionEditArgs.AddBeatmaps(collectionName, args));
        }

        private void ViewOnSelectedCollectionsChanged(object sender, EventArgs eventArgs)
        {
            Collections selectedCollections = new Collections();
            foreach (var collection in _view.SelectedCollections)
            {
                selectedCollections.Add((Collection)collection);
            }
            _model.SelectedCollections = selectedCollections;
        }

        private void _view_RightClick(object sender, Gui.Misc.StringEventArgs e)
        {
            var selectedCollections = _model.SelectedCollections;
            CollectionEditArgs args;
            switch (e.Value)
            {
                case "Delete":
                    if (selectedCollections == null)
                        return;
                    args = CollectionEditArgs.RemoveCollections(selectedCollections);
                    break;
                case "Merge":
                    if (selectedCollections == null)
                        return;
                    args = CollectionEditArgs.MergeCollections(selectedCollections, selectedCollections[0].Name);
                    break;
                case "Intersect":
                    if (selectedCollections == null)
                        return;
                    args = CollectionEditArgs.IntersectCollections(selectedCollections, selectedCollections[0].Name);
                    break;
                case "Inverse":
                    if (selectedCollections == null)
                        return;
                    args = CollectionEditArgs.InverseCollections(selectedCollections, selectedCollections[0].Name);
                    break;
                case "Difference":
                    if (selectedCollections == null)
                        return;
                    args = CollectionEditArgs.DifferenceCollections(selectedCollections, selectedCollections[0].Name);
                    break;
                case "Create":
                    args = CollectionEditArgs.AddCollections(null);
                    break;
                case "Rename":
                    if (_view.SelectedCollection == null)
                        return;
                    args = CollectionEditArgs.RenameCollection(_view.SelectedCollection, null);
                    break;
                case "Duplicate":
                    if (_view.SelectedCollection == null)
                        return;

                    args = CollectionEditArgs.DuplicateCollection(_view.SelectedCollection);
                    break;
                case "Copy":
                    if (selectedCollections == null)
                        return;
                    var tempFolder = Path.Combine(Path.GetTempPath(), "CMcollections");
                    if (Directory.Exists(tempFolder))
                        Directory.Delete(tempFolder, true);

                    Directory.CreateDirectory(tempFolder);
                    var fileName = Helpers.StripInvalidFileNameCharacters(selectedCollections[0].Name, "_");
                    var tempLocation = Path.Combine(tempFolder, $"{fileName}.osdb");
                    Initalizer.OsuFileIo.CollectionLoader.SaveOsdbCollection(selectedCollections, tempLocation);
                    Clipboard.SetFileDropList(new StringCollection { tempLocation });
                    return;
                default:
                    return;
            }

            _model.EmitCollectionEditing(args);
        }

        private void ModelOnCollectionsChanged(object sender, EventArgs eventArgs)
        {
            Collections = _model.GetCollections();
        }
    }
}