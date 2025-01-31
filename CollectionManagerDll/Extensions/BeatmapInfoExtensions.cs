using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuLazerDb;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CollectionManager.Extensions;

internal static class BeatmapInfoExtensions
{
    public static LazerBeatmap ToLazerBeatmap(this BeatmapInfo beatmapInfo, IScoreDataManager scoreDataManager, LazerFile[] setFiles)
    {
        LazerBeatmap lazerBeatmap = new()
        {
            SetFiles = setFiles,
            BackgroundFileName = beatmapInfo.Metadata.BackgroundFile,
            TitleUnicode = beatmapInfo.Metadata.TitleUnicode,
            TitleRoman = beatmapInfo.Metadata.Title,
            ArtistUnicode = beatmapInfo.Metadata.ArtistUnicode,
            ArtistRoman = beatmapInfo.Metadata.Artist,
            Creator = beatmapInfo.Metadata.Author.Username,
            DiffName = beatmapInfo.DifficultyName,
            Md5 = beatmapInfo.MD5Hash,
            MapHash = beatmapInfo.Hash,
            Mp3Name = beatmapInfo.Metadata.AudioFile,
            Tags = beatmapInfo.Metadata.Tags,
            State = (byte)ToLocalState(beatmapInfo.Status),
            EditDate = beatmapInfo.BeatmapSet.DateAdded,
            ApproachRate = beatmapInfo.Difficulty.ApproachRate,
            CircleSize = beatmapInfo.Difficulty.CircleSize,
            HpDrainRate = beatmapInfo.Difficulty.DrainRate,
            OverallDifficulty = beatmapInfo.Difficulty.OverallDifficulty,
            SliderVelocity = beatmapInfo.Difficulty.SliderMultiplier,
            TotalTime = (int)beatmapInfo.Length,
            PreviewTime = beatmapInfo.Metadata.PreviewTime,
            MapId = beatmapInfo.OnlineID,
            MapSetId = beatmapInfo.BeatmapSet.OnlineID,
            PlayMode = ToPlayMode(beatmapInfo.Ruleset.ShortName),
            Source = beatmapInfo.Metadata.Source,
            LastPlayed = beatmapInfo.LastPlayed,
            LastSync = beatmapInfo.LastOnlineUpdate,
            MainBpm = beatmapInfo.BPM,
            OsuGrade = scoreDataManager.GetTopReplayGrade(beatmapInfo.Hash, PlayMode.Osu),
            TaikoGrade = scoreDataManager.GetTopReplayGrade(beatmapInfo.Hash, PlayMode.Taiko),
            CatchGrade = scoreDataManager.GetTopReplayGrade(beatmapInfo.Hash, PlayMode.CatchTheBeat),
            ManiaGrade = scoreDataManager.GetTopReplayGrade(beatmapInfo.Hash, PlayMode.OsuMania),

            // not relevant in lazer
            //Dir = string.Empty,

            // Never used anywhere
            //OsuFileName = beatmapInfo.Hash,
            //Offset = beatmapInfo.UserSettings.Offset,

            // Not stored in realm
            //DrainingTime = 0,
            //Circles = 0,
            //Sliders = 0,
            //Spinners = 0,
            //Somestuff = 0,
            //ThreadId = 0,
            //AudioOffset = 0,
            //LetterBox = string.Empty,
            //Played = false,
            //IsOsz2 = false,
            //MinBpm = 0,
            //MaxBpm = 0,
            //StackLeniency = -1,
            //DisableHitsounds = false,
            //DisableSkin = false,
            //DisableSb = false,
            //BgDim = 0,
        };

        lazerBeatmap.ModPpStars.Add(
            ToPlayMode(beatmapInfo.Ruleset.ShortName),
            new StarRating { { (int)PlayMode.Osu, Math.Round(beatmapInfo.StarRating, 2) } });

        return lazerBeatmap;
    }

    private static PlayMode ToPlayMode(string rulesetShortName)
        => rulesetShortName.ToLowerInvariant() switch
        {
            "osu" => PlayMode.Osu,
            "taiko" => PlayMode.Taiko,
            "fruits" => PlayMode.CatchTheBeat,
            "mania" => PlayMode.OsuMania,
            _ => PlayMode.Osu,
        };

    private static SubmissionStatus ToLocalState(BeatmapOnlineStatus status)
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
