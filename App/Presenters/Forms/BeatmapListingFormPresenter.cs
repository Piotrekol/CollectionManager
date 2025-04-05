namespace CollectionManagerApp.Presenters.Forms;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Presenters.Controls;

public class BeatmapListingFormPresenter
{
    private readonly IBeatmapListingForm _view;
    private readonly ICombinedBeatmapPreviewModel _combinedBeatmapPreviewModel;
    public readonly IBeatmapListingModel BeatmapListingModel;
    public BeatmapListingFormPresenter(IBeatmapListingForm view)
    {
        _view = view;
        //_view.BeatmapListingView.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
        BeatmapListingModel = new BeatmapListingModel(Initalizer.LoadedBeatmaps);
        BeatmapListingModel.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
        _ = new BeatmapListingPresenter(_view.BeatmapListingView, BeatmapListingModel);

        _combinedBeatmapPreviewModel = new CombinedBeatmapPreviewModel();
        CombinedBeatmapPreviewPresenter presenter = new(_view.CombinedBeatmapPreviewView, _combinedBeatmapPreviewModel);
        presenter.MusicControlModel.NextMapRequest += (s, a) => _view.BeatmapListingView.SelectNextOrFirst();
    }

    private void BeatmapListingView_SelectedBeatmapChanged(object sender, EventArgs e) => _combinedBeatmapPreviewModel.SetBeatmap(BeatmapListingModel.SelectedBeatmap);
}