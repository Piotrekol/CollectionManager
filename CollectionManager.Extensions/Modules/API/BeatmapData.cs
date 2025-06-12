namespace CollectionManager.Extensions.Modules.API;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.API.osu;
using CollectionManager.Extensions.Modules.CollectionApiGenerator;
using System.Collections.Generic;
using System.Linq;

public class BeatmapData
{
    private readonly Dictionary<UserTopGenerator.BeatmapModePair, Beatmap> _beatmapCache = [];
    private readonly OsuApi _osuApi;
    private readonly IMapDataManager _mapCacher;
    public BeatmapData(string apiKey, IMapDataManager mapCacher)
    {
        _mapCacher = mapCacher;
        _osuApi = new OsuApi(apiKey);
    }

    private Beatmap CheckInCache(PlayMode gamemode = PlayMode.Osu, int mapId = -1, string mapHash = null)
    {
        if (mapId >= 0)
        {
            if (_mapCacher != null)
            {
                Beatmap map = _mapCacher.GetByMapId(mapId);
                if (map != null)
                {
                    return map;
                }
            }

            Beatmap beatmapFromCache = _beatmapCache.FirstOrDefault(s => s.Key.BeatmapId == mapId & s.Key.PlayMode == gamemode).Value;
            if (beatmapFromCache != null)
            {
                return beatmapFromCache;
            }
        }
        else if (mapHash != null)
        {
            if (_mapCacher != null)
            {
                Beatmap map = _mapCacher.GetByHash(mapHash);
                if (map != null)
                {
                    return map;
                }
            }

            Beatmap beatmapFromCache = _beatmapCache.FirstOrDefault(s => s.Value.Md5 == mapHash & s.Key.PlayMode == gamemode).Value;
            if (beatmapFromCache != null)
            {
                return beatmapFromCache;
            }
        }

        return null;
    }

    public Beatmap GetBeatmapFromHash(string mapHash, PlayMode? gamemode)
    {
        Beatmap map = CheckInCache(gamemode ?? PlayMode.Osu, -1, mapHash);
        if (map != null)
        {
            return map;
        }

        Beatmap result;
        int i = 1;
        do
        {
            result = _osuApi.GetBeatmap(mapHash, gamemode);
        } while (result == null && i++ < 3);

        if (result == null)
        {
            return null;
        }

        _beatmapCache.Add(new UserTopGenerator.BeatmapModePair(result.MapId, result.PlayMode), result);
        return result;
    }

    public Beatmap GetBeatmapFromId(int beatmapId, PlayMode? gamemode)
    {
        Beatmap map = CheckInCache(gamemode ?? PlayMode.Osu, beatmapId);
        if (map != null)
        {
            return map;
        }

        Beatmap result;
        int i = 1;
        do
        {
            result = _osuApi.GetBeatmap(beatmapId, gamemode);
        } while (result == null && i++ < 3);

        if (result == null)
        {
            return null;
        }

        _beatmapCache.Add(new UserTopGenerator.BeatmapModePair(beatmapId, result.PlayMode), result);
        return result;
    }
}