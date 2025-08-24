namespace CollectionManager.App.Shared.Models;

using CollectionManager.App.Shared.Interfaces;
using CollectionManager.Core.Types;

public abstract class GenericMapSetterModel : IGenericMapSetterModel
{
    public virtual void SetBeatmap(Beatmap beatmap) => CurrentBeatmap = beatmap;

    private Beatmap _currentBeatmap;

    public virtual Beatmap CurrentBeatmap
    {
        get => _currentBeatmap;
        set
        {
            _currentBeatmap = value;
            OnBeatmapChanged();
        }
    }

    public event EventHandler BeatmapChanged;

    protected virtual void OnBeatmapChanged() => BeatmapChanged?.Invoke(this, EventArgs.Empty);
}