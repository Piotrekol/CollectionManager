namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Controls;
using Newtonsoft.Json;
using System;

public sealed class ScoresListingPresenter
{
    private readonly IScoresListingView _view;
    private readonly IScoresListingModel _model;

    public ScoresListingPresenter(IScoresListingView view, IScoresListingModel model)
    {
        _view = view;
        _model = model;
        _view.Scores = _model.Scores;
        _view.ModParser = new();
        _view.ColumnsToggled += OnColumnsToggled;
        _model.ScoresChanged += OnScoresChanged;

        string[] visibleColumns = JsonConvert.DeserializeObject<string[]>(Initalizer.Settings.ScoresColumns);

        if (visibleColumns.Length > 0)
        {
            _view.SetVisibleColumns(visibleColumns);
        }
    }

    private void OnScoresChanged(object sender, EventArgs e) => _view.Scores = _model.Scores;

    private static void OnColumnsToggled(object sender, string[] visibleColumnAspectNames) => Initalizer.Settings.ScoresColumns = JsonConvert.SerializeObject(visibleColumnAspectNames);
}
