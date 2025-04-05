namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Misc;
using CollectionManagerApp.Properties;
using Newtonsoft.Json;

public class BeatmapListingPresenter : IBeatmapListingPresenter
{
    private readonly IBeatmapListingView _view;
    private readonly IBeatmapListingModel _model;

    private Beatmaps _beatmaps;

    public Beatmaps Beatmaps
    {
        get => _beatmaps;
        set
        {
            _beatmaps = value;
            _view.SetBeatmaps(value);
        }
    }
    public BeatmapListingPresenter(IBeatmapListingView view, IBeatmapListingModel model)
    {
        _view = view;
        _view.SearchTextChanged += ViewOnSearchTextChanged;
        _view.SelectedBeatmapChanged += (s, a) => _model.SelectedBeatmap = _view.SelectedBeatmap;
        _view.SelectedBeatmapsChanged += (s, a) => _model.SelectedBeatmaps = _view.SelectedBeatmaps;

        _view.BeatmapsDropped += (s, a) => _model.EmitBeatmapsDropped(s, a);
        _view.BeatmapOperation += (s, a) => _model.EmitBeatmapOperation(a);
        _view.ColumnsToggled += (s, a) => OnColumnsToggled(a);

        _model = model;
        _model.BeatmapsChanged += _model_BeatmapsChanged;
        _model.FilteringStarted += ModelOnFilteringStarted;
        _model.FilteringFinished += _model_FilteringFinished;
        _view.SetFilter(_model.GetFilter());

        string[] visibleColumns = JsonConvert.DeserializeObject<string[]>(Settings.Default.BeatmapColumns);

        if (visibleColumns.Length > 0)
        {
            _view.SetVisibleColumns(visibleColumns);
        }

        Beatmaps = _model.GetBeatmaps();
    }

    private static void OnColumnsToggled(string[] visibleColumnAspectNames) => Settings.Default.BeatmapColumns = JsonConvert.SerializeObject(visibleColumnAspectNames);

    private void _model_FilteringFinished(object sender, EventArgs e)
    {
        if (_model.GetFilter() is BeatmapListFilter filter)
        {
            _view.SetCurrentPlayMode(filter.CurrentPlayMode);
            _view.SetCurrentMods(filter.CurrentMods);
        }

        _view.FilteringFinished();
    }

    private void ModelOnFilteringStarted(object sender, EventArgs eventArgs) => _view.FilteringStarted();

    private void ViewOnSearchTextChanged(object sender, EventArgs eventArgs) => _model.FilterBeatmaps(_view.SearchText);

    private void _model_BeatmapsChanged(object sender, EventArgs e) => Beatmaps = _model.GetBeatmaps();

}