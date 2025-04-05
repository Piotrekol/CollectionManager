namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;

public class MapCacher : IMapDataManager
{
    public readonly object LockingObject = new();
    public readonly Beatmaps Beatmaps = [];
    public HashSet<string> BeatmapHashes = [];
    private readonly Dictionary<string, Beatmap> LoadedBeatmapsMd5Dict = [];
    private readonly Dictionary<int, Beatmap> LoadedBeatmapsMapIdDict = [];
    public event EventHandler BeatmapsModified;
    private bool _massStoring = false;
    public MapCacher()
    {

    }

    public void UnloadBeatmaps()
    {
        Beatmaps.Clear();
        BeatmapHashes.Clear();
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

            if (!LoadedBeatmapsMd5Dict.ContainsKey(map.Md5))
            {
                LoadedBeatmapsMd5Dict.Add(map.Md5, map);
            }

            if (!LoadedBeatmapsMapIdDict.ContainsKey(map.MapId))
            {
                LoadedBeatmapsMapIdDict.Add(map.MapId, map);
            }
        }
    }
    public void StartMassStoring() => _massStoring = true;

    public void EndMassStoring()
    {
        _massStoring = false;
        UpdateLookupDicts(null, true);
        OnBeatmapsModified();
    }

    public void StoreBeatmap(Beatmap beatmap)
    {
        if (!BeatmapHashes.Contains(beatmap.Md5))
        {
            _ = BeatmapHashes.Add(beatmap.Md5);
            Beatmaps.Add((Beatmap)beatmap.Clone());
            if (!_massStoring)
            {
                UpdateLookupDicts(beatmap);
                OnBeatmapsModified();
            }
        }
    }

    public bool BeatmapExistsInDatabase(string md5) => BeatmapHashes.Contains(md5);

    private void OnBeatmapsModified() => BeatmapsModified?.Invoke(this, EventArgs.Empty);

    public Beatmap GetByHash(string hash) => LoadedBeatmapsMd5Dict.ContainsKey(hash) ? LoadedBeatmapsMd5Dict[hash] : null;

    public Beatmap GetByMapId(int mapId) => LoadedBeatmapsMapIdDict.ContainsKey(mapId) ? LoadedBeatmapsMapIdDict[mapId] : null;
}
