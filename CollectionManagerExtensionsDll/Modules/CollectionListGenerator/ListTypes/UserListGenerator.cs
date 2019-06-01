using System;
using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.Utils;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
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
            get { return _mainHeader; }
            set { _mainHeader = value.Replace(NewLine, Environment.NewLine); }
        }

        public string mainFooter
        {
            get { return _mainFooter; }
            set { _mainFooter = value.Replace(NewLine, Environment.NewLine); }
        }

        public string collectionBodyFormat
        {
            get { return _collectionBodyFormat; }
            set { _collectionBodyFormat = value.Replace(NewLine, Environment.NewLine); }
        }

        public string collectionFooter
        {
            get { return _collectionFooter; }
            set { _collectionFooter = value.Replace(NewLine, Environment.NewLine); }
        }

        public string collectionHeaderTemplate
        {
            get { return _collectionHeaderTemplate; }
            set { _collectionHeaderTemplate = value.Replace(NewLine, Environment.NewLine); }
        }

        protected override string MainHeader => mainHeader;
        protected override string MainFooter => mainFooter;
        protected override string CollectionBodyFormat => collectionBodyFormat;
        protected override string CollectionFooter => collectionFooter;
        protected override string CollectionHeaderTemplate => collectionHeaderTemplate;

        public override string GetCollectionBody(ICollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber)
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
                foreach (var map in beatmaps)
                {
                    if (map.MapId > 0)
                    {
                        sb.Append(CollectionBodyFormat.RobotoFormat(map));
                    }
                    else
                    {
                        _md5Output.AppendFormat(CollectionBodyFormat, "http://osu.ppy.sh/b/0", map.Md5, "", "", "",
                            map.Md5);
                    }
                }
            }
            else
            {
                foreach (var map in beatmaps)
                {
                    sb.Append(CollectionBodyFormat.RobotoFormat(map));
                }
            }
            sb.Append(_md5Output.ToString());
            _md5Output.Clear();

        }
    }
}