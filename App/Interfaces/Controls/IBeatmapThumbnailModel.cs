using CollectionManager.DataTypes;

namespace App.Interfaces
{
    public interface IBeatmapThumbnailModel : IGenericMapSetterModel
    {
        Mods SelectedMods { get; set; }
    }
}