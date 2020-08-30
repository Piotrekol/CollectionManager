using System.Collections.Generic;
using CollectionManager.DataTypes;

namespace CollectionManager.Modules.CollectionsManager
{
    public class CollectionsManagerWithCounts : CollectionsManager
    {
        private readonly HashSet<string> _missingBeatmapHashes = new HashSet<string>();
        private readonly HashSet<int> _downloadableMapSetIds = new HashSet<int>();
        public int MissingMapSetsCount => _downloadableMapSetIds.Count;
        public int UnknownMapCount => _missingBeatmapHashes.Count;
        public int BeatmapsInCollectionsCount { get; private set; }
        public int CollectionsCount => LoadedCollections.Count;

        public CollectionsManagerWithCounts(Beatmaps loadedBeatmaps) : base(loadedBeatmaps)
        {
        }

        protected override void AfterCollectionsEdit()
        {
            var beatmapCount = 0;
            _missingBeatmapHashes.Clear();
            _downloadableMapSetIds.Clear();
            foreach (var collection in LoadedCollections)
            {
                foreach (var partialBeatmap in collection.UnknownBeatmaps)
                {
                    if (!_missingBeatmapHashes.Contains(partialBeatmap.Md5))
                        _missingBeatmapHashes.Add(partialBeatmap.Md5);
                }
                foreach (var map in collection.DownloadableBeatmaps)
                {
                    if (!_downloadableMapSetIds.Contains(map.MapSetId))
                        _downloadableMapSetIds.Add(map.MapSetId);
                }
                beatmapCount += collection.NumberOfBeatmaps;
            }
            BeatmapsInCollectionsCount = beatmapCount;

            base.AfterCollectionsEdit();
        }
    }
}