using System;
using CollectionManager.DataTypes;
using App.Interfaces;
using App.Misc;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class BeatmapListingPresenter
    {
        readonly IBeatmapListingView _view;
        readonly IBeatmapListingModel _model;

        public BeatmapListingPresenter(IBeatmapListingView view, IBeatmapListingModel model)
        {
            _view = view;
            _view.SearchTextChanged+=ViewOnSearchTextChanged;
            _view.SelectedBeatmapChanged += (s, a) => _model.SelectedBeatmap = _view.SelectedBeatmap;
            _view.SelectedBeatmapsChanged += (s, a) => _model.SelectedBeatmaps = _view.SelectedBeatmaps;
            
            _view.BeatmapsDropped += (s, a) => _model.EmitBeatmapsDropped(s, a);
            _view.BeatmapOperation += (s, a) => _model.EmitBeatmapOperation(a);

            _model = model;
            _model.BeatmapsChanged += (_, _) => RefreshBeatmapsInViewFromModel();
            _model.FilteringStarted+=ModelOnFilteringStarted;
            _model.FilteringFinished += _model_FilteringFinished;
            _view.SetFilter(_model.GetFilter());
            RefreshBeatmapsInViewFromModel();
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

        private void ModelOnFilteringStarted(object sender, EventArgs eventArgs)
        {
            _view.FilteringStarted();
        }

        private void ViewOnSearchTextChanged(object sender, EventArgs eventArgs)
        {
            _model.FilterBeatmaps(_view.SearchText);
        }

        private void RefreshBeatmapsInViewFromModel()
        {
            _view.SetBeatmaps(_model.GetBeatmaps());
            _view.ClearCustomFieldDefinitions();
            var curCol = _model.CurrentCollection;
            if(curCol != null && curCol.CustomFieldDefinitions != null)
            {
                _view.SetCustomFieldDefinitions(curCol.CustomFieldDefinitions);
            }
        }


    }
}