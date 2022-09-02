using CollectionManager.DataTypes;
using System;

namespace App.Interfaces
{
    public interface ICombinedBeatmapPreviewModel : IGenericMapSetterModel
    {
        Mods SelectedMods { get; set; }
        event EventHandler SelectedModsChanged;
    }
}