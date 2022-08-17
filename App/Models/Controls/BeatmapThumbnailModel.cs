using App.Interfaces;
using CollectionManager.DataTypes;

namespace App.Models
{
    public class BeatmapThumbnailModel : GenericMapSetterModel, IBeatmapThumbnailModel
    {
        public Mods SelectedMods { get; set; }
    }
}