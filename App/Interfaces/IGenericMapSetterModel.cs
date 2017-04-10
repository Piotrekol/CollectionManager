using System;
using CollectionManager.DataTypes;

namespace App.Interfaces
{
    public interface IGenericMapSetterModel
    {
        void SetBeatmap(Beatmap beatmap);
        Beatmap CurrentBeatmap { get; }

        event EventHandler BeatmapChanged;
    }
}