using System;
using CollectionManager.DataTypes;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class BeatmapListingPresenter: IBeatmapListingPresenter
    {
        readonly IBeatmapListingView _view;
        readonly IBeatmapListingModel _model;

        private Beatmaps _beatmaps;

        public Beatmaps Beatmaps
        {
            get
            {
                return _beatmaps;
            }
            set
            {
                _beatmaps = value;
                _view.SetBeatmaps(value);
            }
        }
        public BeatmapListingPresenter(IBeatmapListingView view, IBeatmapListingModel model)
        {
            _view = view;
            _view.SearchTextChanged+=ViewOnSearchTextChanged;
            _view.SelectedBeatmapChanged += (s, a) => _model.SelectedBeatmap = _view.SelectedBeatmap;
            _view.SelectedBeatmapsChanged += (s, a) => _model.SelectedBeatmaps = _view.SelectedBeatmaps;
            _view.DeleteBeatmapsFromCollection+= (s, a) => _model.EmitDeleteBeatmapsFromCollection();
            _view.DownloadBeatmaps += (s, a) => _model.EmitDownloadBeatmaps();
            _view.DownloadBeatmapsManaged += (s, a) => _model.EmitDownloadBeatmapsManaged();
            _view.OpenBeatmapPages += (s, a) => _model.EmitOpenBeatmapPages();
            _view.BeatmapsDropped += (s, a) => _model.EmitBeatmapsDropped(s, a);

            _model = model;
            _model.BeatmapsChanged += _model_BeatmapsChanged;
            _model.FilteringStarted+=ModelOnFilteringStarted;
            _model.FilteringFinished += _model_FilteringFinished;
            _view.SetFilter(_model.GetFilter());
            Beatmaps = _model.GetBeatmaps();
        }
        
        private void _model_FilteringFinished(object sender, EventArgs e)
        {
            _view.FilteringFinished();
        }

        private void ModelOnFilteringStarted(object sender, EventArgs eventArgs)
        {
            _view.FilteringStarted();
        }

        private void ViewOnSearchTextChanged(object sender, EventArgs eventArgs)
        {
            _model.FilterBeatmaps(_view.SearchText);
        }

        private void _model_BeatmapsChanged(object sender, System.EventArgs e)
        {
            Beatmaps = _model.GetBeatmaps();
        }


    }
}