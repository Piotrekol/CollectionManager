using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManager.Modules.FileIO.OsuScoresDb;

namespace CollectionManager
{
    public static class Helpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <rant>For whatever reason all of my replays before around 2015(mind you, I still have my 2011 replays) 
        /// were treated by osu! as invalid, with as it turns out, invalidated whole database.</rant>
        /// <param name="replay"></param>
        private static void OsuOldReplayFix(this IReplay replay)
        {
            replay.Version = 20170503;
        }
        public static void Write(this IReplay replay, OsuBinaryWriter writer, bool dbMode = true)
        {
            if (dbMode)
                replay.OsuOldReplayFix();
            writer.Write((byte)replay.PlayMode);
            writer.Write(replay.Version);
            writer.Write(replay.MapHash);
            writer.Write(replay.PlayerName);
            writer.Write(replay.ReplayHash);
            writer.Write(replay.C300);
            writer.Write(replay.C100);
            writer.Write(replay.C50);
            writer.Write(replay.Geki);
            writer.Write(replay.Katu);
            writer.Write(replay.Miss);
            writer.Write(replay.TotalScore);
            writer.Write(replay.MaxCombo);
            writer.Write(replay.Perfect);
            writer.Write(replay.Mods);
            writer.Write(dbMode ? "" : replay.ReplayData);
            writer.Write(replay.Date.Ticks);
            if (dbMode)
            {
                writer.Write(-1);
            }
            else
            {
                writer.Write(replay.CompressedReplayLength);
                if(replay.CompressedReplayLength>0)
                    writer.Write(replay.ReplayData);
            }
            writer.Write(replay.OnlineScoreId);
        }

        public static BeatmapExtension GetMap(this IReplay replay, MapCacher mapCacher)
        {
            if (mapCacher.LoadedBeatmapsMd5Dict.ContainsKey(replay.MapHash))
                return mapCacher.LoadedBeatmapsMd5Dict[replay.MapHash];
            return null;
        }
    }
}