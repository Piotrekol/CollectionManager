
namespace CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.Text;

public class UserListGenerator : GenericGenerator
{
    public static string NewLine = "|NL|";
    private string _mainHeader = "";
    private string _mainFooter = "";
    private string _collectionBodyFormat = "";
    private string _collectionFooter = "";
    private string _collectionHeaderTemplate = "";

    public string mainHeader
    {
        get => _mainHeader; set => _mainHeader = value.Replace(NewLine, Environment.NewLine);
    }

    public string mainFooter
    {
        get => _mainFooter; set => _mainFooter = value.Replace(NewLine, Environment.NewLine);
    }

    public string collectionBodyFormat
    {
        get => _collectionBodyFormat; set => _collectionBodyFormat = value.Replace(NewLine, Environment.NewLine);
    }

    public string collectionFooter
    {
        get => _collectionFooter; set => _collectionFooter = value.Replace(NewLine, Environment.NewLine);
    }

    public string collectionHeaderTemplate
    {
        get => _collectionHeaderTemplate; set => _collectionHeaderTemplate = value.Replace(NewLine, Environment.NewLine);
    }

    protected override string MainHeader => mainHeader;
    protected override string MainFooter => mainFooter;
    protected override string CollectionBodyFormat => collectionBodyFormat;
    protected override string CollectionFooter => collectionFooter;
    protected override string CollectionHeaderTemplate => collectionHeaderTemplate;

    public override string GetCollectionBody(IOsuCollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
    {
        try
        {
            return base.GetCollectionBody(collection, mapSets, collectionNumber);
        }
        catch (FormatException)
        {
            return "One of the format parameters is invalid.";
        }
    }

    protected override void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
    {
        if (mapSetId == -1)
        {
            foreach (Beatmap map in beatmaps)
            {
                _ = map.MapId > 0
                    ? sb.Append(CollectionBodyFormat.RobotoFormat(map))
                    : _md5Output.AppendFormat(CollectionBodyFormat, "http://osu.ppy.sh/b/0", map.Md5, "", "", "",
                        map.Md5);
            }
        }
        else
        {
            foreach (Beatmap map in beatmaps)
            {
                _ = sb.Append(CollectionBodyFormat.RobotoFormat(map));
            }
        }

        _ = sb.Append(_md5Output.ToString());
        _ = _md5Output.Clear();

    }
}