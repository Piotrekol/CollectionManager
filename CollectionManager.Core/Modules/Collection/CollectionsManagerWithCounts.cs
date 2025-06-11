namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.Collections.Generic;

public class CollectionsManagerWithCounts : CollectionsManager
{
    public int MissingMapSetsCount { get; private set; }
    public int UnknownMapCount { get; private set; }
    public int BeatmapsInCollectionsCount { get; private set; }
    public int CollectionsCount => LoadedCollections.Count;

    public CollectionsManagerWithCounts(MapCacher loadedBeatmaps, Dictionary<CollectionEdit, ICollectionEditStrategy> collectionEditStrategies) : base(loadedBeatmaps, collectionEditStrategies)
    {
    }

    public CollectionsManagerWithCounts(MapCacher loadedBeatmaps) : base(loadedBeatmaps)
    {
    }

    protected override void AfterCollectionsEdit()
    {
        int beatmapCount = 0;

        HashSet<string> _missingBeatmapHashes = [];
        HashSet<int> _downloadableMapSetIds = [];

        _missingBeatmapHashes.Clear();
        _downloadableMapSetIds.Clear();
        foreach (IOsuCollection collection in LoadedCollections)
        {
            foreach (Beatmap partialBeatmap in collection.UnknownBeatmaps)
            {
                if (!_missingBeatmapHashes.Contains(partialBeatmap.Md5))
                {
                    _ = _missingBeatmapHashes.Add(partialBeatmap.Md5);
                }
            }

            foreach (Beatmap map in collection.DownloadableBeatmaps)
            {
                if (!_downloadableMapSetIds.Contains(map.MapSetId))
                {
                    _ = _downloadableMapSetIds.Add(map.MapSetId);
                }
            }

            beatmapCount += collection.NumberOfBeatmaps;
        }

        BeatmapsInCollectionsCount = beatmapCount;
        MissingMapSetsCount = _downloadableMapSetIds.Count;
        UnknownMapCount = _missingBeatmapHashes.Count;

        base.AfterCollectionsEdit();
    }
}