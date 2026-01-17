
namespace CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.Text;

public class UserListGenerator : GenericGenerator
{
    public static string NewLine = "|NL|";

    public string mainHeader
    {
        get; set => field = value.Replace(NewLine, Environment.NewLine);
    } = "";

    public string mainFooter
    {
        get; set => field = value.Replace(NewLine, Environment.NewLine);
    } = "";

    public string collectionBodyFormat
    {
        get; set => field = value.Replace(NewLine, Environment.NewLine);
    } = "";

    public string collectionFooter
    {
        get; set => field = value.Replace(NewLine, Environment.NewLine);
    } = "";

    public string collectionHeaderTemplate
    {
        get; set => field = value.Replace(NewLine, Environment.NewLine);
    } = "";

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
                    : _md5Output.Append($"unsubmitted: {map.Artist} - {map.Title} [{map.DiffName}]{Environment.NewLine}");
            }
        }
        else
        {
            foreach (Beatmap map in beatmaps)
            {
                _ = sb.Append(CollectionBodyFormat.RobotoFormat(map));
            }
        }

        _ = sb.Append(_md5Output);
        _ = _md5Output.Clear();

    }
}