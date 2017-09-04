using System;
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
        public Dictionary<PlayModes, Dictionary<int, double>> ModPpStars = new Dictionary<PlayModes, Dictionary<int, double>>();
        //public Dictionary<int, double> ModPpStars = new Dictionary<int, double>();
        public double StarsNomod
        {
            get
            {
                if (ModPpStars.ContainsKey(PlayModes.Osu) && ModPpStars[PlayModes.Osu].ContainsKey(0))
                    return ModPpStars[PlayModes.Osu][0];
                if (ModPpStars.ContainsKey(playMode) && ModPpStars[playMode].ContainsKey(0))
                    return ModPpStars[playMode][0];
                return 0d;
            }
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
        public int MapRating { get; set; }
        public short Offset { get; set; }
        public float StackLeniency { get; set; }
        private PlayModes playMode;
        public PlayModes PlayMode
        {
            get
            {
                return playMode;
            }

            set
            {
                playMode = value;
                if (Enum.IsDefined(PlayMode.GetType(), value))
                {
                    playMode = value;
                }
                else
                {
                    playMode = PlayModes.Osu;
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
        public string VideoDir { get; set; }

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
            MapRating = b.MapRating;
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
            MapRating = b.MapRating;
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
            ModPpStars = new Dictionary<PlayModes, Dictionary<int, double>>();
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
            MapRating = 0;
            Offset = 0;
            StackLeniency = 0;
            PlayMode = PlayModes.Osu;
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

        public override int GetHashCode()
        {
            return Md5.GetHashCode();
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
            if (MapRating != b.MapRating)
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
    }
}

