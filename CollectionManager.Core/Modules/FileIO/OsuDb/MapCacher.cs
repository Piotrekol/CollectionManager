namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;

public class MapCacher : IMapDataManager
{
    private readonly Dictionary<string, Beatmap> LoadedBeatmapsHashDict = [];
    private readonly Dictionary<int, Beatmap> LoadedBeatmapsMapIdDict = [];
    private bool _massStoring;
    private readonly HashSet<string> MassStoringBeatmapHashes = [];

    public object LockingObject { get; } = new();
    public Beatmaps Beatmaps { get; } = [];
    public event EventHandler BeatmapsModified;

    public MapCacher()
    {

    }

    public void UnloadBeatmaps()
    {
        Beatmaps.Clear();
        EndMassStoring();
    }

    private void UpdateLookupDicts(Beatmap map, bool recalculate = false)
    {
        lock (LockingObject)
        {
            if (recalculate)
            {
                LoadedBeatmapsHashDict.Clear();
                LoadedBeatmapsMapIdDict.Clear();
                foreach (Beatmap beatmap in Beatmaps)
                {
                    UpdateLookupDicts(beatmap);
                }

                return;

            }

            _ = LoadedBeatmapsHashDict.TryAdd(map.Md5, map);
            _ = LoadedBeatmapsHashDict.TryAdd(map.Hash, map);
            _ = LoadedBeatmapsMapIdDict.TryAdd(map.MapId, map);
        }
    }
    public void StartMassStoring()
    {
        _massStoring = true;

        foreach (Beatmap beatmap in Beatmaps)
        {
            _ = MassStoringBeatmapHashes.Add(beatmap.Md5);
            _ = MassStoringBeatmapHashes.Add(beatmap.Hash);
        }
    }

    public void EndMassStoring()
    {
        _massStoring = false;
        MassStoringBeatmapHashes.Clear();
        UpdateLookupDicts(null, true);
        OnBeatmapsModified();
    }

    public void StoreBeatmap(Beatmap beatmap)
    {
        if (_massStoring)
        {
            if (MassStoringBeatmapHashes.Add(beatmap.Md5))
            {
                Beatmaps.Add((Beatmap)beatmap.Clone());
            }

            return;
        }

        Beatmaps.Add((Beatmap)beatmap.Clone());
        UpdateLookupDicts(beatmap);
        OnBeatmapsModified();
    }

    public bool BeatmapExistsInDatabase(string md5) => Beatmaps.Any(beatmap => beatmap.Md5 == md5);

    private void OnBeatmapsModified() => BeatmapsModified?.Invoke(this, EventArgs.Empty);

    public Beatmap? GetByHash(string hash) => LoadedBeatmapsHashDict.TryGetValue(hash, out Beatmap value) ? value : null;

    public Beatmap? GetByMapId(int mapId) => LoadedBeatmapsMapIdDict.TryGetValue(mapId, out Beatmap value) ? value : null;

    public Beatmap? Get(string hash, int mapId)
    {
        Beatmap beatmap = GetByHash(hash);

        if (beatmap is not null || mapId <= 10)
        {
            return beatmap;
        }

        return GetByMapId(mapId);
    }
}
