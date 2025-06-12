namespace CollectionManager.Extensions.Utils;

using CollectionManager.Core.Types;
using System.Collections.Generic;

public static class CollectionUtils
{
    /// <summary>
    /// Returns grouped beatmaps using MapSetId
    /// </summary>
    /// <returns>
    /// Dictionary containing MapSetId as key and corresponding beatmaps in value. 
    /// beatmaps with invalid MapSetId (less than 1) are placed under key "-1"
    /// </returns>
    public static Dictionary<int, Beatmaps> GetBeatmapSets(this IOsuCollection collection)
        => collection.AllBeatmaps().GetBeatmapSets();
}