namespace CollectionManager.App.Shared.Interfaces;

using CollectionManager.Core.Types;

public interface IGenericMapSetterModel
{
    void SetBeatmap(Beatmap beatmap);
    Beatmap CurrentBeatmap { get; }

    event EventHandler BeatmapChanged;
}