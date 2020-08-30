using System;
using App.Interfaces;

namespace App.Models
{
    public class InfoTextModel : IInfoTextModel
    {
        private int _beatmapCount;
        private int _beatmapsInCollectionsCount;
        private int _missingMapSetsCount;
        private int _collectionsCount;
        private int _unknownMapCount;

        public InfoTextModel(IUpdateModel updateModel)
        {
            UpdateModel = updateModel;
        }

        public int BeatmapCount
        {
            get => _beatmapCount;
            set
            {
                _beatmapCount = value;
                OnCountsUpdated();
            }
        }

        public int BeatmapsInCollectionsCount
        {
            get => _beatmapsInCollectionsCount;
            set
            {
                _beatmapsInCollectionsCount = value;
                OnCountsUpdated();
            }
        }

        public int MissingMapSetsCount
        {
            get => _missingMapSetsCount;
            set
            {
                _missingMapSetsCount = value;
                OnCountsUpdated();
            }
        }

        public int CollectionsCount
        {
            get => _collectionsCount;
            set
            {
                _collectionsCount = value;
                OnCountsUpdated();
            }
        }

        public int UnknownMapCount
        {
            get => _unknownMapCount;
            set
            {
                _unknownMapCount = value;
                OnCountsUpdated();
            }
        }

        private IUpdateModel UpdateModel { get; }
        public IUpdateModel GetUpdater()
        {
            return UpdateModel;
        }

        public void EmitUpdateTextClicked()
        {
            UpdateTextClicked?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CountsUpdated;
        public event EventHandler UpdateTextClicked;

        protected virtual void OnCountsUpdated()
        {
            CountsUpdated?.Invoke(this, EventArgs.Empty);
        }

    }
}