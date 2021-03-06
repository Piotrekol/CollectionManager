﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CollectionManager.DataTypes
{
    /// <summary>
    /// Contents of this collection should be only modified by CollectionManager.
    /// Edits outside of it are not supported and things may break.
    /// </summary>
    public class Collections :RangeObservableCollection<ICollection>
    {
        public IEnumerable<BeatmapExtension> AllBeatmaps()
        {
            for (int i = 0; i < this.Count; i++)
            {
                foreach (var beatmap in this[i].AllBeatmaps())
                {
                    yield return beatmap;
                }
            }
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }

        protected override event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            remove { base.PropertyChanged -= value; }
        }

    }
}