using System;
using System.Collections;
using System.Collections.Generic;
using CollectionManager.Enums;

namespace CollectionManager.DataTypes
{

    public abstract class Beatmap : ICloneable
    {
        private string _titleUnicode;
        public string TitleUnicode
        {
            get
            {
                return _titleUnicode == string.Empty ? TitleRoman : _titleUnicode;
            }
            set { _titleUnicode = value; }
        }
        private string _artistUnicode;
        public string ArtistUnicode
        {
            get { return _artistUnicode == string.Empty ? ArtistRoman : _artistUnicode; }
            set { _artistUnicode = value; }
        }
        public string TitleRoman { get; set; }
        public string ArtistRoman { get; set; }
        public string Artist
        {
            get
            {
                if (!string.IsNullOrEmpty(ArtistRoman))
                    return ArtistRoman;
                if (!string.IsNullOrEmpty(ArtistUnicode))
                    return ArtistUnicode;

                return "";
            }
        }
        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(TitleRoman))
                    return TitleRoman;
                if (!string.IsNullOrEmpty(TitleUnicode))
                    return TitleUnicode;

                return "";
            }
        }

        public string Creator { get; set; }
        public string DiffName { get; set; }
        public string Mp3Name { get; set; }
        public string Md5 { get; set; }
        public string OsuFileName { get; set; }
        public string MapLink
        {
            get
            {
                if (MapId == 0)
                    return MapSetLink;
                return @"http://osu.ppy.sh/b/" + MapId;

            }
        }
        public string MapSetLink
        {
            get
            {
                if (MapSetId == 0)
                    return string.Empty;
                return @"http://osu.ppy.sh/s/" + MapSetId;
            }
        }
        //TODO: add helper functions for adding/removing star values
        public PlayModeStars ModPpStars = new PlayModeStars();
        public double StarsNomod => Stars(PlayMode);

        public double Stars(PlayMode playMode, Mods mods = Mods.Omod)
        {
            mods = mods & Mods.MapChanging;
            if (ModPpStars.ContainsKey(_playMode) && ModPpStars[_playMode].ContainsKey((int)mods))
                return ModPpStars[_playMode][(int)mods];
            return -1d;
        }

        public double MaxBpm { get; set; }
        public double MinBpm { get; set; }

        public string Tags { get; set; }
        public string StateStr { get; private set; }
        private byte _state;
        public byte State
        {
            get { return _state; }
            set
            {
                string val;
                _state = value;
                switch (value)
                {
                    case 0: val = "Not updated"; break;
                    case 1: val = "Unsubmitted"; break;
                    case 2: val = "Pending"; break;
                    case 3: val = "??"; break;
                    case 4: val = "Ranked"; break;
                    case 5: val = "Approved"; break;
                    case 7: val = "Loved"; break;
                    default: val = "??" + value.ToString(); break;
                }
                StateStr = val;
            }
        }
        public short Circles { get; set; }
        public short Sliders { get; set; }
        public short Spinners { get; set; }
        public DateTime? EditDate { get; set; }
        public float ApproachRate { get; set; }
        public float CircleSize { get; set; }
        public float HpDrainRate { get; set; }
        public float OverallDifficulty { get; set; }
        public double SliderVelocity { get; set; }
        public int DrainingTime { get; set; }
        public int TotalTime { get; set; }
        public int PreviewTime { get; set; }
        public int MapId { get; set; }
        public int MapSetId { get; set; }
        public int ThreadId { get; set; }
        public OsuGrade OsuGrade { get; set; } = OsuGrade.Null;
        public OsuGrade TaikoGrade { get; set; } = OsuGrade.Null;
        public OsuGrade CatchGrade { get; set; } = OsuGrade.Null;
        public OsuGrade ManiaGrade { get; set; } = OsuGrade.Null;

        public short Offset { get; set; }
        public float StackLeniency { get; set; }
        private PlayMode _playMode;
        public PlayMode PlayMode
        {
            get
            {
                return _playMode;
            }

            set
            {
                _playMode = value;
                if (Enum.IsDefined(PlayMode.GetType(), value))
                {
                    _playMode = value;
                }
                else
                {
                    _playMode = PlayMode.Osu;
                }
            }
        }
        public string Source { get; set; }
        public short AudioOffset { get; set; }
        public string LetterBox { get; set; }
        public bool Played { get; set; }
        public DateTime? LastPlayed { get; set; }
        public bool IsOsz2 { get; set; }
        public string Dir { get; set; }
        public DateTime? LastSync { get; set; }
        public bool DisableHitsounds { get; set; }
        public bool DisableSkin { get; set; }
        public bool DisableSb { get; set; }
        public short BgDim { get; set; }
        public int Somestuff { get; set; }

        public Beatmap()
        {
            InitEmptyValues();
        }

        public void CloneValues(Beatmap b)
        {
            TitleUnicode = b.TitleUnicode;
            TitleRoman = b.TitleRoman;
            ArtistUnicode = b.ArtistUnicode;
            ArtistRoman = b.ArtistRoman;
            Creator = b.Creator;
            DiffName = b.DiffName;
            Mp3Name = b.Mp3Name;
            Md5 = b.Md5;
            OsuFileName = b.OsuFileName;
            Tags = b.Tags;
            Somestuff = b.Somestuff;
            State = b.State;
            Circles = b.Circles;
            Sliders = b.Sliders;
            Spinners = b.Spinners;
            EditDate = b.EditDate;
            ApproachRate = b.ApproachRate;
            CircleSize = b.CircleSize;
            HpDrainRate = b.HpDrainRate;
            OverallDifficulty = b.OverallDifficulty;
            SliderVelocity = b.SliderVelocity;
            DrainingTime = b.DrainingTime;
            TotalTime = b.TotalTime;
            PreviewTime = b.PreviewTime;
            MapId = b.MapId;
            MapSetId = b.MapSetId;
            ThreadId = b.ThreadId;
            OsuGrade = b.OsuGrade;
            TaikoGrade = b.TaikoGrade;
            CatchGrade = b.CatchGrade;
            ManiaGrade = b.ManiaGrade;
            Offset = b.Offset;
            StackLeniency = b.StackLeniency;
            PlayMode = b.PlayMode;
            Source = b.Source;
            AudioOffset = b.AudioOffset;
            LetterBox = b.LetterBox;
            Played = b.Played;
            LastPlayed = b.LastPlayed;
            IsOsz2 = b.IsOsz2;
            Dir = b.Dir;
            LastSync = b.LastSync;
            DisableHitsounds = b.DisableHitsounds;
            DisableSkin = b.DisableSkin;
            DisableSb = b.DisableSb;
            BgDim = b.BgDim;
            ModPpStars = b.ModPpStars;
            MaxBpm = b.MaxBpm;
            MinBpm = b.MinBpm;
        }
        public Beatmap(Beatmap b)
        {
            TitleUnicode = b.TitleUnicode;
            TitleRoman = b.TitleRoman;
            ArtistUnicode = b.ArtistUnicode;
            ArtistRoman = b.ArtistRoman;
            Creator = b.Creator;
            DiffName = b.DiffName;
            Mp3Name = b.Mp3Name;
            Md5 = b.Md5;
            OsuFileName = b.OsuFileName;
            Tags = b.Tags;
            Somestuff = b.Somestuff;
            State = b.State;
            Circles = b.Circles;
            Sliders = b.Sliders;
            Spinners = b.Spinners;
            EditDate = b.EditDate;
            ApproachRate = b.ApproachRate;
            CircleSize = b.CircleSize;
            HpDrainRate = b.HpDrainRate;
            OverallDifficulty = b.OverallDifficulty;
            SliderVelocity = b.SliderVelocity;
            DrainingTime = b.DrainingTime;
            TotalTime = b.TotalTime;
            PreviewTime = b.PreviewTime;
            MapId = b.MapId;
            MapSetId = b.MapSetId;
            ThreadId = b.ThreadId;
            OsuGrade = b.OsuGrade;
            TaikoGrade = b.TaikoGrade;
            CatchGrade = b.CatchGrade;
            ManiaGrade = b.ManiaGrade;
            Offset = b.Offset;
            StackLeniency = b.StackLeniency;
            PlayMode = b.PlayMode;
            Source = b.Source;
            AudioOffset = b.AudioOffset;
            LetterBox = b.LetterBox;
            Played = b.Played;
            LastPlayed = b.LastPlayed;
            IsOsz2 = b.IsOsz2;
            Dir = b.Dir;
            LastSync = b.LastSync;
            DisableHitsounds = b.DisableHitsounds;
            DisableSkin = b.DisableSkin;
            DisableSb = b.DisableSb;
            BgDim = b.BgDim;
            ModPpStars = b.ModPpStars;
            MaxBpm = b.MaxBpm;
            MinBpm = b.MinBpm;
        }
        public Beatmap(string artist)
        {
            InitEmptyValues();
            ArtistUnicode = artist;
        }
        public Beatmap(string md5, bool collection)
        {
            InitEmptyValues();
            Md5 = md5;
        }
        public void InitEmptyValues()
        {
            ModPpStars = new PlayModeStars();
            TitleUnicode = string.Empty;
            TitleRoman = string.Empty;
            ArtistUnicode = string.Empty;
            ArtistRoman = string.Empty;
            Creator = string.Empty;
            DiffName = string.Empty;
            Mp3Name = string.Empty;
            Md5 = string.Empty;
            OsuFileName = string.Empty;
            Tags = string.Empty;
            Somestuff = 0;
            State = 0;
            Circles = 0;
            Sliders = 0;
            Spinners = 0;
            EditDate = null;
            ApproachRate = 0;
            CircleSize = 0;
            HpDrainRate = 0;
            OverallDifficulty = 0;
            SliderVelocity = 0;
            DrainingTime = 0;
            TotalTime = 0;
            PreviewTime = 0;
            MapId = 0;
            MapSetId = 0;
            ThreadId = 0;

            OsuGrade = OsuGrade.Null;
            TaikoGrade = OsuGrade.Null;
            CatchGrade = OsuGrade.Null;
            ManiaGrade = OsuGrade.Null;
            Offset = 0;
            StackLeniency = 0;
            PlayMode = PlayMode.Osu;
            Source = string.Empty;
            AudioOffset = 0;
            LetterBox = string.Empty;
            Played = false;
            LastPlayed = null;
            IsOsz2 = false;
            Dir = string.Empty;
            LastSync = null;
            DisableHitsounds = false;
            DisableSkin = false;
            DisableSb = false;
            BgDim = 0;
            MinBpm = 0.0f;
            MaxBpm = 0.0f;
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string GetChecksum()
        {
            //Md5 ensures checksum uniqueness while hashcode provides a way of checking for map contents(updates) in .db
            return $"{Md5}{GetHashCode()}";
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + TitleUnicode.GetHashCode();
                hash = hash * 23 + TitleRoman.GetHashCode();
                hash = hash * 23 + ArtistUnicode.GetHashCode();
                hash = hash * 23 + ArtistRoman.GetHashCode();
                hash = hash * 23 + Creator.GetHashCode();
                hash = hash * 23 + DiffName.GetHashCode();
                hash = hash * 23 + Mp3Name.GetHashCode();
                hash = hash * 23 + Md5.GetHashCode();
                hash = hash * 23 + OsuFileName.GetHashCode();
                hash = hash * 23 + Tags.GetHashCode();
                hash = hash * 23 + Somestuff.GetHashCode();
                hash = hash * 23 + _state.GetHashCode();
                hash = hash * 23 + Circles.GetHashCode();
                hash = hash * 23 + Sliders.GetHashCode();
                hash = hash * 23 + Spinners.GetHashCode();
                hash = hash * 23 + EditDate.GetHashCode();
                hash = hash * 23 + ApproachRate.GetHashCode();
                hash = hash * 23 + CircleSize.GetHashCode();
                hash = hash * 23 + HpDrainRate.GetHashCode();
                hash = hash * 23 + OverallDifficulty.GetHashCode();
                hash = hash * 23 + SliderVelocity.GetHashCode();
                hash = hash * 23 + DrainingTime.GetHashCode();
                hash = hash * 23 + TotalTime.GetHashCode();
                hash = hash * 23 + PreviewTime.GetHashCode();
                hash = hash * 23 + MapId.GetHashCode();
                hash = hash * 23 + MapSetId.GetHashCode();
                hash = hash * 23 + ThreadId.GetHashCode();
                hash = hash * 23 + OsuGrade.GetHashCode();
                hash = hash * 23 + TaikoGrade.GetHashCode();
                hash = hash * 23 + CatchGrade.GetHashCode();
                hash = hash * 23 + ManiaGrade.GetHashCode();
                hash = hash * 23 + Offset.GetHashCode();
                hash = hash * 23 + StackLeniency.GetHashCode();
                hash = hash * 23 + PlayMode.GetHashCode();
                hash = hash * 23 + Source.GetHashCode();
                hash = hash * 23 + AudioOffset.GetHashCode();
                hash = hash * 23 + LetterBox.GetHashCode();
                hash = hash * 23 + Played.GetHashCode();
                hash = hash * 23 + LastPlayed.GetHashCode();
                hash = hash * 23 + IsOsz2.GetHashCode();
                hash = hash * 23 + Dir.GetHashCode();
                //hash = hash * 23 + LastSync.GetHashCode(); //This value is updated by osu even if no changes were made to the actual data
                hash = hash * 23 + DisableHitsounds.GetHashCode();
                hash = hash * 23 + DisableSkin.GetHashCode();
                hash = hash * 23 + DisableSb.GetHashCode();
                hash = hash * 23 + BgDim.GetHashCode();
                hash = hash * 23 + ModPpStars.GetHashCode();
                hash = hash * 23 + MaxBpm.GetHashCode();
                hash = hash * 23 + MinBpm.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var b = obj as Beatmap;
            if (b == null)
                return false;
            if (TitleUnicode != b.TitleUnicode)
                return false;
            if (TitleRoman != b.TitleRoman)
                return false;
            if (ArtistUnicode != b.ArtistUnicode)
                return false;
            if (ArtistRoman != b.ArtistRoman)
                return false;
            if (Creator != b.Creator)
                return false;
            if (DiffName != b.DiffName)
                return false;
            if (Mp3Name != b.Mp3Name)
                return false;
            if (Md5 != b.Md5)
                return false;
            if (OsuFileName != b.OsuFileName)
                return false;
            if (Tags != b.Tags)
                return false;
            if (Somestuff != b.Somestuff)
                return false;
            if (State != b.State)
                return false;
            if (Circles != b.Circles)
                return false;
            if (Sliders != b.Sliders)
                return false;
            if (Spinners != b.Spinners)
                return false;
            if (EditDate != b.EditDate)
                return false;
            if (ApproachRate != b.ApproachRate)
                return false;
            if (CircleSize != b.CircleSize)
                return false;
            if (HpDrainRate != b.HpDrainRate)
                return false;
            if (OverallDifficulty != b.OverallDifficulty)
                return false;
            if (SliderVelocity != b.SliderVelocity)
                return false;
            if (DrainingTime != b.DrainingTime)
                return false;
            if (TotalTime != b.TotalTime)
                return false;
            if (PreviewTime != b.PreviewTime)
                return false;
            if (MapId != b.MapId)
                return false;
            if (MapSetId != b.MapSetId)
                return false;
            if (ThreadId != b.ThreadId)
                return false;
            if (OsuGrade != b.OsuGrade)
                return false;
            if (TaikoGrade != b.TaikoGrade)
                return false;
            if (CatchGrade != b.CatchGrade)
                return false;
            if (ManiaGrade != b.ManiaGrade)
                return false;


            if (Offset != b.Offset)
                return false;
            if (StackLeniency != b.StackLeniency)
                return false;
            if (PlayMode != b.PlayMode)
                return false;
            if (Source != b.Source)
                return false;
            if (AudioOffset != b.AudioOffset)
                return false;
            if (LetterBox != b.LetterBox)
                return false;
            if (Played != b.Played)
                return false;
            if (LastPlayed != b.LastPlayed)
                return false;
            if (IsOsz2 != b.IsOsz2)
                return false;
            if (Dir != b.Dir)
                return false;
            if (LastSync != b.LastSync)
                return false;
            if (DisableHitsounds != b.DisableHitsounds)
                return false;
            if (DisableSkin != b.DisableSkin)
                return false;
            if (DisableSb != b.DisableSb)
                return false;
            if (BgDim != b.BgDim)
                return false;
            if (ModPpStars != b.ModPpStars)
                return false;
            if (MaxBpm != b.MaxBpm)
                return false;
            if (MinBpm != b.MinBpm)
                return false;
            return true;
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Artist) && string.IsNullOrEmpty(Title))
            {
                if (string.IsNullOrEmpty(Md5))
                    return Md5;
                return "mapId: " + MapId;
            }
            var baseStr = Artist + " - " + Title;
            return baseStr;
        }
        public string ToString(bool withDiff)
        {
            if (withDiff)
            {
                return ToString() + " [" + DiffName + "]";
            }
            else return ToString();
        }
    }
}

