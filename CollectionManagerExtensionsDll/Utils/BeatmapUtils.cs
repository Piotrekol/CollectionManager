using CollectionManager.DataTypes;
using CollectionManager.Extensions;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManagerExtensionsDll.Utils
{
    public static class BeatmapUtils
    {
        public static string OsuSongsDirectory = "";

        public static Dictionary<int, Beatmaps> GetMapSets(this ICollection collection, BeatmapListType beatmapListType)
        {
            var mapSets = new Dictionary<int, Beatmaps>();
            switch (beatmapListType)
            {
                case BeatmapListType.All:
                    mapSets = collection.GetBeatmapSets();
                    break;
                case BeatmapListType.NotKnown:
                    mapSets = GetBeatmapSets(collection.NotKnownBeatmaps());
                    break;
                case BeatmapListType.Known:
                    mapSets = GetBeatmapSets(collection.KnownBeatmaps);
                    break;
                case BeatmapListType.Downloadable:
                    mapSets = GetBeatmapSets(collection.DownloadableBeatmaps);
                    break;
            }
            return mapSets;
        }

        public static Dictionary<int, Beatmaps> GetBeatmapSets(this IEnumerable<Beatmap> beatmaps)
            => beatmaps
                .GroupBy(map => map.MapSetId < 1 ? -1 : map.MapSetId)
                .ToDictionary(
                    group => group.Key,
                    group => new Beatmaps(group)
                );

        public static HashSet<int> GetUniqueMapSetIds(this Beatmaps beatmaps, bool filterInvalidIds = true)
        {
            var mapIds = new HashSet<int>();
            if (beatmaps?.Count > 0)
                foreach (var beatmap in beatmaps)
                {
                    if (!mapIds.Contains(beatmap.MapSetId))
                    {
                        if (filterInvalidIds && beatmap.MapSetId < 2)
                            continue;
                        mapIds.Add(beatmap.MapSetId);
                    }
                }
            return mapIds;
        }

        public static string GetImageLocation(this Beatmap beatmap)
        {
            if (beatmap is BeatmapExtension mapExtension)
            {
                if (mapExtension.LocalBeatmapMissing)
                    return string.Empty;
            }

            if (beatmap is LazerBeatmap lazerBeatmap)
            {
                if (string.IsNullOrEmpty(lazerBeatmap.BackgroundRelativeFilePath))
                {
                    return string.Empty;
                }

                return Path.Combine(OsuSongsDirectory, lazerBeatmap.BackgroundRelativeFilePath);
            }

            var osuFileLocation = beatmap.FullOsuFileLocation();
            if (!File.Exists(osuFileLocation))
                return string.Empty;

            var imageLocation = string.Empty;
            using (StreamReader file = new StreamReader(osuFileLocation))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToLower().Contains(".jpg") || line.ToLower().Contains(".png"))
                    {
                        var splited = line.Split(',');
                        imageLocation = Path.Combine(beatmap.BeatmapDirectory(), splited[2].Trim('"'));
                        if (!File.Exists(imageLocation))
                        {
                            return string.Empty;
                        }
                        break;
                    }
                }
            }
            return imageLocation;
        }
        public static string BeatmapDirectory(this Beatmap beatmap, string osuSongsDirectory = null)
        {
            return Path.Combine(osuSongsDirectory ?? OsuSongsDirectory, beatmap.Dir);
        }
        public static string FullOsuFileLocation(this Beatmap beatmap)
        {
            return Path.Combine(beatmap.BeatmapDirectory(), beatmap.OsuFileName);
        }

        public static string FullAudioFileLocation(this Beatmap beatmap)
        {
            if (beatmap is BeatmapExtension mapExtension)
            {
                if (mapExtension.LocalBeatmapMissing)
                    return string.Empty;
            }

            if (beatmap is LazerBeatmap lazerBeatmap)
            {
                return Path.Combine(OsuSongsDirectory, lazerBeatmap.AudioRelativeFilePath);
            }

            return Path.Combine(beatmap.BeatmapDirectory(), beatmap.Mp3Name);
        }

        public static string OszFileName(this Beatmap beatmap)
        {
            var filename = beatmap.MapSetId + " " + beatmap.ArtistRoman + " - " + beatmap.TitleRoman;
            return filename.StripInvalidFileNameCharacters("_") + ".osz";
        }
    }
}
