namespace CollectionManager.Core.Types;
using CollectionManager.Core.Enums;
using System;

public abstract class Beatmap : ICloneable
{
    private string _titleUnicode;
    public string TitleUnicode
    {
        get => _titleUnicode == string.Empty ? TitleRoman : _titleUnicode; set => _titleUnicode = value;
    }
    private string _artistUnicode;
    public string ArtistUnicode
    {
        get => _artistUnicode == string.Empty ? ArtistRoman : _artistUnicode; set => _artistUnicode = value;
    }
    public string TitleRoman { get; set; }
    public string ArtistRoman { get; set; }
    public string Artist => !string.IsNullOrEmpty(ArtistRoman) ? ArtistRoman : !string.IsNullOrEmpty(ArtistUnicode) ? ArtistUnicode : "";
    public string Title => !string.IsNullOrEmpty(TitleRoman) ? TitleRoman : !string.IsNullOrEmpty(TitleUnicode) ? TitleUnicode : "";

    public string Creator { get; set; }
    public string DiffName { get; set; }
    public string Mp3Name { get; set; }
    public string Md5 { get; set; }
    public abstract string Hash { get; set; }
    public string OsuFileName { get; set; }
    public string MapLink => MapId == 0 ? MapSetLink : @"https://osu.ppy.sh/b/" + MapId;
    public string MapSetLink => MapSetId == 0 ? string.Empty : @"https://osu.ppy.sh/s/" + MapSetId;
    //TODO: add helper functions for adding/removing star values
    public PlayModeStars ModPpStars = [];
    public double StarsNomod => Stars(PlayMode);

    public float Stars(PlayMode playMode, Mods mods = Mods.Nm)
    {
        mods &= Mods.MapChanging;
        return ModPpStars.ContainsKey(_playMode) && ModPpStars[_playMode].ContainsKey((int)mods) ? ModPpStars[_playMode][(int)mods] : -1f;
    }

    public double MaxBpm { get; set; }
    public double MinBpm { get; set; }
    public double MainBpm { get; set; }

    public string Tags { get; set; }
    public string StateStr { get; private set; }
    private byte _state;
    public byte State
    {
        get => _state;
        set
        {
            _state = value;
            string val = value switch
            {
                0 => "Not updated",
                1 => "Unsubmitted",
                2 => "Pending",
                3 => "??",
                4 => "Ranked",
                5 => "Approved",
                7 => "Loved",
                _ => "??" + value.ToString(),
            };
            StateStr = val;
        }
    }
    public short Circles { get; set; }
    public short Sliders { get; set; }
    public short Spinners { get; set; }
    public DateTimeOffset? EditDate { get; set; }
    public float ApproachRate { get; set; }
    public float CircleSize { get; set; }
    public float HpDrainRate { get; set; }
    public float OverallDifficulty { get; set; }
    public double? SliderVelocity { get; set; }
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

    public double Offset { get; set; }
    public float? StackLeniency { get; set; }
    private PlayMode _playMode;
    public PlayMode PlayMode
    {
        get => _playMode;

        set
        {
            _playMode = value;
            _playMode = Enum.IsDefined(PlayMode.GetType(), value) ? value : PlayMode.Osu;
        }
    }
    public string Source { get; set; }
    public short AudioOffset { get; set; }
    public string LetterBox { get; set; }
    public bool Played { get; set; }
    public DateTimeOffset? LastPlayed { get; set; }
    public bool IsOsz2 { get; set; }
    public string Dir { get; set; }
    public DateTimeOffset? LastSync { get; set; }
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
        MainBpm = b.MainBpm;
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
        MainBpm = b.MainBpm;
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
        ModPpStars = [];
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
        MainBpm = 0.0f;
    }

    public object Clone() => MemberwiseClone();

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + TitleUnicode.GetHashCode();
            hash = (hash * 23) + TitleRoman.GetHashCode();
            hash = (hash * 23) + ArtistUnicode.GetHashCode();
            hash = (hash * 23) + ArtistRoman.GetHashCode();
            hash = (hash * 23) + Creator.GetHashCode();
            hash = (hash * 23) + DiffName.GetHashCode();
            hash = (hash * 23) + Mp3Name.GetHashCode();
            hash = (hash * 23) + Md5.GetHashCode();
            hash = (hash * 23) + OsuFileName.GetHashCode();
            hash = (hash * 23) + Tags.GetHashCode();
            hash = (hash * 23) + Somestuff.GetHashCode();
            hash = (hash * 23) + _state.GetHashCode();
            hash = (hash * 23) + Circles.GetHashCode();
            hash = (hash * 23) + Sliders.GetHashCode();
            hash = (hash * 23) + Spinners.GetHashCode();
            hash = (hash * 23) + EditDate.GetHashCode();
            hash = (hash * 23) + ApproachRate.GetHashCode();
            hash = (hash * 23) + CircleSize.GetHashCode();
            hash = (hash * 23) + HpDrainRate.GetHashCode();
            hash = (hash * 23) + OverallDifficulty.GetHashCode();
            hash = (hash * 23) + SliderVelocity.GetHashCode();
            hash = (hash * 23) + DrainingTime.GetHashCode();
            hash = (hash * 23) + TotalTime.GetHashCode();
            hash = (hash * 23) + PreviewTime.GetHashCode();
            hash = (hash * 23) + MapId.GetHashCode();
            hash = (hash * 23) + MapSetId.GetHashCode();
            hash = (hash * 23) + ThreadId.GetHashCode();
            hash = (hash * 23) + OsuGrade.GetHashCode();
            hash = (hash * 23) + TaikoGrade.GetHashCode();
            hash = (hash * 23) + CatchGrade.GetHashCode();
            hash = (hash * 23) + ManiaGrade.GetHashCode();
            hash = (hash * 23) + Offset.GetHashCode();
            hash = (hash * 23) + StackLeniency.GetHashCode();
            hash = (hash * 23) + PlayMode.GetHashCode();
            hash = (hash * 23) + Source.GetHashCode();
            hash = (hash * 23) + AudioOffset.GetHashCode();
            hash = (hash * 23) + LetterBox.GetHashCode();
            hash = (hash * 23) + Played.GetHashCode();
            hash = (hash * 23) + LastPlayed.GetHashCode();
            hash = (hash * 23) + IsOsz2.GetHashCode();
            hash = (hash * 23) + Dir.GetHashCode();
            //hash = hash * 23 + LastSync.GetHashCode(); //This value is updated by osu even if no changes were made to the actual data
            hash = (hash * 23) + DisableHitsounds.GetHashCode();
            hash = (hash * 23) + DisableSkin.GetHashCode();
            hash = (hash * 23) + DisableSb.GetHashCode();
            hash = (hash * 23) + BgDim.GetHashCode();
            hash = (hash * 23) + ModPpStars.GetHashCode();
            hash = (hash * 23) + MaxBpm.GetHashCode();
            hash = (hash * 23) + MinBpm.GetHashCode();
            hash = (hash * 23) + MainBpm.GetHashCode();
            return hash;
        }
    }

    public override bool Equals(object obj) => obj is Beatmap b
        && TitleUnicode == b.TitleUnicode
        && TitleRoman == b.TitleRoman
        && ArtistUnicode == b.ArtistUnicode
        && ArtistRoman == b.ArtistRoman
        && Creator == b.Creator
        && DiffName == b.DiffName
        && Mp3Name == b.Mp3Name
        && Md5 == b.Md5
        && OsuFileName == b.OsuFileName
        && Tags == b.Tags
        && Somestuff == b.Somestuff
        && State == b.State
        && Circles == b.Circles
        && Sliders == b.Sliders
        && Spinners == b.Spinners
        && EditDate == b.EditDate
        && ApproachRate == b.ApproachRate
        && CircleSize == b.CircleSize
        && HpDrainRate == b.HpDrainRate
        && OverallDifficulty == b.OverallDifficulty
        && SliderVelocity == b.SliderVelocity
        && DrainingTime == b.DrainingTime
        && TotalTime == b.TotalTime
        && PreviewTime == b.PreviewTime
        && MapId == b.MapId
        && MapSetId == b.MapSetId
        && ThreadId == b.ThreadId
        && OsuGrade == b.OsuGrade
        && TaikoGrade == b.TaikoGrade
        && CatchGrade == b.CatchGrade
        && ManiaGrade == b.ManiaGrade
        && Offset == b.Offset
        && StackLeniency == b.StackLeniency
        && PlayMode == b.PlayMode
        && Source == b.Source
        && AudioOffset == b.AudioOffset
        && LetterBox == b.LetterBox
        && Played == b.Played
        && LastPlayed == b.LastPlayed
        && IsOsz2 == b.IsOsz2
        && Dir == b.Dir
        && LastSync == b.LastSync
        && DisableHitsounds == b.DisableHitsounds
        && DisableSkin == b.DisableSkin
        && DisableSb == b.DisableSb
        && BgDim == b.BgDim
        && ModPpStars == b.ModPpStars
        && MaxBpm == b.MaxBpm
        && MinBpm == b.MinBpm
        && MainBpm == b.MainBpm;

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Artist) && string.IsNullOrEmpty(Title))
        {
            return MapId > 0 ? "mapId: " + MapId : $"unknown: {Md5}";
        }

        string baseStr = Artist + " - " + Title;
        return baseStr;
    }
    public string ToString(bool withDiff) => withDiff ? ToString() + " [" + DiffName + "]" : ToString();
}

