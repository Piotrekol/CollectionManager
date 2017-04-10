using System;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.API
{
    public class BeatmapExtensionEx : BeatmapExtension
    {
        public DateTime ApprovedDate { get; set; }
        public int GenreId { get; set; }
        public int LanguageId { get; set; }
    }
}
