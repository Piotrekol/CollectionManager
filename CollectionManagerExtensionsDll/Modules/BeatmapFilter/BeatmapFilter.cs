using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        private bool BeatmapExtensionIsUsed = false;
        private string _lastSearchString = String.Empty;
        private Dictionary<string, Scores> _scores = new Dictionary<string, Scores>();


        public BeatmapFilter(Beatmaps beatmaps, Scores scores, Beatmap baseBeatmap)
        {
            BeatmapExtensionIsUsed = baseBeatmap.GetType().IsAssignableFrom(typeof(BeatmapExtension));
            SetScores(scores);
            SetBeatmaps(beatmaps);
        }

        public void SetBeatmaps(Beatmaps beatmaps)
        {
            _beatmaps = beatmaps;
            UpdateSearch(_lastSearchString);
        }

        public void SetScores(Scores scores)
        {
            _scores.Clear();
            foreach (var score in scores)
            {
                if (!_scores.ContainsKey(score.MapHash))
                    _scores[score.MapHash] = new Scores { score };
                else
                    _scores[score.MapHash].Add(score);
            }
        }
        public void UpdateSearch(string searchString)
        {
            _lastSearchString = searchString;
            searchString = searchString.ToLower();
            string[] words = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (_beatmaps == null)
                return;
            lock (_beatmaps)
            {
                foreach (var beatmap in _beatmaps)
                {
                    BeatmapHashHidden[beatmap.Md5] = false;
                }

                if (!words.Any(s => s.StartsWith("mods")))
                {
                    CurrentMods = Mods.Nm;
                }

                foreach (string w in words)
                {
                    searchFilter filter = GetSearchFilter(w);
                    if (filter == null)
                        continue;

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

        public Mods CurrentMods { get; private set; } = Mods.Nm;
        public PlayMode CurrentPlayMode { get; private set; } = PlayMode.Osu;
        private double GetStars(Beatmap b) => b.Stars(CurrentPlayMode, CurrentMods);

        private Score GetTopScore(string mapHash)
            => GetScores(mapHash)?
                .Aggregate((first, second)
                    => first.TotalScore > second.TotalScore ? first : second)
                ?? null;

        private Scores GetScores(string mapHash)
            => _scores.ContainsKey(mapHash)
                ? _scores[mapHash]
                : null;

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
                        //beatmap keys
                        case "star":
                        case "stars":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round(GetStars(b), 2), op, num); };

                        case "cs":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.CircleSize, 1), op, num) && b.PlayMode != PlayMode.OsuMania && b.PlayMode != PlayMode.Taiko; };
                        case "hp":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.HpDrainRate, 1), op, num); };
                        case "od":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.OverallDifficulty, 1), op, num); };
                        case "ar":
                            return delegate (Beatmap b) { return isPatternMatch(Math.Round((double)b.ApproachRate, 1), op, num) && b.PlayMode != PlayMode.OsuMania && b.PlayMode != PlayMode.Taiko; };

                        case "key":
                        case "keys":
                            return delegate (Beatmap b) { return b.PlayMode == PlayMode.OsuMania && isPatternMatch(b.CircleSize, op, num); };

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
                        case "objects":
                            return delegate (Beatmap b) { return isPatternMatch(b.Circles + b.Sliders + b.Spinners, op, num); };
                        case "circles":
                            return delegate (Beatmap b) { return isPatternMatch(b.Circles, op, num); };
                        case "sliders":
                            return delegate (Beatmap b) { return isPatternMatch(b.Sliders, op, num); };
                        case "spinners":
                            return delegate (Beatmap b) { return isPatternMatch(b.Spinners, op, num); };

                        //Score keys
                        case "miss":
                        case "misses":
                            return b => isScorePatternMatch(b.Md5, (s) => s.Miss, op, num);
                        case "c300":
                        case "count300":
                            return b => isScorePatternMatch(b.Md5, (s) => s.C300, op, num);
                        case "c100":
                        case "count100":
                            return b => isScorePatternMatch(b.Md5, (s) => s.C100, op, num);
                        case "c50":
                        case "count50":
                            return b => isScorePatternMatch(b.Md5, (s) => s.C50, op, num);
                        case "combo":
                            return b => isScorePatternMatch(b.Md5, (s) => s.MaxCombo, op, num);
                        case "perfect":
                            return b => isScorePatternMatch(b.Md5, (s) => s.Perfect ? 1 : 0, op, num);
                        case "hasscore":
                            return b =>
                            {
                                var topScore = GetTopScore(b.Md5);
                                return isPatternMatch(topScore == null ? 0 : 1, op, num);
                            };
                    }
                }

                switch (key)
                {
                    case "mods":
                        CurrentMods = ToMods(val);
                        return null;
                    case "artist":
                        var artist = val.Replace(SpaceReplacement, " ");
                        return delegate (Beatmap b) { return isArtistMatch(b, artist); };
                    case "title":
                        var title = val.Replace(SpaceReplacement, " ");
                        return delegate (Beatmap b) { return isTitleMatch(b, title); };
                    case "creator":
                        var creator = val.Replace(SpaceReplacement, " ");
                        return delegate (Beatmap b) { return b.Creator.IndexOf(creator, StringComparison.CurrentCultureIgnoreCase) >= 0; };
                    case "unplayed":
                        if (String.IsNullOrEmpty(val))
                            return delegate { return RetFalse(); };
                        break;
                    case "mode":
                        num = descriptorToNum(val, ModePairs);
                        CurrentPlayMode = (PlayMode)num;
                        return delegate (Beatmap b) { return isPatternMatch((double)b.PlayMode, op, num); };
                    case "status":
                        num = descriptorToNum(val, StatusPairs);

                        return delegate (Beatmap b) { return isPatternMatch((double)b.State, op, num); };
                    case "hasscoreswithmod":
                    case "hasscorewithmod":
                    case "hasscoreswithmods":
                    case "hasscorewithmods":
                        var mods = (int)ToMods(val);
                        return b =>
                        {
                            var hasScoresWithMathingMods = GetScores(b.Md5)?.Any(s => (s.Mods & mods) != 0);
                            return hasScoresWithMathingMods ?? false;
                        };
                }
            }
            int id;
            if (Int32.TryParse(searchWord, out id))
            {
                if (BeatmapExtensionIsUsed)
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
                else
                {
                    return delegate (Beatmap b)
                    {
                        //match mapid and mapset id while input is numbers.
                        if (b.MapId == id) return true;
                        if (b.MapSetId == id) return true;
                        if (b.ThreadId == id) return true;
                        return isWordMatch(b, searchWord);
                    };
                }
            }
            if (BeatmapExtensionIsUsed)
                return delegate (Beatmap b) { return isWordMatch((BeatmapExtension)b, searchWord); };
            else
                return delegate (Beatmap b) { return isWordMatch(b, searchWord); };

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
            new KeyValuePair<double, string> ((double)PlayMode.Osu, "osu!"),
            new KeyValuePair<double, string> ((double)PlayMode.Taiko, "taiko"),
            new KeyValuePair<double, string> ((double)PlayMode.CatchTheBeat, "catchthebeat"),
            new KeyValuePair<double, string> ((double)PlayMode.CatchTheBeat, "ctb"),
            new KeyValuePair<double, string> ((double)PlayMode.OsuMania, "osu!mania"),
            new KeyValuePair<double, string> ((double)PlayMode.OsuMania, "osumania"),
            new KeyValuePair<double, string> ((double)PlayMode.OsuMania, "mania"),
            new KeyValuePair<double, string> ((double)PlayMode.OsuMania, "o!m"),
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
        private bool isWordMatch(Beatmap b, string word)
        {
            if (b.ToString(true).IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Creator.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Tags.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.Source.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.ArtistUnicode != null && b.ArtistUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            if (b.TitleUnicode != null && b.TitleUnicode.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            return false;
        }
        private bool isWordMatch(BeatmapExtension b, string word)
        {
            return isWordMatch((Beatmap)b, word) ||
                   (b.UserComment != null &&
                   b.UserComment.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0);
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

        private bool isScorePatternMatch<T>(string mapHash, Func<Score, T> propGetter, string op, T num) where T : IComparable<T>
        {
            var score = GetTopScore(mapHash);
            return score != null && isPatternMatch(propGetter(score), op, num);
        }

        private static Mods ToMods(string shorthandMods)
        {
            var splitMods = Regex.Split(shorthandMods, @"([A-Za-z]{2})").Where(s => !string.IsNullOrEmpty(s)).ToList();
            Mods mods = Mods.Nm;

            foreach (var mod in splitMods)
            {
                if (Enum.TryParse(mod, true, out Mods parsedMod))
                    mods |= parsedMod;
            }

            return mods;
        }
    }
}