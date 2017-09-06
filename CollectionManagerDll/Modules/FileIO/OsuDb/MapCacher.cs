using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.FileIO.OsuDb
{
    public class MapCacher : IMapDataManager
    {
        public readonly object LockingObject = new object();
        public readonly Beatmaps Beatmaps = new Beatmaps();
        public HashSet<string> BeatmapHashes = new HashSet<string>();
        private Dictionary<string, Beatmap> LoadedBeatmapsMd5Dict = new Dictionary<string, Beatmap>();
        private Dictionary<int, Beatmap> LoadedBeatmapsMapIdDict = new Dictionary<int, Beatmap>();
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
                    foreach (var beatmap in Beatmaps)
                    {
                        UpdateLookupDicts(beatmap);
                    }
                    return;

                }

                if (!LoadedBeatmapsMd5Dict.ContainsKey(map.Md5))
                    LoadedBeatmapsMd5Dict.Add(map.Md5, map);

                if (!LoadedBeatmapsMapIdDict.ContainsKey(map.MapId))
                    LoadedBeatmapsMapIdDict.Add(map.MapId, map);
            }
        }
        public void StartMassStoring()
        {
            _massStoring = true;
        }

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
                this.BeatmapHashes.Add(beatmap.Md5);
                this.Beatmaps.Add((Beatmap)beatmap.Clone());
                if (!_massStoring)
                {
                    UpdateLookupDicts(beatmap);
                    OnBeatmapsModified();
                }
            }
        }

        public bool BeatmapExistsInDatabase(string md5)
        {
            return BeatmapHashes.Contains(md5);
        }

        private void OnBeatmapsModified()
        {
            BeatmapsModified?.Invoke(this, EventArgs.Empty);
        }

        public Beatmap GetByHash(string hash)
        {
            if (LoadedBeatmapsMd5Dict.ContainsKey(hash))
                return LoadedBeatmapsMd5Dict[hash];
            return null;
        }

        public Beatmap GetByMapId(int mapId)
        {
            if (LoadedBeatmapsMapIdDict.ContainsKey(mapId))
                return LoadedBeatmapsMapIdDict[mapId];
            return null;
        }
    }
}
