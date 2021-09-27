using System;
using App.Presenters.Controls;
using CollectionManager.Enums;
using App.Interfaces;
using App.Interfaces.Forms;
using App.Misc;
using App.Models;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using GuiComponents.Interfaces;

namespace App.Presenters.Forms
{
    public class MainFormPresenter
    {
        private IMainFormView _view;
        private readonly IMainFormModel _mainFormModel;
        private ICollectionTextModel _collectionTextModel;
        private readonly ICombinedBeatmapPreviewModel _combinedBeatmapPreviewModel;
        public IInfoTextModel InfoTextModel;
        public readonly IBeatmapListingModel BeatmapListingModel;
        public readonly ICollectionListingModel CollectionListingModel;

        public MainFormPresenter(IMainFormView view, IMainFormModel mainFormModel, IInfoTextModel infoTextModel, IWebCollectionProvider webCollectionProvider)
        {
            _view = view;
            _mainFormModel = mainFormModel;

            //Combined listing (Collections+Beatmaps)
            BeatmapListingModel = new BeatmapListingModel(null);
            BeatmapListingModel.BeatmapsDropped += BeatmapListing_BeatmapsDropped;
            BeatmapListingModel.SelectedBeatmapChanged += BeatmapListingViewOnSelectedBeatmapChanged;
            CollectionListingModel = new CollectionListingModel(Initalizer.LoadedCollections, _mainFormModel.GetCollectionEditor());
            CollectionListingModel.CollectionEditing += CollectionListing_CollectionEditing;
            CollectionListingModel.SelectedCollectionsChanged += CollectionListing_SelectedCollectionsChanged;
            new CombinedListingPresenter(_view.CombinedListingView, CollectionListingModel, BeatmapListingModel, webCollectionProvider);

            //Beatmap preview stuff (images, beatmap info like ar,cs,stars...)
            _combinedBeatmapPreviewModel = new CombinedBeatmapPreviewModel();
            var presenter = new CombinedBeatmapPreviewPresenter(_view.CombinedBeatmapPreviewView, _combinedBeatmapPreviewModel);

            presenter.MusicControlModel.NextMapRequest += (s, a) => { _view.CombinedListingView.beatmapListingView.SelectNextOrFirst(); };

            _collectionTextModel = new CollectionTextModel();
            new CollectionTextPresenter(_view.CollectionTextView, _collectionTextModel);

            //General information (Collections loaded, update check etc.)
            InfoTextModel = infoTextModel;
            new InfoTextPresenter(_view.InfoTextView, InfoTextModel);

        }

        private void CollectionListing_CollectionEditing(object sender, CollectionEditArgs collectionEditArgs)
        {
            _mainFormModel.GetCollectionEditor()?.EditCollection(collectionEditArgs);
        }

        private void CollectionListing_SelectedCollectionsChanged(object sender, EventArgs eventArgs)
        {
            var collections = CollectionListingModel.SelectedCollections;
            if (collections != null)
            {
                _collectionTextModel.SetCollections(collections);
            }
        }


        private void BeatmapListing_BeatmapsDropped(object sender, Beatmaps args)
        {
            if (CollectionListingModel.SelectedCollections?.Count == 1)
            {
                var collection = CollectionListingModel.SelectedCollections[0];
                CollectionListing_CollectionEditing(sender, CollectionEditArgs.AddBeatmaps(collection.Name, args));
            }
        }


        private void BeatmapListingViewOnSelectedBeatmapChanged(object sender, EventArgs eventArgs)
        {
            _combinedBeatmapPreviewModel.SetBeatmap(_view.CombinedListingView.beatmapListingView.SelectedBeatmap);
        }

    }
}