namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Types;
using System.Collections.Generic;

public class CollectionsManagerWithCounts : CollectionsManager
{
    private readonly HashSet<string> _missingBeatmapHashes = [];
    private readonly HashSet<int> _downloadableMapSetIds = [];
    public int MissingMapSetsCount => _downloadableMapSetIds.Count;
    public int UnknownMapCount => _missingBeatmapHashes.Count;
    public int BeatmapsInCollectionsCount { get; private set; }
    public int CollectionsCount => LoadedCollections.Count;

    public CollectionsManagerWithCounts(Beatmaps loadedBeatmaps) : base(loadedBeatmaps)
    {
    }

    protected override void AfterCollectionsEdit()
    {
        int beatmapCount = 0;
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

        base.AfterCollectionsEdit();
    }
}