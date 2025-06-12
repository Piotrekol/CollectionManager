namespace CollectionManager.Extensions.Modules.API;

using CollectionManager.Core.Types;
using System;

public class BeatmapExtensionEx : BeatmapExtension
{
    public DateTime ApprovedDate { get; set; }
    public int GenreId { get; set; }
    public int LanguageId { get; set; }
}
