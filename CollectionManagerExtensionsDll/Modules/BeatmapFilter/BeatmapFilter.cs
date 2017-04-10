using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManagerExtensionsDll.Modules.BeatmapFilter
{
    public class BeatmapFilter
    {
        public const string SpaceReplacement = "!._!";
        private Beatmaps _beatmaps;
        public Dictionary<string, bool> BeatmapHashHidden = new Dictionary<string, bool>();

        private delegate bool searchFilter(Beatmap m);
        internal static readonly NumberFormatInfo nfi = new CultureInfo(@"en-US", false).NumberFormat;

        private static Regex regComparison = new Regex(@"^(\w*)([<>=]=?|!=)(.*)$");
        private static Regex regNumber = new Regex
        (@"(?x)
            ^
            [+-]?
            (?:
                (?>\d+) \.?
                |
                \. \d
            )
            \d*
            (?: e [+-]? \d+ )?  
            $
        ");

        public BeatmapFilter(Beatmaps beatmaps)
        {
            SetBeatmaps(beatmaps);
        }

        public void SetBeatmaps(Beatmaps beatmaps)
        {
            _beatmaps = beatmaps;
        }
        public void UpdateSearch(string searchString)
        {
            searchString = searchString.ToLower();
            string[] words = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            lock (_beatmaps)
            {
                foreach (var beatmap in _beatmaps)
                {
                    BeatmapHashHidden[beatmap.Md5] = false;
                }
                foreach (string w in words)
                {
                    searchFilter filter = GetSearchFilter(w);

                    foreach (var b in _beatmaps)
                    {
                        if (!BeatmapHashHidden[b.Md5] && !filter(b))
                        {
                            BeatmapHashHidden[b.Md5] = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns beatmapFilter delegate for specified searchWord.
        /// Unimplemented: key/keys/speed/played/unplayed
        ///  </summary>
        /// <param name="searchWord"></param>
        /// <returns></returns>
        private searchFilter GetSearchFilter(string searchWord)
        {
            Match match = regComparison.Match(searchWord);
            if (match.Success)
            {
                string key = match.Groups[1].Value.ToLower();
                string op = match.Groups[2].Value;
                string val = match.Groups[3].Value.ToLower();
                if (op == @"=") op = @"==";

                double num;

                Match matchNum = regNumber.Match(val);
                if (matchNum.Success)
                {
                    num = Double.Parse(matchNum.Groups[0].Value, nfi);
                    switch (key)
                    {

                        case "star":
                        case "stars":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round(b.StarsNomod, 2), op, num); };

                        case "cs":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.CircleSize, 1), op, num) && b.PlayMode != PlayModes.OsuMania && b.PlayMode != PlayModes.Taiko; };
                        case "hp":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.HpDrainRate, 1), op, num); };
                        case "od":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.OverallDifficulty, 1), op, num); };
                        case "ar":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.ApproachRate, 1), op, num) && b.PlayMode != PlayModes.OsuMania && b.PlayMode != PlayModes.Taiko; };

                        case "key":
                        case "keys":
                            return delegate (Beatmap b) { return RetFalse(); };

                        case "speed":
                            return delegate (Beatmap b) { return RetFalse(); };

                        case "bpm":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round(b.MinBpm), op, num); };
                        case "length":
                            return delegate (Beatmap b) { return isPatternMatch(b.TotalTime / 1000, op, num); };
                        case "drain":
                            return delegate (Beatmap b) { return isPatternMatch(b.DrainingTime, op, num); };

                        case "played":
                            break;
                    }
                }

                switch (key)
                {
                    case "artist":
                        var artist = val.Replace(SpaceReplacement, " ");
                        return delegate (Beatmap b) { return isArtistMatch(b, artist); };
                    case "title":
                        var title = val.Replace(SpaceReplacement, " ");
                        return delegate (Beatmap b) { return isTitleMatch(b, title); };
                    case "unplayed":
                        if (String.IsNullOrEmpty(val))
                            return delegate { return RetFalse(); };
                        break;
                    case "mode":
                        num = descriptorToNum(val, ModePairs);
                        return delegate (Beatmap b) { return isPatternMatch((double)b.PlayMode, op, num); };
                    case "status":
                        num = descriptorToNum(val, StatusPairs);
                        
                        return delegate (Beatmap b) { return isPatternMatch((double)b.State, op, num); };
                }
            }
            int id;
            if (Int32.TryParse(searchWord, out id))
            {
                return delegate (Beatmap b)
                {
                    //match mapid and mapset id while input is numbers.
                    if (b.MapId == id) return true;
                    if (b.MapSetId == id) return true;
                    if (b.ThreadId == id) return true;
                    return isWordMatch((BeatmapExtension)b, searchWord);
                };
            }
            return delegate (Beatmap b) { return isWordMatch((BeatmapExtension)b, searchWord); };
        }

        private static readonly KeyValuePair<double, string>[] StatusPairs = new KeyValuePair<double, string>[]
        {
            new KeyValuePair<double, string> ((double)SubmissionStatus.Unknown, "unknown"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.NotSubmitted, "notsubmitted"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.Pending, "pending"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.Ranked, "ranked"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.Approved, "approved"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.Qualified, "qualified"),
            new KeyValuePair<double, string> ((double)SubmissionStatus.Loved, "loved"),
        };
        private static readonly KeyValuePair<double, string>[] ModePairs = new KeyValuePair<double, string>[]
        {
            new KeyValuePair<double, string> ((double)PlayModes.Osu, "osu!"),
            new KeyValuePair<double, string> ((double)PlayModes.Taiko, "taiko"),
            new KeyValuePair<double, string> ((double)PlayModes.CatchTheBeat, "catchthebeat"),
            new KeyValuePair<double, string> ((double)PlayModes.CatchTheBeat, "ctb"),
            new KeyValuePair<double, string> ((double)PlayModes.OsuMania, "osu!mania"),
            new KeyValuePair<double, string> ((double)PlayModes.OsuMania, "osumania"),
            new KeyValuePair<double, string> ((double)PlayModes.OsuMania, "mania"),
            new KeyValuePair<double, string> ((double)PlayModes.OsuMania, "o!m"),
        };



        public enum SubmissionStatus
        {
            Unknown,
            NotSubmitted,
            Pending,
            EditableCutoff,
            Ranked,
            Approved,
            Qualified,
            Loved
        }
        private static double descriptorToNum(string got, KeyValuePair<double, string>[] pairs)
        {
            if (got.Length == 0)
                return Double.NaN;

            foreach (KeyValuePair<double, string> want in pairs)
            {
                if (want.Value.StartsWith(got))
                    return want.Key;
            }

            return Double.NaN;
        }

        private bool RetFalse()
        {
            return false;
        }
        private bool isWordMatch(BeatmapExtension b, string word)
        {
            if (b.ToString(true).IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Creator.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Tags.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Source.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.ArtistUnicode != null && b.ArtistUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.TitleUnicode != null && b.TitleUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            return false;
        }

        private bool isArtistMatch(Beatmap b, string word)
        {
            if (b.ArtistUnicode != null && b.ArtistUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.ArtistRoman != null && b.ArtistRoman.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            return false;
        }
        private bool isTitleMatch(Beatmap b, string word)
        {
            if (b.TitleUnicode != null && b.TitleUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.TitleRoman != null && b.TitleRoman.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            return false;
        }
        private bool isPatternMatch<T>(T left, string op, T right) where T : IComparable<T>
        {
            int cmp = left.CompareTo(right);
            switch (op)
            {
                case @"<":
                    return cmp < 0;
                case @">":
                    return cmp > 0;
                case "==":
                    return cmp == 0;
                case ">=":
                    return cmp >= 0;
                case "<=":
                    return cmp <= 0;
                case "!=":
                    return cmp != 0;
                default:
                    return false;
            }
        }
    }
}