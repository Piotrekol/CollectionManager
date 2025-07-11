namespace CollectionManager.Extensions.DataTypes;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Extensions;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using System.Linq;

public class ScoreSaveConditions
{
    /// <summary>
    /// Minimum pp on a score required for it to be eligible for saving.
    /// </summary>
    public double MinimumPp { get; set; }
    /// <summary>
    /// Maximum pp on a score required for it to be eligible for saving.
    /// </summary>
    public double MaximumPp { get; set; } = 5000;
    /// <summary>
    /// Minimum acc on a score required for it to be eligible for saving.
    /// </summary>
    public double MinimumAcc { get; set; }
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

    public bool IsEligibleForSaving(ApiScore score, PlayMode gamemode)
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

        double acc = ReplayExtensions.OsuScore.CalculateAccuracy(gamemode, score.Count50, score.Count100, score.Count300, score.Countmiss, score.Countgeki, score.Countkatu);

        if (acc < MinimumAcc || acc > MaximumAcc)
        {
            return false;
        }

        return true;
    }
}

public enum RankTypes { SAndBetter = 0, AAndWorse = 1, All = 2 }