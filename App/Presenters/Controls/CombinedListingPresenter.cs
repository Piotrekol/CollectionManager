using System;
using App.Interfaces;
using CollectionManager.DataTypes;
using CollectionManagerApp.Properties;
using GuiComponents.Interfaces;
using Newtonsoft.Json;

namespace App.Presenters.Controls
{
    public class CombinedListingPresenter
    {

        private readonly ICombinedListingView _view;
        private readonly IBeatmapListingView _beatmapsView;
        private readonly ICollectionListingView _collectionsView;
        public readonly IBeatmapListingModel BeatmapListingModel;
        private readonly IWebCollectionProvider _webCollectionProvider;
        private readonly IUserDialogs _userDialogs;
        private readonly ICollectionListingModel _collectionListingModel;

        public CombinedListingPresenter(ICombinedListingView view, ICollectionListingModel collectionListingModel, IBeatmapListingModel beatmapListingModel, IWebCollectionProvider webCollectionProvider, IUserDialogs userDialogs)
        {
            _view = view;
            _beatmapsView = _view.beatmapListingView;
            _collectionsView = _view.CollectionListingView;

            _collectionListingModel = collectionListingModel;
            BeatmapListingModel = beatmapListingModel;

            _webCollectionProvider = webCollectionProvider;
            _userDialogs = userDialogs;
            new BeatmapListingPresenter(_beatmapsView, BeatmapListingModel);
            new CollectionListingPresenter(_collectionsView, collectionListingModel, userDialogs);

            BeatmapListingModel.SelectedBeatmapsChanged += BeatmapListingModelOnSelectedBeatmapsChanged;
            _collectionsView.SelectedCollectionChanged += CollectionsViewOnSelectedCollectionChanged;
            _collectionsView.SelectedCollectionsChanged += CollectionsViewOnSelectedCollectionsChanged;
            _collectionsView.ColumnsToggled += OnColumnsToggled;

            var visibleColumns = JsonConvert.DeserializeObject<string[]>(Settings.Default.CollectionColumns);

            if (visibleColumns.Length > 0)
            {
                _collectionsView.SetVisibleColumns(visibleColumns);
            }
        }

        private void OnColumnsToggled(object sender, string[] visibleCollumnAspectNames)
        {
            Settings.Default.CollectionColumns = JsonConvert.SerializeObject(visibleCollumnAspectNames);
        }

        private void BeatmapListingModelOnSelectedBeatmapsChanged(object sender, EventArgs e)
        {
            _collectionsView.HighlightedCollections = _collectionListingModel.GetCollectionsForBeatmaps(BeatmapListingModel.SelectedBeatmaps);
        }

        private void CollectionsViewOnSelectedCollectionsChanged(object sender, EventArgs eventArgs)
        {

        }

        private async void CollectionsViewOnSelectedCollectionChanged(object sender, EventArgs eventArgs)
        {
            var collection = _collectionsView.SelectedCollection;
            if (collection is WebCollection wb)
            {
                await wb.Load(_webCollectionProvider);
            }

            BeatmapListingModel.SetCollection(collection);
            _beatmapsView.ClearSelection();
        }
    }
}