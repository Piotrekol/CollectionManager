namespace CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using System.Collections.Generic;
using System.Text;

public abstract class GenericGenerator : IListGenerator
{
    protected StringBuilder _mainStringBuilder = new();
    protected StringBuilder _md5Output = new();

    protected abstract string MainHeader { get; }
    protected abstract string MainFooter { get; }

    /*0 - mapLink 
     * 1- full map string with diff 
     * 2- map creator 
     * 3- map diff 
     * 4- nomodStars 
     * 5- full map string without diff*/
    protected abstract string CollectionBodyFormat { get; }

    protected abstract string CollectionFooter { get; }

    /*0 - collection name 
     *1 - collection map count*/
    protected abstract string CollectionHeaderTemplate { get; }
    public void StartGenerating()
    {

    }

    public void EndGenerating()
    {

    }

    public string GetListHeader(OsuCollections collections) => MainHeader;

    public virtual string GetCollectionBody(IOsuCollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
    {
        _ = _mainStringBuilder.Clear();
        _ = _md5Output.Clear();

        _ = _mainStringBuilder.AppendFormat(CollectionHeaderTemplate, collection.Name, collection.NumberOfBeatmaps);
        foreach (KeyValuePair<int, Beatmaps> mapSet in mapSets)
        {
            GetMapSetList(mapSet.Key, mapSet.Value, ref _mainStringBuilder);
        }

        _ = _mainStringBuilder.Append(CollectionFooter);
        return _mainStringBuilder.ToString();
    }

    public string GetListFooter(OsuCollections collections) => MainFooter;
    protected virtual void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
    {

        if (mapSetId == -1)
        {
            foreach (Beatmap map in beatmaps)
            {
                _ = map.MapId > 0
                    ? sb.AppendFormat(CollectionBodyFormat, map.MapLink, map.ToString(true), map.Creator, map.DiffName, map.StarsNomod, map.ToString())
                    : _md5Output.AppendFormat(CollectionBodyFormat, "http://osu.ppy.sh/b/0", map.Md5, "", "", "", map.Md5);
            }
        }
        else
        {
            foreach (Beatmap map in beatmaps)
            {
                _ = sb.AppendFormat(CollectionBodyFormat, map.MapLink, map.ToString(true), map.Creator, map.DiffName, map.StarsNomod, map.ToString());
            }
        }

        _ = sb.Append(_md5Output.ToString());
        _ = _md5Output.Clear();
    }
}