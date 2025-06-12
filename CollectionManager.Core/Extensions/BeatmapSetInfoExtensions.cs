namespace CollectionManager.Core.Extensions;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using System.Collections.Generic;
using System.Linq;

internal static class BeatmapSetInfoExtensions
{
    public static IEnumerable<LazerBeatmap> ToLazerBeatmaps(this BeatmapSetInfo beatmapSetInfo, IScoreDataManager scoreDataManager)
    {
        LazerFile[] setFiles = beatmapSetInfo.Files.Select(namedFile => namedFile.ToLazerFile()).ToArray();

        return beatmapSetInfo.Beatmaps.Select(beatmap => beatmap.ToLazerBeatmap(scoreDataManager, setFiles));
    }
}
