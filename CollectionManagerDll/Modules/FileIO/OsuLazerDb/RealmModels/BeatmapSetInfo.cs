﻿using CollectionManager.Annotations;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
[MapTo("BeatmapSet")]
internal partial class BeatmapSetInfo
    : IRealmObject
{
    [PrimaryKey]
    public Guid ID { get; set; }

    [Indexed]
    public int OnlineID { get; set; } = -1;

    public DateTimeOffset DateAdded { get; set; }

    /// <summary>
    /// The date this beatmap set was first submitted.
    /// </summary>
    public DateTimeOffset? DateSubmitted { get; set; }

    /// <summary>
    /// The date this beatmap set was ranked.
    /// </summary>
    public DateTimeOffset? DateRanked { get; set; }

    // [JsonIgnore]
    // public IBeatmapMetadataInfo Metadata => Beatmaps.FirstOrDefault()?.Metadata ?? new BeatmapMetadata();

    public IList<BeatmapInfo> Beatmaps { get; } = null!;

    public IList<RealmNamedFileUsage> Files { get; } = null!;

    [Ignored]
    public BeatmapOnlineStatus Status
    {
        get => (BeatmapOnlineStatus)StatusInt;
        set => StatusInt = (int)value;
    }

    [MapTo(nameof(Status))]
    public int StatusInt { get; set; } = (int)BeatmapOnlineStatus.None;

    public bool DeletePending { get; set; }

    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Whether deleting this beatmap set should be prohibited (due to it being a system requirement to be present).
    /// </summary>
    public bool Protected { get; set; }

    public double MaxStarDifficulty => Beatmaps.Count == 0 ? 0 : Beatmaps.Max(b => b.StarRating);

    public double MaxLength => Beatmaps.Count == 0 ? 0 : Beatmaps.Max(b => b.Length);

    public double MaxBPM => Beatmaps.Count == 0 ? 0 : Beatmaps.Max(b => b.BPM);
    public double MinBPM => Beatmaps.Count == 0 ? 0 : Beatmaps.Min(b => b.BPM);

    //public BeatmapSetInfo(IEnumerable<BeatmapInfo>? beatmaps = null)
    //    : this()
    //{
    //    ID = Guid.NewGuid();
    //    if (beatmaps != null)
    //        Beatmaps.AddRange(beatmaps);
    //}

    [UsedImplicitly] // Realm
    private BeatmapSetInfo()
    {
    }

    public bool Equals(BeatmapSetInfo? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other == null) return false;

        return ID == other.ID;
    }

    //public override string ToString() => Metadata.GetDisplayString();

    //public bool Equals(IBeatmapSetInfo? other) => other is BeatmapSetInfo b && Equals(b);

    //IEnumerable<IBeatmapInfo> IBeatmapSetInfo.Beatmaps => Beatmaps;

    //IEnumerable<INamedFileUsage> IHasNamedFiles.Files => Files;

    //public bool AllBeatmapsUpToDate => Beatmaps.All(b => b.MatchesOnlineVersion);
}
