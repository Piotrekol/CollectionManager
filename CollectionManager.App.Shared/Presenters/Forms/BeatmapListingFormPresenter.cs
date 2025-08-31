namespace CollectionManager.App.Shared.Presenters.Forms;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Models.Controls;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;

public class BeatmapListingFormPresenter
{
    private readonly IBeatmapListingForm _view;
    private readonly ICombinedBeatmapPreviewModel _combinedBeatmapPreviewModel;
    private readonly IScoresListingModel _scoresListingModel;
    public IBeatmapListingModel BeatmapListingModel { get; private set; }

    public BeatmapListingFormPresenter(IBeatmapListingForm view, IUserDialogs userDialogs)
    {
        _view = view;
        //_view.BeatmapListingView.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
        BeatmapListingModel = new BeatmapListingModel(Initalizer.LoadedBeatmaps);
        BeatmapListingModel.SelectedBeatmapChanged += BeatmapListingView_SelectedBeatmapChanged;
        _ = new BeatmapListingPresenter(_view.BeatmapListingView, BeatmapListingModel, userDialogs, groupBy: string.Empty);

        _combinedBeatmapPreviewModel = new CombinedBeatmapPreviewModel();
        CombinedBeatmapPreviewPresenter presenter = new(_view.CombinedBeatmapPreviewView, _combinedBeatmapPreviewModel);
        presenter.MusicControlModel.NextMapRequest += (s, a) => _view.BeatmapListingView.SelectNextOrFirst();

        _scoresListingModel = new ScoresListingModel();
        _ = new ScoresListingPresenter(_view.ScoresListingView, _scoresListingModel);
    }

    private void BeatmapListingView_SelectedBeatmapChanged(object sender, EventArgs e)
    {
        Beatmap selectedBeatmap = BeatmapListingModel.SelectedBeatmap;
        _combinedBeatmapPreviewModel.SetBeatmap(selectedBeatmap);

        Scores scores = selectedBeatmap is null ? null : Initalizer.OsuFileIo.ScoresDatabase.GetScores(selectedBeatmap);
        _scoresListingModel.Scores = scores;
    }
}