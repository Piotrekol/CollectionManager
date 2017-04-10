using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    internal class TxtListGenerator: IListGenerator
    {
        internal string Lb = "\r\n";
        private StringBuilder _stringBuilder = new StringBuilder();
        
        public string GetListHeader(Collections collections)
        {
            return "";
        }
        

        public string GetCollectionBody(Collection collection, Dictionary<int, Beatmaps> mapSets,int collectionNumber)
        {
            _stringBuilder.Clear();

            _stringBuilder.AppendFormat("{0}{1}{0}{0}", Lb, string.Format("Collection {0}: {1}", collectionNumber, collection.Name));
            //collection content(beatmaps)

            foreach (var mapSet in mapSets)
            {
                GetMapSetList(mapSet.Key, mapSet.Value, ref _stringBuilder);
            }

            return _stringBuilder.ToString();
        }

        public string GetListFooter(Collections collections)
        {
            return "";
        }

        public void StartGenerating()
        {
            _stringBuilder.Clear();
        }

        public void EndGenerating()
        {
            _stringBuilder.Clear();
        }

        private void GetMapSetList(int mapSetId, Beatmaps beatmaps, ref StringBuilder sb)
        {

            if (mapSetId == -1)
            {
                foreach (var map in beatmaps)
                {
                    if (map.MapId > 0)
                    {
                        sb.AppendFormat("{0} {1} - {2}", map.MapLink, map.ArtistRoman, map.TitleRoman);
                        if (!string.IsNullOrWhiteSpace(map.DiffName))
                            sb.AppendFormat(" [{0}]{1}", map.DiffName, Lb);
                    }
                    else
                    {
                        sb.AppendFormat("No data - {0}{1}", map.Md5, Lb);
                    }

                }
            }
            else
            {
                sb.AppendFormat("{0} {1} - {2}", beatmaps[0].MapLink, beatmaps[0].ArtistRoman, beatmaps[0].TitleRoman);
                foreach (var map in beatmaps)
                {
                    sb.AppendFormat(" [{0}] {1}★", map.DiffName, map.StarsNomod);
                }
                sb.Append(Lb);
            }
        }
    }
}