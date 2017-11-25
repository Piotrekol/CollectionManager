using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Enums;
using MoreLinq;

namespace CollectionManagerExtensionsDll.DataTypes
{
    public class OsuResult : EventArgs
    {
        public MsnResult Msn { get; set; }
        public List<Beatmap> Beatmaps { get; set; }
        private Beatmap _beatmap;

        public Beatmap Beatmap
        {
            get
            {
                if (_beatmap != null)
                    return _beatmap;
                _beatmap = Beatmaps != null && Beatmaps.Count > 0 ? Beatmaps.MaxBy(t=>t.StarsNomod) : null;
                return _beatmap;
            }
        }
        public BeatmapSource BeatmapSource { get; set; }
    }
}