﻿using CollectionManager.Annotations;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels.Enums;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
[MapTo("Score")]
internal partial class ScoreInfo
    : IRealmObject
{
    [PrimaryKey]
    public Guid ID { get; set; }

    /// <summary>
    /// The <see cref="BeatmapInfo"/> this score was made against.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property may be <see langword="null"/> if the score was set on a beatmap (or a version of the beatmap) that is not available locally
    /// e.g. due to online updates, or local modifications to the beatmap.
    /// The property will only link to a <see cref="BeatmapInfo"/> if its <see cref="Beatmaps.BeatmapInfo.Hash"/> matches <see cref="BeatmapHash"/>.
    /// </para>
    /// <para>
    /// Due to the above, whenever setting this, make sure to also set <see cref="BeatmapHash"/> to allow relational consistency when a beatmap is potentially changed.
    /// </para>
    /// </remarks>
    public BeatmapInfo? BeatmapInfo { get; set; }

    /// <summary>
    /// The version of the client this score was set using.
    /// Sourced from <see cref="OsuGameBase.Version"/> at the point of score submission.
    /// </summary>
    public string ClientVersion { get; set; } = string.Empty;

    /// <summary>
    /// The <see cref="osu.Game.Beatmaps.BeatmapInfo.Hash"/> at the point in time when the score was set.
    /// </summary>
    public string BeatmapHash { get; set; } = string.Empty;

    public RulesetInfo Ruleset { get; set; } = null!;

    public IList<RealmNamedFileUsage> Files { get; } = null!;

    public string Hash { get; set; } = string.Empty;

    public bool DeletePending { get; set; }

    /// <summary>
    /// The total number of points awarded for the score.
    /// </summary>
    public long TotalScore { get; set; }

    /// <summary>
    /// The total number of points awarded for the score without including mod multipliers.
    /// </summary>
    /// <remarks>
    /// The purpose of this property is to enable future lossless rebalances of mod multipliers.
    /// </remarks>
    public long TotalScoreWithoutMods { get; set; }

    /// <summary>
    /// The version of processing applied to calculate total score as stored in the database.
    /// If this does not match <see cref="LegacyScoreEncoder.LATEST_VERSION"/>,
    /// the total score has not yet been updated to reflect the current scoring values.
    ///
    /// See <see cref="BackgroundDataStoreProcessor"/>'s conversion logic.
    /// </summary>
    /// <remarks>
    /// This may not match the version stored in the replay files.
    /// </remarks>
    public int TotalScoreVersion { get; set; } = -999;

    /// <summary>
    /// Used to preserve the total score for legacy scores.
    /// </summary>
    /// <remarks>
    /// Not populated if <see cref="IsLegacyScore"/> is <c>false</c>.
    /// </remarks>
    public long? LegacyTotalScore { get; set; }

    /// <summary>
    /// If background processing of this beatmap failed in some way, this flag will become <c>true</c>.
    /// Should be used to ensure we don't repeatedly attempt to reprocess the same scores each startup even though we already know they will fail.
    /// </summary>
    /// <remarks>
    /// See https://github.com/ppy/osu/issues/24301 for one example of how this can occur (missing beatmap file on disk).
    /// </remarks>
    public bool BackgroundReprocessingFailed { get; set; }

    public int MaxCombo { get; set; }

    public double Accuracy { get; set; }

    [Ignored]
    public bool HasOnlineReplay { get; set; }

    public DateTimeOffset Date { get; set; }

    public double? PP { get; set; }

    /// <summary>
    /// Whether the performance points in this score is awarded to the player. This is used for online display purposes (see <see cref="SoloScoreInfo.Ranked"/>).
    /// </summary>
    [Ignored]
    public bool Ranked { get; set; }

    /// <summary>
    /// The online ID of this score.
    /// </summary>
    /// <remarks>
    /// In the osu-web database, this ID (if present) comes from the new <c>solo_scores</c> table.
    /// </remarks>
    [Indexed]
    public long OnlineID { get; set; } = -1;

    /// <summary>
    /// The legacy online ID of this score.
    /// </summary>
    /// <remarks>
    /// In the osu-web database, this ID (if present) comes from the legacy <c>osu_scores_*_high</c> tables.
    /// This ID is also stored to replays set on osu!stable.
    /// </remarks>
    [Indexed]
    public long LegacyOnlineID { get; set; } = -1;

    [MapTo("User")]
    public RealmUser RealmUser { get; set; } = null!;

    [MapTo("Mods")]
    public string ModsJson { get; set; } = string.Empty;

    [MapTo("Statistics")]
    public string StatisticsJson { get; set; } = string.Empty;

    [MapTo("MaximumStatistics")]
    public string MaximumStatisticsJson { get; set; } = string.Empty;

    //public ScoreInfo(BeatmapInfo? beatmap = null, RulesetInfo? ruleset = null, RealmUser? realmUser = null)
    //{
    //    Ruleset = ruleset ?? new RulesetInfo();
    //    BeatmapInfo = beatmap ?? new BeatmapInfo();
    //    BeatmapHash = BeatmapInfo.Hash;
    //    RealmUser = realmUser ?? new RealmUser();
    //    ID = Guid.NewGuid();
    //}

    [UsedImplicitly] // Realm
    private ScoreInfo()
    {
    }

    // TODO: this is a bit temporary to account for the fact that this class is used to ferry API user data to certain UI components.
    // Eventually we should either persist enough information to realm to not require the API lookups, or perform the API lookups locally.
    private APIUser? user;

    //[Ignored]
    //public APIUser User
    //{
    //    get => user ??= new APIUser
    //    {
    //        Id = RealmUser.OnlineID,
    //        Username = RealmUser.Username,
    //        CountryCode = RealmUser.CountryCode,
    //    };
    //    set
    //    {
    //        user = value;

    //        RealmUser = new RealmUser
    //        {
    //            OnlineID = user.OnlineID,
    //            Username = user.Username,
    //            CountryCode = user.CountryCode,
    //        };
    //    }
    //}

    [Ignored]
    public ScoreRank Rank
    {
        get => (ScoreRank)RankInt;
        set => RankInt = (int)value;
    }

    [MapTo(nameof(Rank))]
    public int RankInt { get; set; }

    public int UserID => RealmUser.OnlineID;

    public int RulesetID => Ruleset.OnlineID;

    public int Combo { get; set; }

    /// <summary>
    /// Whether this <see cref="ScoreInfo"/> represents a legacy (osu!stable) score.
    /// </summary>
    public bool IsLegacyScore { get; set; }

    private Dictionary<HitResult, int>? statistics;

    //[Ignored]
    //public Dictionary<HitResult, int> Statistics
    //{
    //    get
    //    {
    //        if (statistics != null)
    //            return statistics;

    //        if (!string.IsNullOrEmpty(StatisticsJson))
    //            statistics = JsonCon.DeserializeObject<Dictionary<HitResult, int>>(StatisticsJson);

    //        return statistics ??= new Dictionary<HitResult, int>();
    //    }
    //    set => statistics = value;
    //}

    private Dictionary<HitResult, int>? maximumStatistics;

    [Ignored]
    public Dictionary<HitResult, int> MaximumStatistics
    {
        get
        {
            if (maximumStatistics != null)
                return maximumStatistics;

            if (!string.IsNullOrEmpty(MaximumStatisticsJson))
                maximumStatistics = JsonSerializer.Deserialize<Dictionary<HitResult, int>>(MaximumStatisticsJson);

            return maximumStatistics ??= new Dictionary<HitResult, int>();
        }
        set => maximumStatistics = value;
    }

    private Mod[]? mods;

    //[Ignored]
    //public Mod[] Mods
    //{
    //    get
    //    {
    //        if (mods != null)
    //            return mods;

    //        return APIMods.Select(m => m.ToMod(Ruleset.CreateInstance())).ToArray();
    //    }
    //    set
    //    {
    //        clearAllMods();
    //        mods = value;
    //        updateModsJson();
    //    }
    //}

    //private APIMod[]? apiMods;
}
