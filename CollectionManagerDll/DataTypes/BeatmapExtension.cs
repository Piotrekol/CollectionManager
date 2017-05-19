using System;

namespace CollectionManager.DataTypes
{
    public class BeatmapExtension : Beatmap
    {

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Artist) && string.IsNullOrEmpty(Title))
                return Md5;
            var baseStr = Artist + " - " + Title;
            return baseStr;
        }
        public string ToString(bool withDiff)
        {
            if (withDiff)
            {
                return ToString() + " [" + DiffName + "]";
            }
            else return ToString();
        }

        private string _artist;
        public string Artist
        {
            get
            {
                if (_artist == null)
                {
                    if (!string.IsNullOrEmpty(ArtistRoman))
                        _artist = ArtistRoman;
                    else if (!string.IsNullOrEmpty(ArtistUnicode))
                        _artist = ArtistUnicode;
                    else
                        _artist = "";
                }
                return _artist;
            }
        }
        private string _title;
        public string Title
        {
            get
            {
                if (_title == null)
                {
                    if (!string.IsNullOrEmpty(TitleRoman))
                        _title = TitleRoman;
                    else if (!string.IsNullOrEmpty(TitleUnicode))
                        _title = TitleUnicode;
                    else
                        _title = "";
                }
                return _title;
            }
        }

        #region ICeBeatmapProps
        public string Name { get { return this.ToString(); } }

        public bool DataDownloaded { get; set; }
        public bool LocalBeatmapMissing { get; set; }
        public bool LocalVersionDiffers { get; set; }
        public string UserComment { get; set; } = "";

        #endregion
    }
}