using System;
using CollectionManagerExtensionsDll.Enums;

namespace CollectionManagerExtensionsDll.DataTypes
{
    public class MsnResult :EventArgs
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Diff { get; set; }
        public OsuState OsuState { get; set; }

        public string Raw
        {
            get
            {
                string ret = $"{Artist} - {Title}";
                if (!string.IsNullOrEmpty(Diff))
                    ret += $" [{Diff}]";
                return ret;
            }
        }
        
    }
}