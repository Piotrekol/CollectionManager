﻿using System;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using CollectionManager.Modules.FileIO.OsuScoresDb;

namespace CollectionManager.DataTypes
{
    public class Replay : IReplay, ICloneable
    {
        public PlayMode PlayMode { get; set; }
        public int Version { get; set; }
        public string MapHash { get; set; }
        public string PlayerName { get; set; }
        public string ReplayHash { get; set; }
        public int C300 { get; set; }
        public int C100 { get; set; }
        public int C50 { get; set; }
        public int Geki { get; set; }
        public int Katu { get; set; }
        public int Miss { get; set; }
        public long TotalScore { get; set; }
        public int MaxCombo { get; set; }
        public bool Perfect { get; set; }
        public int Mods { get; set; }
        public double AdditionalMods { get; set; }
        public virtual string ReplayData { get; set; }
        public DateTimeOffset Date { get; set; }
        public long DateTicks { get; set; }
        public int CompressedReplayLength { get; set; }
        public byte[] CompressedReplay { get; set; }
        public long OnlineScoreId { get; set; } = -1;


        public static IReplay Read(OsuBinaryReader reader, IReplay outobj = null, bool minimalLoad = true, int? version = null)
        {
            if (outobj == null)
                outobj = new Replay();
            outobj.PlayMode = (PlayMode)reader.ReadByte();
            outobj.Version = reader.ReadInt32();
            outobj.MapHash = reader.ReadString();
            outobj.PlayerName = reader.ReadString();
            outobj.ReplayHash = reader.ReadString();
            outobj.C300 = reader.ReadInt16();
            outobj.C100 = reader.ReadInt16();
            outobj.C50 = reader.ReadInt16();
            outobj.Geki = reader.ReadInt16();
            outobj.Katu = reader.ReadInt16();
            outobj.Miss = reader.ReadInt16();
            outobj.TotalScore = reader.ReadInt32();
            outobj.MaxCombo = reader.ReadInt16();
            outobj.Perfect = reader.ReadBoolean();
            outobj.Mods = reader.ReadInt32();
            outobj.ReplayData = reader.ReadString();
            outobj.DateTicks = reader.ReadInt64();
            outobj.Date = GetDate(outobj.DateTicks);
            outobj.CompressedReplayLength = reader.ReadInt32();
            if (outobj.CompressedReplayLength > 0)
            {
                if (minimalLoad)
                    reader.ReadBytes(outobj.CompressedReplayLength);
                else
                    outobj.CompressedReplay = reader.ReadBytes(outobj.CompressedReplayLength);
            }

            version = version ?? outobj.Version;
            if (version >= 20140721)
                outobj.OnlineScoreId = reader.ReadInt64();
            else if (version >= 20121008)
                outobj.OnlineScoreId = reader.ReadInt32();

            if ((((Mods)outobj.Mods) & DataTypes.Mods.Tp) != 0)
                outobj.AdditionalMods = reader.ReadDouble();

            return outobj;
        }

        private static DateTime GetDate(long ticks)
        {
            if (ticks < 0L)
            {
                return new DateTime();
            }
            try
            {
                return new DateTime(ticks, DateTimeKind.Utc);
            }
            catch (Exception e)
            {
                return new DateTime();
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}