using System.Collections;
using System.Collections.Generic;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.DataTypes
{
    public interface ICollection
    {
        /// <summary>
        /// Contains all beatmap hashes contained in this collection
        /// </summary>
        IReadOnlyCollection<string> BeatmapHashes { get; }

        /// <summary>
        /// Contains beatmaps that did not find a match in LoadedMaps
        /// nor had additional data(MapSetId)
        /// </summary>
        Beatmaps UnknownBeatmaps { get; }

        /// <summary>
        /// Contains beatmaps that did not find a match in LoadedMaps
        /// but contain enough information(MapSetId) to be able to issue new download
        /// </summary>
        /// <remarks>.osdb files contain this data since v2</remarks>
        Beatmaps DownloadableBeatmaps { get; }

        /// <summary>
        /// Contains beatmap with data from LoadedMaps
        /// </summary>
        Beatmaps KnownBeatmaps { get; }

        /// <summary>
        /// Total number of beatmaps contained in this collection
        /// </summary>
        int NumberOfBeatmaps { get; }

        int NumberOfMissingBeatmaps { get; }

        /// <summary>
        /// Username of last person editing this collection
        /// </summary>
        string LastEditorUsername { get; set; }

        /// <summary>
        /// Collection name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Osu!Stats collection Id
        /// </summary>
        int OnlineId { get; set; }

        int Id { get; set; }

        IReadOnlyCollection<CustomFieldDefinition> CustomFieldDefinitions { get; }

        void SetLoadedMaps(MapCacher instance);
        IEnumerable<BeatmapExtension> AllBeatmaps();
        IEnumerable<BeatmapExtension> NotKnownBeatmaps();
        void AddBeatmap(Beatmap map);
        void AddBeatmap(BeatmapExtension map);
        void AddBeatmapByHash(string hash);
        void AddBeatmapByMapId(int mapId);
        void ReplaceBeatmap(string hash, Beatmap newBeatmap);
        void ReplaceBeatmap(int mapId, Beatmap newBeatmap);
        bool RemoveBeatmap(string hash);
        IEnumerator GetEnumerator();
    }
}