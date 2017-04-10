using System;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class CombinedListingPresenter
    {
        private readonly ICombinedListingView _view;
        private readonly IBeatmapListingView _beatmapsView;
        private readonly ICollectionListingView _collectionsView;
        public readonly IBeatmapListingModel BeatmapListingModel;
        public CombinedListingPresenter(ICombinedListingView view, ICollectionListingModel collectionListingModel,IBeatmapListingModel beatmapListingModel)
        {
            _view = view;
            _beatmapsView = _view.beatmapListingView;
            _collectionsView = _view.CollectionListingView;

            BeatmapListingModel = beatmapListingModel;
            new BeatmapListingPresenter(_beatmapsView, BeatmapListingModel);
            new CollectionListingPresenter(_collectionsView, collectionListingModel);

            _collectionsView.SelectedCollectionChanged += CollectionsViewOnSelectedCollectionChanged;
            _collectionsView.SelectedCollectionsChanged += CollectionsViewOnSelectedCollectionsChanged;
        }

        private void CollectionsViewOnSelectedCollectionsChanged(object sender, EventArgs eventArgs)
        {

        }

        private void CollectionsViewOnSelectedCollectionChanged(object sender, EventArgs eventArgs)
        {
            var collection = _collectionsView.SelectedCollection;
            BeatmapListingModel.SetCollection(collection);
            _beatmapsView.ClearSelection();
        }
    }
}