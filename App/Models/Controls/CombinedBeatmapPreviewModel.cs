using App.Interfaces;
using CollectionManager.DataTypes;
using System;

namespace App.Models
{
    public class CombinedBeatmapPreviewModel : GenericMapSetterModel, ICombinedBeatmapPreviewModel
    {
        private Mods selectedMods;

        public Mods SelectedMods
        {
            get => selectedMods;
            set
            {
                selectedMods = value;
                SelectedModsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedModsChanged;
    }
}