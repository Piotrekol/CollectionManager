namespace CollectionManager.Extensions.DataTypes;

using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;

public class ScoreSaveConditions
{
    /// <summary>
    /// Minimum pp on a score required for it to be eligible for saving.
    /// </summary>
    public double MinimumPp { get; set; } = 0;
    /// <summary>
    /// Maximum pp on a score required for it to be eligible for saving.
    /// </summary>
    public double MaximumPp { get; set; } = 2000;
    /// <summary>
    /// Minimum acc on a score required for it to be eligible for saving.
    /// </summary>
    public double MinimumAcc { get; set; } = 0;
    /// <summary>
    /// Maximum acc on a score required for it to be eligible for saving.
    /// </summary>
    public double MaximumAcc { get; set; } = 100;
    /// <summary>
    /// fetch scores that are in specified rank range
    /// </summary>
    public RankTypes RanksToGet { get; set; } = RankTypes.All;
    /// <summary>
    /// fetch scores with specified mods
    /// </summary>
    public List<Mods> ModCombinations { get; set; } = [];

    public bool IsEligibleForSaving(ApiScore score)
    {
        if (score.Pp < MinimumPp || score.Pp > MaximumPp)
        {
            return false;
        }

        if (ModCombinations.Count > 0 && ModCombinations.All(mod => score.EnabledMods != (int)mod))
        {
            return false;
        }

        switch (RanksToGet)
        {
            case RankTypes.All:
                break;
            default:
                break;
                /*case RankTypes.SAndBetter:
                    if (score.Rank.Contains("S"))
                        return false;
                    break;
                case RankTypes.AAndWorse:
                    if (!score.Rank.Contains("S"))
                        return false;
                    break;
               */
        }

        //acc calc...(osu! mode)
        int totalHits = score.Countmiss + score.Count50 + score.Count100 + score.Count300;
        int pointsOfHits = (score.Count50 * 50) + (score.Count100 * 100) + (score.Count300 * 300);
        double acc = pointsOfHits / (double)(totalHits * 300) * 100;
        acc = Math.Round(acc, 2);

        if (acc < MinimumAcc || acc > MaximumAcc)
        {
            return false;
        }

        return true;
    }
}

public enum RankTypes { SAndBetter = 0, AAndWorse = 1, All = 2 }