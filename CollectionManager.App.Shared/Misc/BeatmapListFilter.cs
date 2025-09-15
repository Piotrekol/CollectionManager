namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Filter;
using System.Threading;
using Timer = System.Timers.Timer;

public class BeatmapListFilter : ICommonModelFilter, IDisposable
{
    private readonly BeatmapFilter _beatmapFilter;
    public Mods CurrentMods => _beatmapFilter.MainMods;
    public PlayMode CurrentPlayMode => _beatmapFilter.MainPlayMode;
    private string _searchString;
    private readonly Lock _searchStringLock = new();
    private readonly Timer timer;
    public event EventHandler FilteringStarted;
    public event EventHandler FilteringFinished;
    public BeatmapListFilter(Beatmaps beatmaps, Scores scores)
    {
        _beatmapFilter = new BeatmapFilter(beatmaps, scores, new BeatmapExtension());
        timer = new Timer
        {
            Interval = 400,
            AutoReset = false
        };

        timer.Elapsed += Timer_Elapsed;
    }

    public void SetBeatmaps(Beatmaps beatmaps)
        => _beatmapFilter.SetBeatmaps(beatmaps);

    public void SetScores(Scores scores)
        => _beatmapFilter.SetScores(scores);

    private void Timer_Elapsed(object sender, EventArgs e)
    {
        using Lock.Scope @lock = _searchStringLock.EnterScope();

        OnFilteringStarted();
        _beatmapFilter.UpdateSearch(_searchString);
        OnFilteringFinished();
    }

    public bool Filter(object modelObject) => modelObject is BeatmapExtension && (!_beatmapFilter.BeatmapHashHidden.ContainsKey(((BeatmapExtension)modelObject).Md5) || !_beatmapFilter.BeatmapHashHidden[((BeatmapExtension)modelObject).Md5]);

    public void UpdateSearch(string value)
    {
        using Lock.Scope @lock = _searchStringLock.EnterScope();

        _searchString = value;

        if (timer.Enabled)
        {
            timer.Stop();
        }

        timer.Start();
    }

    protected virtual void OnFilteringFinished() => FilteringFinished?.Invoke(this, EventArgs.Empty);

    protected virtual void OnFilteringStarted() => FilteringStarted?.Invoke(this, EventArgs.Empty);

    public void Dispose()
    {
        timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}