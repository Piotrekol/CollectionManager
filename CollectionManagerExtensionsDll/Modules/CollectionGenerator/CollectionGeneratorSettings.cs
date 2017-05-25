using System.Collections.Generic;

namespace CollectionManagerExtensionsDll.Modules.CollectionGenerator
{
    public class CollectionGeneratorSettings
    {
        /// <summary>
        /// How many top scores should be fetched for each username.
        /// </summary>
        public int NumberOfTops { get; set; } = 100;
        /// <summary>
        /// string.format pattern target for saving collection name({0} being username)
        /// </summary>
        public string CollectionNameSavePattern { get; set; }
        /// <summary>
        /// Should collections be grouped according to mods used on player scores?
        /// </summary>
        public bool GroupByMods { get; set; }
        /// <summary>
        /// Merge collections with same names.
        /// </summary>
        public bool MergeCollections { get; set; }
        public ScoreSaveConditions ScoreSaveConditions;
    }
    public class ScoreSaveConditions
    {
        /// <summary>
        /// Minimum pp on a score required for it to be egible for saving.
        /// </summary>
        public double MinimumPp { get; set; } = 0;
        /// <summary>
        /// Maximum pp on a score required for it to be egible for saving.
        /// </summary>
        public double MaximumPp { get; set; } = 1000000;
        /// <summary>
        /// Minimum acc on a score required for it to be egible for saving.
        /// </summary>
        public double MinimumAcc { get; set; } = 0;
        /// <summary>
        /// Maximum acc on a score required for it to be egible for saving.
        /// </summary>
        public double MaximumAcc { get; set; } = 100;
        /// <summary>
        /// fetch scores that are in specified rank range
        /// </summary>
        public RankTypes RanksToGet { get; set; } = RankTypes.All;
    }
    public enum RankTypes { SAndBetter, AAndWorse, All }
}