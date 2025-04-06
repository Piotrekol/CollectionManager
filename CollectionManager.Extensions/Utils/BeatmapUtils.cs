namespace CollectionManager.Extensions.Utils;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class BeatmapUtils
{
    public static string OsuSongsDirectory = "";

    public static Dictionary<int, Beatmaps> GetMapSets(this IOsuCollection collection, BeatmapListType beatmapListType)
    {
        Dictionary<int, Beatmaps> mapSets = [];
        switch (beatmapListType)
        {
            case BeatmapListType.All:
                mapSets = collection.GetBeatmapSets();
                break;
            case BeatmapListType.NotKnown:
                mapSets = collection.NotKnownBeatmaps().GetBeatmapSets();
                break;
            case BeatmapListType.Known:
                mapSets = collection.KnownBeatmaps.GetBeatmapSets();
                break;
            case BeatmapListType.Downloadable:
                mapSets = collection.DownloadableBeatmaps.GetBeatmapSets();
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
        HashSet<int> mapIds = [];
        if (beatmaps?.Count > 0)
        {
            foreach (Beatmap beatmap in beatmaps)
            {
                if (!mapIds.Contains(beatmap.MapSetId))
                {
                    if (filterInvalidIds && beatmap.MapSetId < 2)
                    {
                        continue;
                    }

                    _ = mapIds.Add(beatmap.MapSetId);
                }
            }
        }

        return mapIds;
    }

    public static string GetImageLocation(this Beatmap beatmap)
    {
        if (beatmap is BeatmapExtension mapExtension)
        {
            if (mapExtension.LocalBeatmapMissing)
            {
                return string.Empty;
            }
        }

        if (beatmap is LazerBeatmap lazerBeatmap)
        {
            if (string.IsNullOrEmpty(lazerBeatmap.BackgroundRelativeFilePath))
            {
                return string.Empty;
            }

            return Path.Combine(OsuSongsDirectory, lazerBeatmap.BackgroundRelativeFilePath);
        }

        string osuFileLocation = beatmap.FullOsuFileLocation();
        if (!File.Exists(osuFileLocation))
        {
            return string.Empty;
        }

        string imageLocation = string.Empty;
        using (StreamReader file = new(osuFileLocation))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(".jpg", StringComparison.CurrentCultureIgnoreCase) || line.Contains(".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    string[] splited = line.Split(',');
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
    public static string BeatmapDirectory(this Beatmap beatmap, string osuSongsDirectory = null) => Path.Combine(osuSongsDirectory ?? OsuSongsDirectory, beatmap.Dir);
    public static string FullOsuFileLocation(this Beatmap beatmap) => Path.Combine(beatmap.BeatmapDirectory(), beatmap.OsuFileName);

    public static string FullAudioFileLocation(this Beatmap beatmap)
    {
        if (beatmap is BeatmapExtension mapExtension)
        {
            if (mapExtension.LocalBeatmapMissing)
            {
                return string.Empty;
            }
        }

        if (beatmap is LazerBeatmap lazerBeatmap)
        {
            return Path.Combine(OsuSongsDirectory, lazerBeatmap.AudioRelativeFilePath);
        }

        return Path.Combine(beatmap.BeatmapDirectory(), beatmap.Mp3Name);
    }

    public static string OszFileName(this Beatmap beatmap)
    {
        string filename = beatmap.MapSetId + " " + beatmap.ArtistRoman + " - " + beatmap.TitleRoman;
        return filename.StripInvalidFileNameCharacters("_") + ".osz";
    }
}
