using System;

namespace App.Interfaces
{
    public interface IInfoTextModel
    {
        void SetBeatmapCount(int beatmapCount);
        void SetCollectionCount(int collectionsCount, int beatmapsInCollectionsCount);
        void SetMissingMapSetsCount(int missingBeatmapsCount);
        int BeatmapCount { get; }
        int BeatmapsInCollectionsCount { get; }
        int MissingMapSetsCount { get; }
        int CollectionsCount { get; }
        IUpdateModel GetUpdater();
        void EmitUpdateTextClicked();
        event EventHandler CountsUpdated;
        event EventHandler UpdateTextClicked;

    }
}