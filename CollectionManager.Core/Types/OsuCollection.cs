namespace CollectionManager.Core.Types;

using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OsuCollection : IEnumerable, IOsuCollection
{
    private MapCacher LoadedMaps;

    /// <summary>
    /// Contains all beatmaps in this collection indexed by their hash
    /// </summary>
    private readonly Dictionary<string, BeatmapExtension> _beatmaps = [];
    private bool _beatmapCachesValid;

    /// <summary>
    /// Contains all beatmap hashes contained in this collection
    /// </summary>
    public IReadOnlyCollection<string> BeatmapHashes => _beatmaps.Keys;

    /// <summary>
    /// Contains beatmaps that did not find a match in LoadedMaps
    /// nor had additional data(MapSetId)
    /// </summary>
    public Beatmaps UnknownBeatmaps
    {
        get
        {
            EnsureCacheValid();
            return field;
        }

        private set;
    }

    /// <summary>
    /// Contains beatmaps that did not find a match in LoadedMaps
    /// but contain enough information(MapSetId) to be able to issue new download
    /// </summary>
    /// <remarks>.osdb files contain this data since v2</remarks>
    public Beatmaps DownloadableBeatmaps
    {
        get
        {
            EnsureCacheValid();
            return field;
        }

        private set;
    }

    /// <summary>
    /// Contains beatmap with data from LoadedMaps
    /// </summary>
    public Beatmaps KnownBeatmaps
    {
        get
        {
            EnsureCacheValid();
            return field;
        }

        private set;
    }

    private void EnsureCacheValid()
    {
        if (_beatmapCachesValid)
        {
            return;
        }

        Beatmaps known = [];
        Beatmaps downloadable = [];
        Beatmaps unknown = [];

        using RangeObservableCollection<Beatmap>.SuspendContext __ = known.SuspendCollectionChangedEvents();
        using RangeObservableCollection<Beatmap>.SuspendContext ___ = downloadable.SuspendCollectionChangedEvents();
        using RangeObservableCollection<Beatmap>.SuspendContext ____ = unknown.SuspendCollectionChangedEvents();

        foreach (BeatmapExtension beatmap in _beatmaps.Values)
        {
            if (!beatmap.LocalBeatmapMissing)
            {
                known.Add(beatmap);
            }
            else if (beatmap.MapSetId is not 0)
            {
                downloadable.Add(beatmap);
            }
            else
            {
                unknown.Add(beatmap);
            }
        }

        KnownBeatmaps = known;
        DownloadableBeatmaps = downloadable;
        UnknownBeatmaps = unknown;

        _beatmapCachesValid = true;
    }

    /// <summary>
    /// Invalidates all cached views
    /// </summary>
    private void InvalidateCache()
    {
        _beatmapCachesValid = false;
        KnownBeatmaps = null;
        DownloadableBeatmaps = null;
        UnknownBeatmaps = null;
    }

    public override string ToString() => $"osu! map Collection: \"{Name}\" Count: {BeatmapHashes.Count}";

    /// <summary>
    /// Total number of beatmaps contained in this collection
    /// </summary>
    public virtual int NumberOfBeatmaps
    {
        get => _beatmaps.Count;
        set => throw new InvalidOperationException("Number of beatmaps can not be set in base collections.");
    }

    public virtual int NumberOfMissingBeatmaps => _beatmaps.Values.Count(b => b.LocalBeatmapMissing);

    /// <summary>
    /// Username of last person editing this collection
    /// </summary>
    public string LastEditorUsername { get; set; }

    public int OnlineId { get; set; }

    public int Id { get; set; }

    public Guid LazerId { get; set; }

    public OsuCollection(MapCacher instance)
    {
        SetLoadedMaps(instance);
    }

    public void SetLoadedMaps(MapCacher instance)
    {
        if (instance is null)
        {
            throw new BeatmapCacherNotInitalizedException();
        }

        if (LoadedMaps is not null)
        {
            LoadedMaps.BeatmapsModified -= LoadedMaps_BeatmapsModified;
        }

        LoadedMaps = instance;
        LoadedMaps.BeatmapsModified += LoadedMaps_BeatmapsModified;
        ReprocessBeatmaps();
    }

    private void LoadedMaps_BeatmapsModified(object sender, EventArgs e) => ReprocessBeatmaps();

    private void ReprocessBeatmaps()
    {
        if (_beatmaps.Count is 0)
        {
            InvalidateCache();

            return;
        }

        List<BeatmapExtension> tempBeatmaps = [.. _beatmaps.Values];
        _beatmaps.Clear();

        foreach (BeatmapExtension beatmap in tempBeatmaps)
        {
            ProcessNewlyAddedMap(beatmap);
        }

        InvalidateCache();
    }

    public IEnumerable<BeatmapExtension> AllBeatmaps() => _beatmaps.Values;

    public IEnumerable<BeatmapExtension> NotKnownBeatmaps() => _beatmaps.Values.Where(b => b.LocalBeatmapMissing);

    public string Name { get; set; } = ".";

    public void AddBeatmap(Beatmap map)
    {
        BeatmapExtension exMap = new();
        exMap.CloneValues(map);
        AddBeatmap(exMap);
    }

    public void AddBeatmap(BeatmapExtension map)
    {
        if (string.IsNullOrEmpty(map.Hash))
        {
            map.Hash = "semiRandomHash:" + map.MapId + "|" + map.MapSetId;
        }

        if (_beatmaps.ContainsKey(map.Hash))
        {
            return;
        }

        ProcessNewlyAddedMap(map);
        InvalidateCache();
    }

    public void AddBeatmapByHash(string hash)
    {
        if (_beatmaps.ContainsKey(hash))
        {
            return;
        }

        AddBeatmap(new BeatmapExtension() { Hash = hash });
    }

    public void AddBeatmapByMapId(int mapId)
    {
        if (_beatmaps.Values.Any(m => m.MapId == mapId))
        {
            return;
        }

        AddBeatmap(new BeatmapExtension() { MapId = mapId });
    }

    protected virtual void ProcessNewlyAddedMap(BeatmapExtension map)
    {
        lock (LoadedMaps.LockingObject)
        {
            Beatmap knownMap = LoadedMaps.Get(map.Hash, map.MapId);

            if (knownMap is null)
            {
                map.LocalBeatmapMissing = true;
                _beatmaps[map.Hash] = map;

                return;
            }

            if (knownMap is not BeatmapExtension beatmapToStore)
            {
                beatmapToStore = new BeatmapExtension();
                beatmapToStore.CloneValues(knownMap);
            }

            beatmapToStore.LocalBeatmapMissing = false;
            beatmapToStore.LocalVersionDiffers = knownMap.Hash != map.Hash;
            _beatmaps[knownMap.Hash] = beatmapToStore;

            return;
        }
    }

    public void ReplaceBeatmap(string hash, Beatmap newBeatmap)
    {
        if (RemoveBeatmap(hash))
        {
            AddBeatmap(newBeatmap);
        }
    }

    public void ReplaceBeatmap(int mapId, Beatmap newBeatmap)
    {
        BeatmapExtension map = _beatmaps.Values.FirstOrDefault(m => m.MapId == mapId);

        if (map is not null && RemoveBeatmap(map.Hash))
        {
            AddBeatmap(newBeatmap);
        }
    }

    public virtual bool RemoveBeatmap(string hash) => RemoveBeatmaps([hash]) is 1;

    public virtual int RemoveBeatmaps(IEnumerable<string> hashes)
    {
        if (hashes is null)
        {
            return 0;
        }

        HashSet<string> hashesToRemove = [.. hashes];
        hashesToRemove.IntersectWith(_beatmaps.Keys);

        if (hashesToRemove.Count is 0)
        {
            return 0;
        }

        foreach (string hash in hashesToRemove)
        {
            _ = _beatmaps.Remove(hash);
        }

        InvalidateCache();
        return hashesToRemove.Count;
    }

    public IEnumerator GetEnumerator() => AllBeatmaps().GetEnumerator();
}