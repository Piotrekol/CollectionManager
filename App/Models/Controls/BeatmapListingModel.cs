using System;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using App.Interfaces;
using App.Misc;
using CollectionManager.Modules.FileIO;
using Common;
using Gui.Misc;

namespace App.Models
{
    public class BeatmapListingModel : IBeatmapListingModel
    {
        public event EventHandler BeatmapsChanged;
        public event EventHandler FilteringStarted;
        public event EventHandler FilteringFinished;
        public event EventHandler SelectedBeatmapChanged;
        public event EventHandler SelectedBeatmapsChanged;
        public event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
        public event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        private BeatmapListFilter _beatmapListFilter;
        private Beatmaps _beatmapsDataSource;
        private Beatmap _selectedBeatmap;
        public Beatmap SelectedBeatmap
        {
            get
            {
                return _selectedBeatmap;
            }
            set
            {
                _selectedBeatmap = value;
                SelectedBeatmapChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public ICollection CurrentCollection { get; private set; }
        private Beatmaps _selectedBeatmaps;
        public Beatmaps SelectedBeatmaps
        {
            get
            {
                return _selectedBeatmaps;
            }
            set
            {
                _selectedBeatmaps = value;
                SelectedBeatmapsChanged?.Invoke(this, EventArgs.Empty);

            }
        }

        public void EmitBeatmapOperation(BeatmapListingAction args)
        {
            BeatmapOperation?.Invoke(this, args);
        }
        public void EmitBeatmapsDropped(object sender, Beatmaps beatmaps)
        {
            BeatmapsDropped?.Invoke(sender, beatmaps);
        }


        public BeatmapListingModel(Beatmaps dataSource)
        {
            SetBeatmaps(dataSource);
            _beatmapListFilter = new BeatmapListFilter(GetBeatmaps(), Initalizer.OsuFileIo.ScoresDatabase.Scores);
            _beatmapListFilter.FilteringStarted += delegate { OnFilteringStarted(); };
            _beatmapListFilter.FilteringFinished += delegate { OnFilteringFinished(); };
        }

        public Beatmaps GetBeatmaps()
        {
            return _beatmapsDataSource;
        }
        public void SetBeatmaps(Beatmaps beatmaps)
        {
            _beatmapsDataSource = beatmaps;
            OnBeatmapsChanged();
        }

        public void SetCollection(ICollection collection)
        {
            CurrentCollection = collection;
            if (collection == null)
            {
                SetBeatmaps(null);
            }
            else
            {
                var maps = new Beatmaps();
                maps.AddRange(collection.AllBeatmaps());
                SetBeatmaps(maps);
            }
        }

        public void FilterBeatmaps(string text)
        {
            _beatmapListFilter.UpdateSearch(text);
        }

        public IModelFilter GetFilter()
        {
            return _beatmapListFilter;
        }

        protected virtual void OnFilteringStarted()
        {
            FilteringStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFilteringFinished()
        {
            FilteringFinished?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnBeatmapsChanged()
        {
            if (_beatmapsDataSource != null && _beatmapListFilter != null)
            {
                _beatmapListFilter.SetScores(Initalizer.OsuFileIo.ScoresDatabase.Scores);
                _beatmapListFilter.SetBeatmaps(_beatmapsDataSource);
            }

            BeatmapsChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}