namespace CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using System.Collections.Generic;
using System.Text;

internal class TxtListGenerator : IListGenerator
{
    internal string Lb = "\r\n";
    private StringBuilder _stringBuilder = new();

    public string GetListHeader(OsuCollections collections) => "";

    public string GetCollectionBody(IOsuCollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
    {
        _ = _stringBuilder.Clear();

        _ = _stringBuilder.AppendFormat("{0}{1}{0}{0}", Lb, string.Format("Collection {0}: {1}", collectionNumber, collection.Name));
        //collection content(beatmaps)

        foreach (KeyValuePair<int, Beatmaps> mapSet in mapSets)
        {
            GetMapSetList(mapSet.Key, mapSet.Value, ref _stringBuilder);
        }

        return _stringBuilder.ToString();
    }

    public string GetListFooter(OsuCollections collections) => "";

    public void StartGenerating() => _stringBuilder.Clear();

    public void EndGenerating() => _stringBuilder.Clear();

    private void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
    {

        if (mapSetId == -1)
        {
            foreach (Beatmap map in beatmaps)
            {
                if (map.MapId > 0)
                {
                    _ = sb.AppendFormat("{0} {1} - {2}", map.MapLink, map.ArtistRoman, map.TitleRoman);
                    if (!string.IsNullOrWhiteSpace(map.DiffName))
                    {
                        _ = sb.AppendFormat(" [{0}]{1}", map.DiffName, Lb);
                    }
                }
                else
                {
                    _ = sb.AppendFormat("No data - {0}{1}", map.Md5, Lb);
                }
            }
        }
        else
        {
            _ = sb.AppendFormat("{0} {1} - {2}", beatmaps[0].MapSetLink, beatmaps[0].ArtistRoman, beatmaps[0].TitleRoman);
            foreach (Beatmap map in beatmaps)
            {
                _ = sb.AppendFormat(" [{0}] {1}★({2})", map.DiffName, map.StarsNomod, map.MapId);
            }

            _ = sb.Append(Lb);
        }
    }
}