using System;
using App.Interfaces;

namespace App.Models
{
    public class InfoTextModel : IInfoTextModel
    {
        public InfoTextModel(IUpdateModel updateModel)
        {
            UpdateModel = updateModel;
        }

        public int BeatmapCount { get; set; }
        public int BeatmapsInCollectionsCount { get; set; }
        public int MissingMapSetsCount { get; set; }
        public int CollectionsCount { get; set; }
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

        public void SetBeatmapCount(int beatmapCount)
        {
            BeatmapCount = beatmapCount;
            OnCountsUpdated();
        }

        public void SetCollectionCount(int collectionsCount, int beatmapsInCollectionsCount)
        {
            CollectionsCount = collectionsCount;
            BeatmapsInCollectionsCount = beatmapsInCollectionsCount;
            OnCountsUpdated();
        }

        public void SetMissingMapSetsCount(int missingBeatmapsCount)
        {
            MissingMapSetsCount = missingBeatmapsCount;
            OnCountsUpdated();
        }

        protected virtual void OnCountsUpdated()
        {
            CountsUpdated?.Invoke(this, EventArgs.Empty);
        }

    }
}