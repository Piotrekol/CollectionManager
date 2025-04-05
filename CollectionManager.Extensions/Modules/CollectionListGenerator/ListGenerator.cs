namespace CollectionManager.Extensions.Modules.CollectionListGenerator;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;
using CollectionManager.Extensions.Utils;
using System.Collections.Generic;
using System.Text;

public class ListGenerator
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly Dictionary<CollectionListSaveType, IListGenerator> _listGenerators;

    public ListGenerator()
    {
        _listGenerators = new Dictionary<CollectionListSaveType, IListGenerator>
        {
            { CollectionListSaveType.Txt, new TxtListGenerator() },
            { CollectionListSaveType.Html, new HtmlListGenerator() },
            { CollectionListSaveType.osuBBCode, new OsuBbCodeGenerator() },
            { CollectionListSaveType.RedditCode, new RedditCodeGenerator() },
            { CollectionListSaveType.BeatmapList, new BeatmapListGenerator() },
            { CollectionListSaveType.UserDefined, null }
        };
    }
    public string GetMissingMapsList(OsuCollections collections,
        CollectionListSaveType listType = CollectionListSaveType.Txt) => GenerateList(collections, listType, BeatmapListType.NotKnown);

    public string GetAllMapsList(OsuCollections collections,
        CollectionListSaveType listType = CollectionListSaveType.Txt) => GenerateList(collections, listType, BeatmapListType.All);
    public string GetAllMapsList(OsuCollections collections, UserListGenerator listGenerator)
    {
        _listGenerators[CollectionListSaveType.UserDefined] = listGenerator;
        return GenerateList(collections, CollectionListSaveType.UserDefined, BeatmapListType.All);
    }

    private string GenerateList(OsuCollections collections,
        CollectionListSaveType listType = CollectionListSaveType.Txt,
        BeatmapListType beatmapListType = BeatmapListType.All)
    {
        if (collections == null)
        {
            return "";
        }

        IListGenerator generator = _listGenerators[listType];
        generator.StartGenerating();

        _ = _stringBuilder.Clear();
        _ = _stringBuilder.Append(generator.GetListHeader(collections));
        for (int i = 0; i < collections.Count; i++)
        {
            Dictionary<int, Beatmaps> mapSets = collections[i].GetMapSets(beatmapListType);
            _ = _stringBuilder.Append(
                generator.GetCollectionBody(collections[i], mapSets, i)
                );

        }

        _ = _stringBuilder.Append(generator.GetListFooter(collections));

        generator.EndGenerating();

        return _stringBuilder.ToString();
    }
}