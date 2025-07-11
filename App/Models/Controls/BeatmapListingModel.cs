﻿namespace CollectionManagerApp.Models.Controls;

using CollectionManager.Common;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Misc;

public class BeatmapListingModel : IBeatmapListingModel, IDisposable
{
    public event EventHandler BeatmapsChanged;
    public event EventHandler FilteringStarted;
    public event EventHandler FilteringFinished;
    public event EventHandler SelectedBeatmapChanged;
    public event EventHandler SelectedBeatmapsChanged;
    public event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
    public event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

    private readonly BeatmapListFilter _beatmapListFilter;
    private Beatmaps _beatmapsDataSource;
    private Beatmap _selectedBeatmap;
    public Beatmap SelectedBeatmap
    {
        get => _selectedBeatmap;
        set
        {
            _selectedBeatmap = value;
            SelectedBeatmapChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public IOsuCollection CurrentCollection { get; private set; }
    private Beatmaps _selectedBeatmaps;
    public Beatmaps SelectedBeatmaps
    {
        get => _selectedBeatmaps;
        set
        {
            _selectedBeatmaps = value;
            SelectedBeatmapsChanged?.Invoke(this, EventArgs.Empty);

        }
    }

    public void EmitBeatmapOperation(BeatmapListingAction args) => BeatmapOperation?.Invoke(this, args);
    public void EmitBeatmapsDropped(object sender, Beatmaps beatmaps) => BeatmapsDropped?.Invoke(sender, beatmaps);

    public BeatmapListingModel(Beatmaps dataSource)
    {
        SetBeatmaps(dataSource);
        _beatmapListFilter = new BeatmapListFilter(GetBeatmaps(), Initalizer.OsuFileIo.ScoresDatabase.Scores);
        _beatmapListFilter.FilteringStarted += delegate
        { OnFilteringStarted(); };
        _beatmapListFilter.FilteringFinished += delegate
        { OnFilteringFinished(); };
    }

    public Beatmaps GetBeatmaps() => _beatmapsDataSource;
    public void SetBeatmaps(Beatmaps beatmaps)
    {
        _beatmapsDataSource = beatmaps;
        OnBeatmapsChanged();
    }

    public void SetCollection(IOsuCollection collection)
    {
        if (collection == null)
        {
            SetBeatmaps(null);
            CurrentCollection = collection;
            return;
        }

        CurrentCollection = collection;
        Beatmaps maps = [.. collection.AllBeatmaps()];
        SetBeatmaps(maps);
    }

    public void FilterBeatmaps(string text) => _beatmapListFilter.UpdateSearch(text);

    public ICommonModelFilter GetFilter() => _beatmapListFilter;

    protected virtual void OnFilteringStarted() => FilteringStarted?.Invoke(this, EventArgs.Empty);

    protected virtual void OnFilteringFinished() => FilteringFinished?.Invoke(this, EventArgs.Empty);

    protected virtual void OnBeatmapsChanged()
    {
        if (_beatmapsDataSource != null && _beatmapListFilter != null)
        {
            _beatmapListFilter.SetScores(Initalizer.OsuFileIo.ScoresDatabase.Scores);
            _beatmapListFilter.SetBeatmaps(_beatmapsDataSource);
        }

        BeatmapsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _beatmapListFilter?.Dispose();
        GC.SuppressFinalize(this);
    }
}