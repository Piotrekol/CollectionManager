namespace CollectionManager.Extensions.Modules.OsuUrlParser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Parser
{
    private static readonly Regex beatmapSetRegex = new(@"(?:https:\/\/osu.ppy.sh\/s\/)([\d]{0,10})", RegexOptions.Compiled);
    private static readonly Regex beatmapIdRegex = new(@"(?:https:\/\/osu.ppy.sh\/b\/)([\d]{0,10})", RegexOptions.Compiled);

    private static bool LineIsCollectionName(string line) =>
        !string.IsNullOrWhiteSpace(line) &&
        line.Count(l => l is ' ' or '\t' or '\'' or '!') < 2 && //arbitary number that works 100% out of 50% times!
        !line.Contains("osu.ppy.sh/");
    public enum MapIdType { Map, Set, None }

    public class TextProcessorLinkResult
    {
        public int Id { get; set; }
        public MapIdType IdType { get; set; }
        public string Line { get; set; }
    }

    public static Dictionary<string, List<TextProcessorLinkResult>> ParseLines(List<string> lines)
    {
        Dictionary<string, List<TextProcessorLinkResult>> ret = [];
        string currentCollectionName = "ParsedText";
        ret[currentCollectionName] = [];
        foreach (string line in lines)
        {
            if (LineIsCollectionName(line.Trim()))
            {
                currentCollectionName = line.Trim();
                if (!ret.ContainsKey(currentCollectionName))//In case collection names are repeated in list.
                {
                    ret[currentCollectionName] = [];
                }
            }
            else
            {
                ret[currentCollectionName].Add(ExtractMapLink(line));
            }
        }

        if (ret["ParsedText"].Count == 0)
        {
            _ = ret.Remove("ParsedText");
        }

        return ret;
    }

    public static TextProcessorLinkResult ExtractMapLink(string line)
    {
        string mapId = GetParsedLine(line, beatmapIdRegex);
        if (mapId != string.Empty)
        {
            return new TextProcessorLinkResult { Id = Convert.ToInt32(mapId), IdType = MapIdType.Map, Line = line }; //Since regex matched a number there's no need for int.TryParse
        }

        mapId = GetParsedLine(line, beatmapSetRegex);
        if (mapId != string.Empty)
        {
            return new TextProcessorLinkResult { Id = Convert.ToInt32(mapId), IdType = MapIdType.Set, Line = line }; //Same as above
        }

        return new TextProcessorLinkResult { IdType = MapIdType.None, Line = line };
    }
    public static string GetParsedLine(string line, Regex regex)
    {
        Match result = regex.Match(line);

        if (!result.Success)
        {
            return string.Empty;
        }

        return result.Groups[1].Value;
    }
}
