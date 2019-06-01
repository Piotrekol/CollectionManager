using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    public abstract class GenericGenerator : IListGenerator
    {
        protected StringBuilder _mainStringBuilder = new StringBuilder();
        protected StringBuilder _md5Output = new StringBuilder();

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

        public string GetListHeader(Collections collections)
        {
            return MainHeader;
        }

        public virtual string GetCollectionBody(ICollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
        {
            _mainStringBuilder.Clear();
            _md5Output.Clear();

            _mainStringBuilder.AppendFormat(CollectionHeaderTemplate, collection.Name, collection.NumberOfBeatmaps);
            foreach (var mapSet in mapSets)
            {
                GetMapSetList(mapSet.Key, mapSet.Value, ref _mainStringBuilder);
            }
            _mainStringBuilder.Append(CollectionFooter);
            return _mainStringBuilder.ToString();
        }

        public string GetListFooter(Collections collections)
        {
            return MainFooter;
        }
        protected virtual void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
        {

            if (mapSetId == -1)
            {
                foreach (var map in beatmaps)
                {
                    if (map.MapId > 0)
                    {

                        sb.AppendFormat(CollectionBodyFormat, map.MapLink, map.ToString(true), map.Creator, map.DiffName, map.StarsNomod, map.ToString());
                    }
                    else
                    {
                        _md5Output.AppendFormat(CollectionBodyFormat, "http://osu.ppy.sh/b/0", map.Md5, "","","",map.Md5);
                    }
                }
            }
            else
            {
                foreach (var map in beatmaps)
                {
                    sb.AppendFormat(CollectionBodyFormat, map.MapLink, map.ToString(true), map.Creator, map.DiffName, map.StarsNomod, map.ToString());
                }
            }
            sb.Append(_md5Output.ToString());
            _md5Output.Clear();
        }
    }
}