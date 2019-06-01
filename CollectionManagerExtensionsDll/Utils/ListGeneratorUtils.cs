using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    internal static class CollectionUtils
    {
        /// <summary>
        /// Returns grouped beatmaps using MapSetId
        /// </summary>
        /// <returns>
        /// Dictionary containing MapSetId as key and corresponding beatmaps in value. 
        /// beatmaps with invalid MapSetId (less than 1) are placed under key "-1"
        /// </returns>
        internal static Dictionary<int, Beatmaps> GetBeatmapSets(this ICollection collection)
        {
            return GetBeatmapSets(collection.AllBeatmaps());
        }
        

        internal static Dictionary<int, Beatmaps> GetBeatmapSets(IEnumerable<Beatmap> collection)
        {
            var beatmapSets = new Dictionary<int, Beatmaps>();
            beatmapSets.Add(-1, new Beatmaps());

            foreach (var map in collection)
            {
                if (map.MapSetId < 1)
                {
                    beatmapSets[-1].Add(map);
                }
                else if (beatmapSets.ContainsKey(map.MapSetId))
                {
                    beatmapSets[map.MapSetId].Add(map);
                }
                else
                {
                    beatmapSets.Add(map.MapSetId, new Beatmaps() { map });
                }
            }
            return beatmapSets;
        }
    }
}