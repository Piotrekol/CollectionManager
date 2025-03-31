using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Utils;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Returns grouped beatmaps using MapSetId
        /// </summary>
        /// <returns>
        /// Dictionary containing MapSetId as key and corresponding beatmaps in value. 
        /// beatmaps with invalid MapSetId (less than 1) are placed under key "-1"
        /// </returns>
        public static Dictionary<int, Beatmaps> GetBeatmapSets(this ICollection collection)
            => BeatmapUtils.GetBeatmapSets(collection.AllBeatmaps());
    }
}