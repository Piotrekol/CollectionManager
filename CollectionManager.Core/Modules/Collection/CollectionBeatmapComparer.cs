namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Types;
using System.Collections.Generic;

internal class CollectionBeatmapComparer : IEqualityComparer<BeatmapExtension>
{
    public bool Equals(BeatmapExtension x, BeatmapExtension y) => x != null && y != null && x.Md5 == y.Md5 && x.MapId == y.MapId;

    public int GetHashCode(BeatmapExtension obj)
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + obj.Md5.GetHashCode();
            return (hash * 23) + obj.MapId.GetHashCode();
        }
    }
}