using System;

namespace CollectionManager.DataTypes
{
    public class BeatmapExtension : Beatmap
    {
        #region ICeBeatmapProps
        public string Name { get { return this.ToString(); } }

        public bool DataDownloaded { get; set; }
        public bool LocalBeatmapMissing { get; set; }
        public bool LocalVersionDiffers { get; set; }
        public string UserComment { get; set; } = "";

        #endregion
    }
}