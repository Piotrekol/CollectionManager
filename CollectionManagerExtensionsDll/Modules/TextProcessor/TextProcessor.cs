using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CollectionManagerExtensionsDll.Modules.TextProcessor
{
    public class TextProcessor
    {
        private static Regex beatmapSetRegex = new Regex(@"(?:https:\/\/osu.ppy.sh\/s\/)([\d]{0,10})", RegexOptions.Compiled);
        private static Regex beatmapIdRegex = new Regex(@"(?:https:\/\/osu.ppy.sh\/b\/)([\d]{0,10})", RegexOptions.Compiled);

        private bool LineIsCollectionName(string line) =>
            line.Count(l => l == ' ' || l == '\t') < 3; //arbitary number that works 100% out of 50% times!
        public enum MapIdType { Map, Set, None }

        public class TextProcessorLinkResult
        {
            public int Id { get; set; }
            public MapIdType IdType { get; set; }
            public string Line { get; set; }
        }

        public Dictionary<string, List<TextProcessorLinkResult>> ParseLines(List<string> lines)
        {
            var ret = new Dictionary<string, List<TextProcessorLinkResult>>();
            var currentCollectionName = "ParsedText";
            foreach (var line in lines)
            {
                if (LineIsCollectionName(line.Trim()))
                {
                    currentCollectionName = line.Trim();
                    if (!ret.ContainsKey(currentCollectionName))//In case collection names are repeated in list.
                        ret[currentCollectionName] = new List<TextProcessorLinkResult>();
                }
                else
                    ret[currentCollectionName].Add(ExtractMapLink(line));
            }

            return ret;
        }

        public TextProcessorLinkResult ExtractMapLink(string line)
        {
            var mapId = GetParsedLine(line, beatmapIdRegex);
            if (mapId != string.Empty)
                return new TextProcessorLinkResult { Id = Convert.ToInt32(mapId), IdType = MapIdType.Map, Line = line }; //Since regex matched a number there's no need for int.TryParse

            mapId = GetParsedLine(line, beatmapSetRegex);
            if (mapId != string.Empty)
                return new TextProcessorLinkResult { Id = Convert.ToInt32(mapId), IdType = MapIdType.Set, Line = line }; //Same as above

            return new TextProcessorLinkResult { IdType = MapIdType.None, Line = line };
        }
        public string GetParsedLine(string line, Regex regex)
        {
            var result = regex.Match(line);

            if (!result.Success)
                return string.Empty;

            return result.Groups[1].Value;
        }

    }
}
