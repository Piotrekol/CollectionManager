namespace CollectionManagerApp.Misc;

using CollectionManager.Common;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Filter;
using System.Windows.Forms;

public class BeatmapListFilter : ICommonModelFilter, IDisposable
{
    private readonly BeatmapFilter _beatmapFilter;
    public Mods CurrentMods => _beatmapFilter.CurrentMods;
    public PlayMode CurrentPlayMode => _beatmapFilter.CurrentPlayMode;
    private string _searchString;
    private readonly object _searchStringLockingObject = new();
    private readonly Timer timer;
    public event EventHandler FilteringStarted;
    public event EventHandler FilteringFinished;
    public BeatmapListFilter(Beatmaps beatmaps, Scores scores)
    {
        _beatmapFilter = new BeatmapFilter(beatmaps, scores, new BeatmapExtension());
        timer = new Timer
        {
            Interval = 400
        };
        timer.Tick += Timer_Tick;
    }

    public void SetBeatmaps(Beatmaps beatmaps)
        => _beatmapFilter.SetBeatmaps(beatmaps);

    public void SetScores(Scores scores)
        => _beatmapFilter.SetScores(scores);

    private void Timer_Tick(object sender, EventArgs e)
    {
        lock (timer)
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
        }

        lock (_searchStringLockingObject)
        {
            OnFilteringStarted();
            _beatmapFilter.UpdateSearch(_searchString);
            OnFilteringFinished();
        }
    }

    public bool Filter(object modelObject) => modelObject is BeatmapExtension && (!_beatmapFilter.BeatmapHashHidden.ContainsKey(((BeatmapExtension)modelObject).Md5) || !_beatmapFilter.BeatmapHashHidden[((BeatmapExtension)modelObject).Md5]);

    public void UpdateSearch(string value)
    {
        lock (_searchStringLockingObject)
        {
            _searchString = value;
        }

        lock (timer)
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }

            timer.Start();
        }
    }

    protected virtual void OnFilteringFinished() => FilteringFinished?.Invoke(this, EventArgs.Empty);

    protected virtual void OnFilteringStarted() => FilteringStarted?.Invoke(this, EventArgs.Empty);

    public void Dispose()
    {
        timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}