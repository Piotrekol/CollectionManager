using System.Collections.Generic;
using System.IO;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes;

namespace CollectionManagerExtensionsDll.Utils
{
    public static class BeatmapUtils
    {
        internal static Dictionary<int, Beatmaps> GetMapSets(this ICollection collection, BeatmapListType beatmapListType)
        {
            var mapSets = new Dictionary<int, Beatmaps>();
            switch (beatmapListType)
            {
                case BeatmapListType.All:
                    mapSets = collection.GetBeatmapSets();
                    break;
                case BeatmapListType.NotKnown:
                    mapSets = CollectionUtils.GetBeatmapSets(collection.NotKnownBeatmaps());
                    break;
                case BeatmapListType.Known:
                    mapSets = CollectionUtils.GetBeatmapSets(collection.KnownBeatmaps);
                    break;
                case BeatmapListType.Downloadable:
                    mapSets = CollectionUtils.GetBeatmapSets(collection.DownloadableBeatmaps);
                    break;
            }
            return mapSets;
        }

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

        public static string OsuSongsDirectory = "";
        public static string GetImageLocation(this Beatmap beatmap)
        {
            if (beatmap.GetType().IsAssignableFrom(typeof(BeatmapExtension)))
            {
                if (((BeatmapExtension)beatmap).LocalBeatmapMissing)
                    return string.Empty;
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
        public static string BeatmapDirectory(this Beatmap beatmap)
        {
            return Path.Combine(OsuSongsDirectory, beatmap.Dir);
        }
        public static string FullOsuFileLocation(this Beatmap beatmap)
        {
            return Path.Combine(beatmap.BeatmapDirectory(), beatmap.OsuFileName);
        }

        public static string FullAudioFileLocation(this Beatmap beatmap)
        {
            var isSubclassed = beatmap.GetType().IsAssignableFrom(typeof(BeatmapExtension));
            if (isSubclassed)
            {
                if (((BeatmapExtension)beatmap).LocalBeatmapMissing)
                    return string.Empty;
            }
           
            return Path.Combine(beatmap.BeatmapDirectory(), beatmap.Mp3Name);

        }
    }
}
