﻿namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Models.Controls;

public class CombinedBeatmapPreviewPresenter
{
    private readonly ICombinedBeatmapPreviewView _view;
    private readonly ICombinedBeatmapPreviewModel _model;

    private readonly IBeatmapThumbnailModel _beatmapThumbnailModel;
    public readonly IMusicControlModel MusicControlModel;
    public CombinedBeatmapPreviewPresenter(ICombinedBeatmapPreviewView view, ICombinedBeatmapPreviewModel model)
    {
        _view = view;
        _model = model;
        _model.BeatmapChanged += ModelOnBeatmapChanged;
        _beatmapThumbnailModel = new BeatmapThumbnailModel();
        _ = new BeatmapThumbnailPresenter(_view.BeatmapThumbnailView, _beatmapThumbnailModel);

        MusicControlModel = new MusicControlModel();
        _ = new MusicControlPresenter(_view.MusicControlView, MusicControlModel);
    }

    private void ModelOnBeatmapChanged(object sender, EventArgs eventArgs)
    {
        Beatmap map = _model.CurrentBeatmap;
        _beatmapThumbnailModel.SetBeatmap(map);
        MusicControlModel.SetBeatmap(map);
    }
}