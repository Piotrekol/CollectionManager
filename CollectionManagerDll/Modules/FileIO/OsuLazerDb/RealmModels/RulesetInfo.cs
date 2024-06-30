﻿using CollectionManager.Annotations;
using Realms;
using System;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
[MapTo("Ruleset")]
internal partial class RulesetInfo
    : IRealmObject
{
    [PrimaryKey]
    public string ShortName { get; set; } = string.Empty;

    [Indexed]
    public int OnlineID { get; set; } = -1;

    public string Name { get; set; } = string.Empty;

    public string InstantiationInfo { get; set; } = string.Empty;

    /// <summary>
    /// Stores the last applied <see cref="DifficultyCalculator.Version"/>
    /// </summary>
    public int LastAppliedDifficultyVersion { get; set; }

    public RulesetInfo(string shortName, string name, string instantiationInfo, int onlineID)
    {
        ShortName = shortName;
        Name = name;
        InstantiationInfo = instantiationInfo;
        OnlineID = onlineID;
    }

    [UsedImplicitly]
    public RulesetInfo()
    {
    }

    public bool Available { get; set; }

    public bool Equals(RulesetInfo? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other == null) return false;

        return ShortName == other.ShortName;
    }

    public int CompareTo(RulesetInfo? other)
    {
        if (OnlineID >= 0 && other?.OnlineID >= 0)
            return OnlineID.CompareTo(other.OnlineID);

        // Official rulesets are always given precedence for the time being.
        if (OnlineID >= 0)
            return -1;
        if (other?.OnlineID >= 0)
            return 1;

        return string.Compare(ShortName, other?.ShortName, StringComparison.Ordinal);
    }

    public override string ToString() => Name;

    public RulesetInfo Clone() => new RulesetInfo
    {
        OnlineID = OnlineID,
        Name = Name,
        ShortName = ShortName,
        InstantiationInfo = InstantiationInfo,
        Available = Available,
        LastAppliedDifficultyVersion = LastAppliedDifficultyVersion,
    };
}
