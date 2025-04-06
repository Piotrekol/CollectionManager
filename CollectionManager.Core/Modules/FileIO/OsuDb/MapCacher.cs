namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;

public class MapCacher : IMapDataManager
{
    private readonly Dictionary<string, Beatmap> LoadedBeatmapsMd5Dict = [];
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
                LoadedBeatmapsMd5Dict.Clear();
                LoadedBeatmapsMapIdDict.Clear();
                foreach (Beatmap beatmap in Beatmaps)
                {
                    UpdateLookupDicts(beatmap);
                }

                return;

            }

            _ = LoadedBeatmapsMd5Dict.TryAdd(map.Md5, map);
            _ = LoadedBeatmapsMapIdDict.TryAdd(map.MapId, map);
        }
    }
    public void StartMassStoring()
    {
        _massStoring = true;

        foreach (Beatmap beatmap in Beatmaps)
        {
            _ = MassStoringBeatmapHashes.Add(beatmap.Md5);
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

    public Beatmap GetByHash(string hash) => LoadedBeatmapsMd5Dict.TryGetValue(hash, out Beatmap value) ? value : null;

    public Beatmap GetByMapId(int mapId) => LoadedBeatmapsMapIdDict.TryGetValue(mapId, out Beatmap value) ? value : null;
}
