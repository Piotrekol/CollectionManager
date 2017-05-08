using System;
using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes;
using CollectionManagerExtensionsDll.Utils;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator
{
    public class ListGenerator
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private Dictionary<CollectionListSaveType, IListGenerator> _listGenerators;

        public ListGenerator()
        {
            _listGenerators = new Dictionary<CollectionListSaveType, IListGenerator>();

            _listGenerators.Add(CollectionListSaveType.Txt, new TxtListGenerator());
            _listGenerators.Add(CollectionListSaveType.Html, new HtmlListGenerator());
            _listGenerators.Add(CollectionListSaveType.osuBBCode, new OsuBbCodeGenerator());
            _listGenerators.Add(CollectionListSaveType.RedditCode, new RedditCodeGenerator());
            _listGenerators.Add(CollectionListSaveType.BeatmapList, new BeatmapListGenerator());
            _listGenerators.Add(CollectionListSaveType.UserDefined, null);
        }
        public string GetMissingMapsList(Collections collections,
            CollectionListSaveType listType = CollectionListSaveType.Txt)
        {
            return GenerateList(collections, listType, BeatmapListType.NotKnown);
        }

        public string GetAllMapsList(Collections collections,
            CollectionListSaveType listType = CollectionListSaveType.Txt)
        {
            return GenerateList(collections, listType, BeatmapListType.All);
        }
        public string GetAllMapsList(Collections collections, UserListGenerator listGenerator)
        {
            _listGenerators[CollectionListSaveType.UserDefined] = listGenerator;
            return GenerateList(collections, CollectionListSaveType.UserDefined, BeatmapListType.All);
        }

        private string GenerateList(Collections collections,
            CollectionListSaveType listType = CollectionListSaveType.Txt,
            BeatmapListType beatmapListType = BeatmapListType.All)
        {
            if (collections == null) return "";
            var generator = _listGenerators[listType];
            generator.StartGenerating();

            _stringBuilder.Clear();
            _stringBuilder.Append(generator.GetListHeader(collections));
            for (int i = 0; i < collections.Count; i++)
            {
                var mapSets = collections[i].GetMapSets(beatmapListType);
                _stringBuilder.Append(
                    generator.GetCollectionBody(collections[i], mapSets, i)
                    );

            }
            _stringBuilder.Append(generator.GetListFooter(collections));

            generator.EndGenerating();


            return _stringBuilder.ToString();
        }




    }
}