using App.Presenters.Controls;
using App.Interfaces;
using App.Models;
using GuiComponents.Interfaces;

namespace App.Presenters.Forms
{
    public class BeatmapListingFormPresenter
    {
        private readonly IBeatmapListingForm _view;
        private ICombinedBeatmapPreviewModel _combinedBeatmapPreviewModel;
        public readonly IBeatmapListingModel BeatmapListingModel;
        public BeatmapListingFormPresenter(IBeatmapListingForm view)
        {
            _view = view;
            //_view.BeatmapListingView.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
            BeatmapListingModel = new BeatmapListingModel(Initalizer.LoadedBeatmaps);
            BeatmapListingModel.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
            new BeatmapListingPresenter(_view.BeatmapListingView, BeatmapListingModel);

            _combinedBeatmapPreviewModel = new CombinedBeatmapPreviewModel();
            var presenter =new CombinedBeatmapPreviewPresenter(_view.CombinedBeatmapPreviewView, _combinedBeatmapPreviewModel);
            presenter.MusicControlModel.NextMapRequest += (s, a) => _view.BeatmapListingView.SelectNextOrFirst();
        }

        private void BeatmapListingView_SelectedBeatmapChanged(object sender, System.EventArgs e)
        {
            _combinedBeatmapPreviewModel.SetBeatmap(BeatmapListingModel.SelectedBeatmap);
        }
    }
}