namespace CollectionManagerApp.Presenters.Forms;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Interfaces.Forms;
using CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Presenters.Controls;

public class MainFormPresenter
{
    private readonly IMainFormView _view;
    private readonly IMainFormModel _mainFormModel;
    private readonly ICollectionTextModel _collectionTextModel;
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
        _ = new CombinedListingPresenter(_view.CombinedListingView, CollectionListingModel, BeatmapListingModel, webCollectionProvider, mainFormModel.GetUserDialogs());

        //Beatmap preview stuff (images, beatmap info like ar,cs,stars...)
        _combinedBeatmapPreviewModel = new CombinedBeatmapPreviewModel();
        CombinedBeatmapPreviewPresenter presenter = new(_view.CombinedBeatmapPreviewView, _combinedBeatmapPreviewModel);

        presenter.MusicControlModel.NextMapRequest += (s, a) => _view.CombinedListingView.beatmapListingView.SelectNextOrFirst();

        _collectionTextModel = new CollectionTextModel();
        _ = new CollectionTextPresenter(_view.CollectionTextView, _collectionTextModel);

        //General information (Collections loaded, update check etc.)
        InfoTextModel = infoTextModel;
        _ = new InfoTextPresenter(_view.InfoTextView, InfoTextModel);

    }

    private void CollectionListing_CollectionEditing(object sender, CollectionEditArgs collectionEditArgs) => _mainFormModel.GetCollectionEditor()?.EditCollection(collectionEditArgs);

    private void CollectionListing_SelectedCollectionsChanged(object sender, EventArgs eventArgs)
    {
        OsuCollections collections = CollectionListingModel.SelectedCollections;
        if (collections != null)
        {
            _collectionTextModel.SetCollections(collections);
        }
    }

    private void BeatmapListing_BeatmapsDropped(object sender, Beatmaps args)
    {
        if (CollectionListingModel.SelectedCollections?.Count == 1)
        {
            IOsuCollection collection = CollectionListingModel.SelectedCollections[0];
            CollectionListing_CollectionEditing(sender, CollectionEditArgs.AddBeatmaps(collection.Name, args));
        }
    }

    private void BeatmapListingViewOnSelectedBeatmapChanged(object sender, EventArgs eventArgs) => _combinedBeatmapPreviewModel.SetBeatmap(_view.CombinedListingView.beatmapListingView.SelectedBeatmap);

}