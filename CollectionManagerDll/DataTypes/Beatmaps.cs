using System;
using System.Collections.Generic;

namespace CollectionManager.DataTypes
{
    public class Beatmaps : RangeObservableCollection<Beatmap>
    {
        public Beatmaps()
            : base() { }

        public Beatmaps(IEnumerable<Beatmap> collection)
            : base(collection) { }
    }
}