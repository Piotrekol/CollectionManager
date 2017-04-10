using System;
using App.Interfaces;
using CollectionManager.DataTypes;

namespace App.Models
{
    public abstract class GenericMapSetterModel :IGenericMapSetterModel
    {
        public virtual void SetBeatmap(Beatmap beatmap)
        {
            CurrentBeatmap = beatmap;
        }

        private Beatmap _currentBeatmap;

        public virtual Beatmap CurrentBeatmap
        {
            get
            {
                return _currentBeatmap;
            }
            set
            {
                _currentBeatmap = value;
                OnBeatmapChanged();
            }
        }

        public event EventHandler BeatmapChanged;

        protected virtual void OnBeatmapChanged()
        {
            BeatmapChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}