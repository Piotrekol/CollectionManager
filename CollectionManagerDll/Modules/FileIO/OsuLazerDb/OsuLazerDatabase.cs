using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using Realms;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CollectionManager.Modules.FileIO.OsuLazerDb;
public sealed class OsuLazerDatabase
    : OsuRealmReader
{
    private readonly IMapDataManager mapDataManager;
    private readonly LazerBeatmap baseBeatmap = new();

    public OsuLazerDatabase(IMapDataManager mapDataManager)
    {
        this.mapDataManager = mapDataManager;
    }

    public void Load(string realmFilePath, IProgress<string> progress, CancellationToken cancellationToken)
    {
        using var localRealm = GetRealm(realmFilePath);
        var allLazerBeatmaps = localRealm.All<BeatmapInfo>();
        var beatmapsCount = allLazerBeatmaps.Count();
        progress?.Report($"Loading {beatmapsCount} beatmaps");
        mapDataManager.StartMassStoring();

        foreach (var lazerBeatmap in allLazerBeatmaps.AsEnumerable())
        {
            ToBeatmap(lazerBeatmap, baseBeatmap);
            mapDataManager.StoreBeatmap(baseBeatmap);
        }

        mapDataManager.EndMassStoring();
        progress?.Report($"Loaded {beatmapsCount} beatmaps");
    }

    private Beatmap ToBeatmap(BeatmapInfo beatmapInfo, LazerBeatmap baseBeatmap)
    {
        baseBeatmap.InitEmptyValues();

        baseBeatmap.AudioRelativeFilePath = beatmapInfo.AudioFile is not null ? GetRelativePath(beatmapInfo.AudioFile.File) : string.Empty;
        baseBeatmap.BackgroundRelativeFilePath = beatmapInfo.BackgroundFile is not null ? GetRelativePath(beatmapInfo.BackgroundFile.File) : string.Empty;
        baseBeatmap.ModPpStars.Add(ToPlayMode(beatmapInfo.Ruleset.ShortName), new StarRating { { 0, Math.Round(beatmapInfo.StarRating, 2) } });
        baseBeatmap.TitleUnicode = beatmapInfo.Metadata.TitleUnicode;
        baseBeatmap.TitleRoman = beatmapInfo.Metadata.Title;
        baseBeatmap.ArtistUnicode = beatmapInfo.Metadata.ArtistUnicode;
        baseBeatmap.ArtistRoman = beatmapInfo.Metadata.Artist;
        baseBeatmap.Creator = beatmapInfo.Metadata.Author.Username;
        baseBeatmap.DiffName = beatmapInfo.DifficultyName;
        baseBeatmap.Mp3Name = beatmapInfo.AudioFile?.Filename ?? string.Empty;
        baseBeatmap.Md5 = beatmapInfo.MD5Hash; 
        baseBeatmap.OsuFileName = beatmapInfo.File?.File.Hash ?? string.Empty;
        baseBeatmap.Tags = beatmapInfo.Metadata.Tags;
        baseBeatmap.Somestuff = 0;
        baseBeatmap.State = (byte)ToLocalState(beatmapInfo.Status);
        baseBeatmap.Circles = 0;// TODO:
        baseBeatmap.Sliders = 0;// TODO:
        baseBeatmap.Spinners = 0;// TODO:
        baseBeatmap.EditDate = beatmapInfo.BeatmapSet.DateAdded;
        baseBeatmap.ApproachRate = beatmapInfo.Difficulty.ApproachRate;
        baseBeatmap.CircleSize = beatmapInfo.Difficulty.CircleSize;
        baseBeatmap.HpDrainRate = beatmapInfo.Difficulty.DrainRate;
        baseBeatmap.OverallDifficulty = beatmapInfo.Difficulty.OverallDifficulty;
        baseBeatmap.SliderVelocity = beatmapInfo.Difficulty.SliderMultiplier;
        baseBeatmap.DrainingTime = (int)beatmapInfo.Length; //TODO: Haven't checked if this is correct value.. most likely not
        baseBeatmap.TotalTime = 0; //TODO: most likely this is value above...?
        baseBeatmap.PreviewTime = beatmapInfo.Metadata.PreviewTime;
        baseBeatmap.MapId = beatmapInfo.OnlineID;
        baseBeatmap.MapSetId = beatmapInfo.BeatmapSet.OnlineID;
        baseBeatmap.ThreadId = 0;
        baseBeatmap.OsuGrade = OsuGrade.Null; //TODO: score reading
        baseBeatmap.TaikoGrade = OsuGrade.Null;
        baseBeatmap.CatchGrade = OsuGrade.Null;
        baseBeatmap.ManiaGrade = OsuGrade.Null;
        baseBeatmap.Offset = beatmapInfo.UserSettings.Offset;
        baseBeatmap.StackLeniency = beatmapInfo.StackLeniency;
        baseBeatmap.PlayMode = ToPlayMode(beatmapInfo.Ruleset.ShortName);
        baseBeatmap.Source = beatmapInfo.Metadata.Source;
        baseBeatmap.AudioOffset = 0; // not used anywhere
        baseBeatmap.LetterBox = string.Empty;// not used anywhere
        baseBeatmap.Played = false;// not used anywhere
        baseBeatmap.LastPlayed = beatmapInfo.LastPlayed;
        baseBeatmap.IsOsz2 = false;// not used anywhere
        baseBeatmap.Dir = string.Empty;// not relevant in lazer
        baseBeatmap.LastSync = beatmapInfo.LastOnlineUpdate;
        baseBeatmap.DisableHitsounds = false;
        baseBeatmap.DisableSkin = false;
        baseBeatmap.DisableSb = false;
        baseBeatmap.BgDim = 0;
        baseBeatmap.MinBpm = 0;//TODO: 
        baseBeatmap.MaxBpm = 0;//TODO: 
        baseBeatmap.MainBpm = beatmapInfo.BPM;

        return null;
    }

    private string GetRelativePath(RealmFile realmFile)
            => Path.Combine(realmFile.Hash.Remove(1), realmFile.Hash.Remove(2), realmFile.Hash);

    private PlayMode ToPlayMode(string rulesetShortName)
        => rulesetShortName.ToLowerInvariant() switch
        {
            "osu" => PlayMode.Osu,
            "taiko" => PlayMode.Taiko,
            "fruits" => PlayMode.CatchTheBeat,
            "mania" => PlayMode.OsuMania,

            _ => PlayMode.Osu,
        };

    private SubmissionStatus ToLocalState(BeatmapOnlineStatus status)
        => status switch
        {
            BeatmapOnlineStatus.LocallyModified => SubmissionStatus.NotSubmitted,
            BeatmapOnlineStatus.None => SubmissionStatus.Unknown,
            BeatmapOnlineStatus.Graveyard => SubmissionStatus.Pending,
            BeatmapOnlineStatus.WIP => SubmissionStatus.Pending,
            BeatmapOnlineStatus.Pending => SubmissionStatus.Pending,
            BeatmapOnlineStatus.Ranked => SubmissionStatus.Ranked,
            BeatmapOnlineStatus.Approved => SubmissionStatus.Approved,
            BeatmapOnlineStatus.Qualified => SubmissionStatus.Qualified,
            BeatmapOnlineStatus.Loved => SubmissionStatus.Loved,
            _ => SubmissionStatus.Unknown,
        };
}
