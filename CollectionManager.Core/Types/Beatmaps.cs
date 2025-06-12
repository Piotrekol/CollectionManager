namespace CollectionManager.Core.Types;

using System.Collections.Generic;

public class Beatmaps : RangeObservableCollection<Beatmap>
{
    public Beatmaps()
        : base() { }

    public Beatmaps(IEnumerable<Beatmap> collection)
        : base(collection) { }
}