using System;
using App.Interfaces;
using App.Models;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
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
            _model.BeatmapChanged+=ModelOnBeatmapChanged;
            _beatmapThumbnailModel = new BeatmapThumbnailModel();
            model.SelectedModsChanged += Model_SelectedModsChanged;
            new BeatmapThumbnailPresenter(_view.BeatmapThumbnailView, _beatmapThumbnailModel);

            MusicControlModel = new MusicControlModel();
            new MusicControlPresenter(_view.MusicControlView, MusicControlModel);
        }

        private void Model_SelectedModsChanged(object sender, EventArgs e)
        {
            _beatmapThumbnailModel.SelectedMods = _model.SelectedMods;
        }

        private void ModelOnBeatmapChanged(object sender, EventArgs eventArgs)
        {
            var map = _model.CurrentBeatmap;
            _beatmapThumbnailModel.SetBeatmap(map);
            MusicControlModel.SetBeatmap(map);
        }
    }
}