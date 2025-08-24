namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Misc;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using Newtonsoft.Json;

public class BeatmapListingPresenter : IBeatmapListingPresenter
{
    private readonly IBeatmapListingView _view;
    private readonly IBeatmapListingModel _model;
    private readonly IUserDialogs _userDialogs;
    private readonly BeatmapListingPresenterSettings _settings;

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

    public BeatmapListingPresenter(IBeatmapListingView view, IBeatmapListingModel model, IUserDialogs userDialogs)
    {
        _view = view;
        _view.SearchTextChanged += ViewOnSearchTextChanged;
        _view.SelectedBeatmapChanged += (s, a) => _model.SelectedBeatmap = _view.SelectedBeatmap;
        _view.SelectedBeatmapsChanged += (s, a) => _model.SelectedBeatmaps = _view.SelectedBeatmaps;
        _view.BeatmapSearchHelpClicked += _view_BeatmapSearchHelpClicked;

        _view.BeatmapsDropped += (s, a) => _model.EmitBeatmapsDropped(s, a);
        _view.BeatmapOperation += (s, a) => _model.EmitBeatmapOperation(a);
        _view.ColumnsToggled += (_, aspectNames) => OnSettingsChanged(visibleAspectNames: aspectNames);
        _view.BeatmapGroupColumnChanged += (_, columnName) => OnSettingsChanged(groupBy: columnName);

        _model = model;
        _userDialogs = userDialogs;
        _model.BeatmapsChanged += _model_BeatmapsChanged;
        _model.FilteringStarted += ModelOnFilteringStarted;
        _model.FilteringFinished += _model_FilteringFinished;
        _view.SetFilter(_model.GetFilter());

        ConvertLegacyBeatmapSettings();
        _settings = JsonConvert.DeserializeObject<BeatmapListingPresenterSettings>(Initalizer.Settings.BeatmapListingPresenterSettings);
        _view.SetVisibleColumns(_settings.VisibleColumns);
        _view.SetGroupColumn(_settings.GroupBy);

        Beatmaps = _model.GetBeatmaps();
    }

    private void OnSettingsChanged(string[] visibleAspectNames = null, string groupBy = null)
    {
        if (visibleAspectNames != null)
        {
            _settings.VisibleColumns = visibleAspectNames;
        }

        if (groupBy != null)
        {
            _settings.GroupBy = groupBy;
        }

        Initalizer.Settings.BeatmapListingPresenterSettings = JsonConvert.SerializeObject(_settings);
    }

    private static void ConvertLegacyBeatmapSettings()
    {
        string[] legacyVisibleColumns = JsonConvert.DeserializeObject<string[]>(Initalizer.Settings.BeatmapColumns);

        if (legacyVisibleColumns.Length == 0)
        {
            return;
        }

        BeatmapListingPresenterSettings settings = new()
        {
            VisibleColumns = legacyVisibleColumns,
            GroupBy = ""
        };

        Initalizer.Settings.BeatmapColumns = "[]";
        Initalizer.Settings.BeatmapListingPresenterSettings = JsonConvert.SerializeObject(settings);
    }

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

    private void _view_BeatmapSearchHelpClicked(object sender, EventArgs e) => ResourceStrings.GeneralHelpDialog(_userDialogs);
}
