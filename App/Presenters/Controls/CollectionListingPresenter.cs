using System;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class CollectionListingPresenter
    {
        ICollectionListingView _view;

        private Collections _collections;
        private ICollectionListingModel _model;

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
        public CollectionListingPresenter(ICollectionListingView view, ICollectionListingModel model)
        {
            _view = view;
            _view.RightClick += _view_RightClick;
            _view.SelectedCollectionsChanged += ViewOnSelectedCollectionsChanged;
            _view.BeatmapsDropped += ViewOnBeatmapsDropped;
            _model = model;
            _model.CollectionsChanged += ModelOnCollectionsChanged;
            Collections = _model.GetCollections();
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
                    args = CollectionEditArgs.RemoveCollections(selectedCollections);
                    break;
                case "Merge":
                    args = CollectionEditArgs.MergeCollections(selectedCollections, selectedCollections[0].Name);
                    break;
                case "Create":
                    args = CollectionEditArgs.AddCollections(null);
                    break;
                case "Rename":
                    if (_view.SelectedCollection == null)
                        return;
                    args = CollectionEditArgs.RenameCollection(_view.SelectedCollection, null);
                    break;
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