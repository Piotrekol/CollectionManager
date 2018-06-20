using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManagerExtensionsDll.Modules.API.osu;
using CollectionManagerExtensionsDll.Modules.CollectionApiGenerator;

namespace CollectionManagerExtensionsDll.Modules.API
{
    public class BeatmapData
    {
        readonly Dictionary<UserTopGenerator.BeatmapModePair, Beatmap> _beatmapCache = new Dictionary<UserTopGenerator.BeatmapModePair, Beatmap>();
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
                    var map = _mapCacher.GetByMapId(mapId);
                    if (map != null)
                        return map;
                }

                var beatmapFromCache = _beatmapCache.FirstOrDefault(s => s.Key.BeatmapId == mapId & s.Key.PlayMode == gamemode).Value;
                if (beatmapFromCache != null)
                    return beatmapFromCache;
            }
            else if (mapHash != null)
            {
                if (_mapCacher != null)
                {
                    var map = _mapCacher.GetByHash(mapHash);
                    if (map != null)
                        return map;
                }

                var beatmapFromCache = _beatmapCache.FirstOrDefault(s => s.Value.Md5 == mapHash & s.Key.PlayMode == gamemode).Value;
                if (beatmapFromCache != null)
                    return beatmapFromCache;
            }

            return null;
        }

        public Beatmap GetBeatmapFromHash(string mapHash, PlayMode gamemode)
        {
            var map = CheckInCache(gamemode, -1, mapHash);
            if (map != null)
                return map;

            Beatmap result;
            int i = 1;
            do
            {
                result = _osuApi.GetBeatmap(mapHash, gamemode);
            } while (result == null && i++ < 3);

            if (result == null)
                return null;

            _beatmapCache.Add(new UserTopGenerator.BeatmapModePair(result.MapId, gamemode), result);
            return result;
        }

        public Beatmap GetBeatmapFromId(int beatmapId, PlayMode gamemode)
        {
            var map = CheckInCache(gamemode, beatmapId);
            if (map != null)
                return map;

            Beatmap result;
            int i = 1;
            do
            {
                result = _osuApi.GetBeatmap(beatmapId, gamemode);
            } while (result == null && i++ < 3);

            if (result == null)
                return null;

            _beatmapCache.Add(new UserTopGenerator.BeatmapModePair(beatmapId, gamemode), result);
            return result;
        }
    }
}