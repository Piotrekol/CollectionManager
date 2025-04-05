namespace CollectionManager.Extensions.Modules.Filter;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class BeatmapFilter
{
    public const string SpaceReplacement = "!._!";
    private Beatmaps _beatmaps;
    public Dictionary<string, bool> BeatmapHashHidden = [];

    private delegate bool searchFilter(Beatmap m);
    internal static readonly NumberFormatInfo nfi = new CultureInfo(@"en-US", false).NumberFormat;

    private static readonly Regex regComparison = new(@"^(\w*)([<>=]=?|!=)(.*)$");
    private static readonly Regex regNumber = new(@"(?x)
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

    private readonly bool BeatmapExtensionIsUsed;
    private string _lastSearchString = string.Empty;
    private readonly Dictionary<string, Scores> _scores = [];

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
        foreach (Core.Interfaces.IReplay score in scores)
        {
            if (!_scores.TryGetValue(score.MapHash, out Scores value))
            {
                _scores[score.MapHash] = [score];
            }
            else
            {
                value.Add(score);
            }
        }
    }
    public void UpdateSearch(string searchString)
    {
        _lastSearchString = searchString;
        searchString = searchString.ToLower(CultureInfo.CurrentCulture);
        string[] words = searchString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (_beatmaps == null)
        {
            return;
        }

        lock (_beatmaps)
        {
            foreach (Beatmap beatmap in _beatmaps)
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
                {
                    continue;
                }

                foreach (Beatmap b in _beatmaps)
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

    private IReplay GetTopScore(string mapHash)
        => GetScores(mapHash)?
            .Aggregate((first, second)
                => first.TotalScore > second.TotalScore ? first : second)
            ?? null;

    private Scores GetScores(string mapHash)
        => _scores.TryGetValue(mapHash, out Scores value)
            ? value : null;

    /// <summary>
    /// Returns beatmapFilter delegate for specified searchWord.
    /// Unimplemented: key/keys/speed/unplayed
    ///  </summary>
    /// <param name="searchWord"></param>
    /// <returns></returns>
    private searchFilter GetSearchFilter(string searchWord)
    {
        Match match = regComparison.Match(searchWord);
        if (match.Success)
        {
            string key = match.Groups[1].Value.ToLower(CultureInfo.CurrentCulture);
            string op = match.Groups[2].Value;
            string val = match.Groups[3].Value.ToLower(CultureInfo.CurrentCulture);
            if (op == @"=")
            {
                op = @"==";
            }

            double num;

            Match matchNum = regNumber.Match(val);
            if (matchNum.Success)
            {
                num = double.Parse(matchNum.Groups[0].Value, nfi);
                switch (key)
                {
                    //beatmap keys
                    case "star":
                    case "stars":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round(GetStars(b), 2), op, num); };

                    case "cs":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round((double)b.CircleSize, 1), op, num) && b.PlayMode != PlayMode.OsuMania && b.PlayMode != PlayMode.Taiko; };
                    case "hp":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round((double)b.HpDrainRate, 1), op, num); };
                    case "od":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round((double)b.OverallDifficulty, 1), op, num); };
                    case "ar":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round((double)b.ApproachRate, 1), op, num) && b.PlayMode != PlayMode.OsuMania && b.PlayMode != PlayMode.Taiko; };

                    case "key":
                    case "keys":
                        return delegate (Beatmap b)
                        { return b.PlayMode == PlayMode.OsuMania && isPatternMatch(b.CircleSize, op, num); };

                    case "speed":
                        return delegate (Beatmap b)
                        { return RetFalse(); };

                    case "bpm":
                        return delegate (Beatmap b)
                        { return isPatternMatch(Math.Round(b.MinBpm), op, num); };
                    case "length":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.TotalTime / 1000, op, num); };
                    case "drain":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.DrainingTime, op, num); };

                    case "played":
                        return delegate (Beatmap b)
                        { return b.LastPlayed.HasValue && isPatternMatch((DateTime.Now - b.LastPlayed.Value).Days, op, num); };
                    case "objects":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.Circles + b.Sliders + b.Spinners, op, num); };
                    case "circles":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.Circles, op, num); };
                    case "sliders":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.Sliders, op, num); };
                    case "spinners":
                        return delegate (Beatmap b)
                        { return isPatternMatch(b.Spinners, op, num); };

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
                            IReplay topScore = GetTopScore(b.Md5);
                            return isPatternMatch(topScore == null ? 0 : 1, op, num);
                        };
                    case "haslocalactivity":
                        return b =>
                        {
                            bool hasTopScore = GetTopScore(b.Md5) != null;
                            bool hasRank = b.OsuGrade != OsuGrade.Null || b.CatchGrade != OsuGrade.Null || b.TaikoGrade != OsuGrade.Null || b.ManiaGrade != OsuGrade.Null;
                            bool hasValidLastPlayed = b.LastPlayed.HasValue && b.LastPlayed.Value > DateTimeOffset.MinValue;

                            if (num == 0)
                            {
                                return isPatternMatch(hasTopScore == false ? 0 : 1, op, num)
                                       && isPatternMatch(hasValidLastPlayed == false ? 0 : 1, op, num)
                                       && isPatternMatch(hasRank == false ? 0 : 1, op, num);
                            }

                            return isPatternMatch(hasTopScore == false ? 0 : 1, op, num)
                                   || isPatternMatch(hasValidLastPlayed == false ? 0 : 1, op, num)
                                   || isPatternMatch(hasRank == false ? 0 : 1, op, num);
                        };
                }
            }

            switch (key)
            {
                case "mods":
                    CurrentMods = ToMods(val);
                    return null;
                case "artist":
                    string artist = val.Replace(SpaceReplacement, " ");
                    return delegate (Beatmap b)
                    { return isArtistMatch(b, artist); };
                case "title":
                    string title = val.Replace(SpaceReplacement, " ");
                    return delegate (Beatmap b)
                    { return isTitleMatch(b, title); };
                case "creator":
                    string creator = val.Replace(SpaceReplacement, " ");
                    return delegate (Beatmap b)
                    { return b.Creator.Contains(creator, StringComparison.CurrentCultureIgnoreCase); };
                case "mode":
                    num = descriptorToNum(val, ModePairs);
                    CurrentPlayMode = (PlayMode)num;
                    return delegate (Beatmap b)
                    { return isPatternMatch((double)b.PlayMode, op, num); };
                case "status":
                    num = descriptorToNum(val, StatusPairs);

                    return delegate (Beatmap b)
                    { return isPatternMatch(b.State, op, num); };
                case "hasscoreswithmod":
                case "hasscorewithmod":
                case "hasscoreswithmods":
                case "hasscorewithmods":
                    int mods = (int)ToMods(val);
                    return b =>
                    {
                        bool? hasScoresWithMathingMods = GetScores(b.Md5)?.Any(s => (s.Mods & mods) != 0);
                        return hasScoresWithMathingMods ?? false;
                    };
            }
        }

        if (int.TryParse(searchWord, out int id))
        {
            if (BeatmapExtensionIsUsed)
            {
                return delegate (Beatmap b)
                {
                    //match mapid and mapset id while input is numbers.
                    if (b.MapId == id)
                    {
                        return true;
                    }

                    if (b.MapSetId == id)
                    {
                        return true;
                    }

                    if (b.ThreadId == id)
                    {
                        return true;
                    }

                    return isWordMatch((BeatmapExtension)b, searchWord);
                };
            }
            else
            {
                return delegate (Beatmap b)
                {
                    //match mapid and mapset id while input is numbers.
                    if (b.MapId == id)
                    {
                        return true;
                    }

                    if (b.MapSetId == id)
                    {
                        return true;
                    }

                    if (b.ThreadId == id)
                    {
                        return true;
                    }

                    return isWordMatch(b, searchWord);
                };
            }
        }

        if (BeatmapExtensionIsUsed)
        {
            return delegate (Beatmap b)
            { return isWordMatch((BeatmapExtension)b, searchWord); };
        }
        else
        {
            return delegate (Beatmap b)
            { return isWordMatch(b, searchWord); };
        }
    }

    private static readonly KeyValuePair<double, string>[] StatusPairs = new KeyValuePair<double, string>[]
    {
        new((double)SubmissionStatus.Unknown, "unknown"),
        new((double)SubmissionStatus.NotSubmitted, "notsubmitted"),
        new((double)SubmissionStatus.Pending, "pending"),
        new((double)SubmissionStatus.Ranked, "ranked"),
        new((double)SubmissionStatus.Approved, "approved"),
        new((double)SubmissionStatus.Qualified, "qualified"),
        new((double)SubmissionStatus.Loved, "loved"),
    };
    private static readonly KeyValuePair<double, string>[] ModePairs = new KeyValuePair<double, string>[]
    {
        new((double)PlayMode.Osu, "osu!"),
        new((double)PlayMode.Taiko, "taiko"),
        new((double)PlayMode.CatchTheBeat, "catchthebeat"),
        new((double)PlayMode.CatchTheBeat, "ctb"),
        new((double)PlayMode.OsuMania, "osu!mania"),
        new((double)PlayMode.OsuMania, "osumania"),
        new((double)PlayMode.OsuMania, "mania"),
        new((double)PlayMode.OsuMania, "o!m"),
    };
    private static readonly char[] separator = new char[] { ' ' };

    private static double descriptorToNum(string got, KeyValuePair<double, string>[] pairs)
    {
        if (got.Length == 0)
        {
            return double.NaN;
        }

        foreach (KeyValuePair<double, string> want in pairs)
        {
            if (want.Value.StartsWith(got))
            {
                return want.Key;
            }
        }

        return double.NaN;
    }

    private static bool RetFalse() => false;
    private static bool isWordMatch(Beatmap b, string word)
    {
        if (b.ToString(true).Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.Creator.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.Tags.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.Source.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.ArtistUnicode != null && b.ArtistUnicode.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.TitleUnicode != null && b.TitleUnicode.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }
    private static bool isWordMatch(BeatmapExtension b, string word) => isWordMatch((Beatmap)b, word) ||
               (b.UserComment != null &&
               b.UserComment.Contains(word, StringComparison.CurrentCultureIgnoreCase));

    private static bool isArtistMatch(Beatmap b, string word)
    {
        if (b.ArtistUnicode != null && b.ArtistUnicode.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.ArtistRoman != null && b.ArtistRoman.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }
    private static bool isTitleMatch(Beatmap b, string word)
    {
        if (b.TitleUnicode != null && b.TitleUnicode.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        if (b.TitleRoman != null && b.TitleRoman.Contains(word, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        return false;
    }
    private static bool isPatternMatch<T>(T left, string op, T right) where T : IComparable<T>
    {
        int cmp = left.CompareTo(right);
        return op switch
        {
            @"<" => cmp < 0,
            @">" => cmp > 0,
            "==" => cmp == 0,
            ">=" => cmp >= 0,
            "<=" => cmp <= 0,
            "!=" => cmp != 0,
            _ => false,
        };
    }

    private bool isScorePatternMatch<T>(string mapHash, Func<IReplay, T> propGetter, string op, T num) where T : IComparable<T>
    {
        IReplay score = GetTopScore(mapHash);
        return score != null && isPatternMatch(propGetter(score), op, num);
    }

    private static Mods ToMods(string shorthandMods)
    {
        List<string> splitMods = Regex.Split(shorthandMods, @"([A-Za-z]{2})").Where(s => !string.IsNullOrEmpty(s)).ToList();
        Mods mods = Mods.Nm;

        foreach (string mod in splitMods)
        {
            if (Enum.TryParse(mod, true, out Mods parsedMod))
            {
                mods |= parsedMod;
            }
        }

        return mods;
    }
}