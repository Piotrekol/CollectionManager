namespace CollectionManager.Core.Types;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using System;
using System.Globalization;

public abstract class Beatmap : ICloneable
{
    public string TitleUnicode { get; set; }
    public string ArtistUnicode { get; set; }
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
    public string MapLink => MapId <= 0 ? MapSetLink : @"https://osu.ppy.sh/b/" + MapId;
    public string MapSetLink => MapSetId <= 0 ? string.Empty : @"https://osu.ppy.sh/s/" + MapSetId;
    public PlayModeStars ModPpStars { get; private set; } = [];
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
                _ => "??" + value.ToString(CultureInfo.InvariantCulture),
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

    public double Offset { get; set; }
    public float StackLeniency { get; set; }
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
    public TimingPoint[] TimingPoints { get; set; } = [];
    public bool DisableVideo { get; set; }
    public bool VisualOverride { get; set; }
    public int LastModification { get; set; }
    public byte ManiaScrollSpeed { get; set; }

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
        TimingPoints = b.TimingPoints;
        DisableVideo = b.DisableVideo;
        VisualOverride = b.VisualOverride;
        LastModification = b.LastModification;
        ManiaScrollSpeed = b.ManiaScrollSpeed;
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
        TimingPoints = b.TimingPoints;
        DisableVideo = b.DisableVideo;
        VisualOverride = b.VisualOverride;
        LastModification = b.LastModification;
        ManiaScrollSpeed = b.ManiaScrollSpeed;
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
        TimingPoints = [];
        DisableVideo = false;
        VisualOverride = false;
        LastModification = 0;
        ManiaScrollSpeed = 0;
    }

    public object Clone() => MemberwiseClone();

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(TitleUnicode);
        hash.Add(TitleRoman);
        hash.Add(ArtistUnicode);
        hash.Add(ArtistRoman);
        hash.Add(Creator);
        hash.Add(DiffName);
        hash.Add(Mp3Name);
        hash.Add(Md5);
        hash.Add(OsuFileName);
        hash.Add(Tags);
        hash.Add(_state);
        hash.Add(Circles);
        hash.Add(Sliders);
        hash.Add(Spinners);
        hash.Add(EditDate);
        hash.Add(ApproachRate);
        hash.Add(CircleSize);
        hash.Add(HpDrainRate);
        hash.Add(OverallDifficulty);
        hash.Add(SliderVelocity);
        hash.Add(DrainingTime);
        hash.Add(TotalTime);
        hash.Add(PreviewTime);
        hash.Add(MapId);
        hash.Add(MapSetId);
        hash.Add(ThreadId);
        hash.Add(OsuGrade);
        hash.Add(TaikoGrade);
        hash.Add(CatchGrade);
        hash.Add(ManiaGrade);
        hash.Add(Offset);
        hash.Add(StackLeniency);
        hash.Add(PlayMode);
        hash.Add(Source);
        hash.Add(AudioOffset);
        hash.Add(LetterBox);
        hash.Add(Played);
        hash.Add(LastPlayed);
        hash.Add(IsOsz2);
        hash.Add(Dir);
        hash.Add(DisableHitsounds);
        hash.Add(DisableSkin);
        hash.Add(DisableSb);
        hash.Add(BgDim);
        hash.Add(ModPpStars);
        hash.Add(MaxBpm);
        hash.Add(MinBpm);
        hash.Add(MainBpm);
        hash.Add(TimingPoints);
        hash.Add(DisableVideo);
        hash.Add(VisualOverride);
        hash.Add(LastModification);
        hash.Add(ManiaScrollSpeed);

        return hash.ToHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Beatmap b)
        {
            return false;
        }

        return TitleUnicode == b.TitleUnicode
            && TitleRoman == b.TitleRoman
            && ArtistUnicode == b.ArtistUnicode
            && ArtistRoman == b.ArtistRoman
            && Creator == b.Creator
            && DiffName == b.DiffName
            && Mp3Name == b.Mp3Name
            && Md5 == b.Md5
            && OsuFileName == b.OsuFileName
            && Tags == b.Tags
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
            && MainBpm == b.MainBpm
            && TimingPoints == b.TimingPoints
            && DisableVideo == b.DisableVideo
            && VisualOverride == b.VisualOverride
            && LastModification == b.LastModification
            && ManiaScrollSpeed == b.ManiaScrollSpeed;
    }

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

