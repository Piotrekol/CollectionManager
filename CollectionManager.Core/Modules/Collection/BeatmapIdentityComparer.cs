namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Types;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using System.Collections.Generic;

/// <summary>
/// Beatmap identity comparer which compares beatmaps by their MapId when above the <see cref="MapCacher.InvalidMapIdThreshold"/> or its Hash.
/// </summary>
internal sealed class BeatmapIdentityComparer : IEqualityComparer<BeatmapExtension>
{
    public static BeatmapIdentityComparer Instance { get; } = new();

    public bool Equals(BeatmapExtension x, BeatmapExtension y) =>
        x is not null && y is not null && KeyOf(x) == KeyOf(y);

    public int GetHashCode(BeatmapExtension obj) => KeyOf(obj).GetHashCode();

    internal static string KeyOf(BeatmapExtension beatmap) =>
        beatmap.MapId > MapCacher.InvalidMapIdThreshold ? $"id:{beatmap.MapId}" : $"h:{beatmap.Hash}";
}
