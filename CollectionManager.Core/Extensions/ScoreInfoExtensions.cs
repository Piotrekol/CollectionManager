﻿namespace CollectionManager.Core.Extensions;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Mod;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels.Enums;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

internal static class ScoreInfoExtensions
{
    private static readonly Lazy<ModParser> _modParser = new();
    private static readonly Lazy<string[]> _AllModAcronyms = new(()
        => _modParser.Value.AllMods.Select(x => x.ShortMod).ToArray()
    );

    internal static LazerReplay ToLazerReplay(this ScoreInfo scoreInfo)
    {
        LazerMod[] lazerMods = string.IsNullOrWhiteSpace(scoreInfo.ModsJson)
            ? null
            : JsonSerializer.Deserialize<LazerMod[]>(scoreInfo.ModsJson);

        Mods mods = (lazerMods ?? Enumerable.Empty<LazerMod>())
             .Select(m => m.Acronym)
             .Where(_AllModAcronyms.Value.Contains)
             .SelectMany(shortMod => _modParser.Value.AllMods.Where(x => x.ShortMod == shortMod))
             .Select(osuMod => osuMod.Value)
             .Aggregate(Mods.Nm, (accumulator, value) => accumulator |= value);

        LazerReplay replay = new()
        {
            PlayMode = (PlayMode)scoreInfo.RulesetID,
            LazerVersion = scoreInfo.ClientVersion,
            Version = -999,
            MapHash = scoreInfo.BeatmapHash,
            PlayerName = scoreInfo.RealmUser.Username,
            UserId = scoreInfo.UserID,
            ReplayHash = scoreInfo.Hash,
            TotalScore = scoreInfo.TotalScore,
            MaxCombo = scoreInfo.MaxCombo,
            Grade = ToOsuGrade(scoreInfo.Rank),

            C300 = -1,
            C100 = -1,
            C50 = -1,
            Geki = -1,
            Katu = -1,
            Miss = -1,

            Mods = (int)mods,
            Date = scoreInfo.Date,
            OnlineScoreId = scoreInfo.OnlineID,

            DateTicks = -1,
            ReplayData = null,
            CompressedReplayLength = -1,
            CompressedReplay = null,

            Perfect = false,
            AdditionalMods = 0d
        };

        LazerReplayStatistics stats = string.IsNullOrWhiteSpace(scoreInfo.StatisticsJson)
            ? null
            : JsonSerializer.Deserialize<LazerReplayStatistics>(scoreInfo.StatisticsJson);

        if (stats is not null)
        {
            replay.C300 = stats.great ?? 0;
            // osu, taiko, mania and fallback for ctb
            replay.C100 = stats.ok ?? stats.large_tick_hit ?? 0;
            // osu, taiko, mania and fallback for ctb
            replay.C50 = stats.meh ?? stats.small_tick_hit ?? 0;
            replay.Miss = stats.miss ?? 0;

            //gekis and katus are unrecoverable for taiko scores - lazer stores sum of these.
            //Mania or ctb
            replay.Geki = stats.perfect ?? stats.large_bonus ?? 0;
            //Mania or ctb or taiko
            replay.Katu = stats.good ?? stats.small_tick_hit ?? stats.large_bonus ?? 0;
        }

        return replay;
    }

    private static OsuGrade ToOsuGrade(ScoreRank scoreRank)
        => scoreRank switch
        {
            ScoreRank.F => OsuGrade.Null,
            ScoreRank.D => OsuGrade.D,
            ScoreRank.C => OsuGrade.C,
            ScoreRank.B => OsuGrade.B,
            ScoreRank.A => OsuGrade.A,
            ScoreRank.S => OsuGrade.S,
            ScoreRank.SH => OsuGrade.SH,
            ScoreRank.X => OsuGrade.SS,
            ScoreRank.XH => OsuGrade.SSH,
            _ => OsuGrade.Null
        };

    private class LazerMod
    {
        [JsonPropertyName("acronym")]
        public string Acronym { get; set; }
    }

    private class LazerReplayStatistics
    {
        [JsonPropertyName("none")]
        public int? none { get; set; }

        [JsonPropertyName("miss")]
        public int? miss { get; set; }

        [JsonPropertyName("meh")]
        public int? meh { get; set; }

        [JsonPropertyName("ok")]
        public int? ok { get; set; }

        [JsonPropertyName("good")]
        public int? good { get; set; }

        [JsonPropertyName("great")]
        public int? great { get; set; }

        [JsonPropertyName("perfect")]
        public int? perfect { get; set; }

        [JsonPropertyName("small_tick_miss")]
        public int? small_tick_miss { get; set; }

        [JsonPropertyName("small_tick_hit")]
        public int? small_tick_hit { get; set; }

        [JsonPropertyName("large_tick_miss")]
        public int? large_tick_miss { get; set; }

        [JsonPropertyName("large_tick_hit")]
        public int? large_tick_hit { get; set; }

        [JsonPropertyName("small_bonus")]
        public int? small_bonus { get; set; }

        [JsonPropertyName("large_bonus")]
        public int? large_bonus { get; set; }

        [JsonPropertyName("ignore_miss")]
        public int? ignore_miss { get; set; }

        [JsonPropertyName("ignore_hit")]
        public int? ignore_hit { get; set; }

        [JsonPropertyName("combo_break")]
        public int? combo_break { get; set; }

        [JsonPropertyName("slider_tail_hit")]
        public int? slider_tail_hit { get; set; }
    }
}
