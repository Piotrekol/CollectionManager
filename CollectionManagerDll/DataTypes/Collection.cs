using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CollectionManager.Annotations;
using CollectionManager.Exceptions;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.DataTypes
{
    public class Collection : IEnumerable
    {
        private MapCacher LoadedMaps = null;

        /// <summary>
        /// Contains all beatmap hashes contained in this collection
        /// </summary>
        private HashSet<string> _beatmapHashes = new HashSet<string>();
        /// <summary>
        /// Contains beatmaps that did not find a match in LoadedMaps
        /// nor had additional data(MapSetId)
        /// </summary>
        public Beatmaps UnknownBeatmaps { get; } = new Beatmaps();
        /// <summary>
        /// Contains beatmaps that did not find a match in LoadedMaps
        /// but contain enough information(MapSetId) to be able to issue new download
        /// </summary>
        /// <remarks>.osdb files contain this data since v2</remarks>
        public Beatmaps DownloadableBeatmaps { get; }= new Beatmaps();
        /// <summary>
        /// Contains beatmap with data from LoadedMaps
        /// </summary>
        public Beatmaps KnownBeatmaps { get; }= new Beatmaps();



        /// <summary>
        /// Total number of beatmaps contained in this collection
        /// </summary>
        public int NumberOfBeatmaps { get { return UnknownBeatmaps.Count + KnownBeatmaps.Count + DownloadableBeatmaps.Count; } }

        public int NumberOfMissingBeatmaps { get { return UnknownBeatmaps.Count + DownloadableBeatmaps.Count; } }
        /// <summary>
        /// Username of last person editing this collection
        /// </summary>
        public string LastEditorUsername { get; set; }

        public void SetLoadedMaps(MapCacher instance)
        {
            if (instance == null)
                throw new BeatmapCacherNotInitalizedException();
            if (LoadedMaps != null)
                LoadedMaps.BeatmapsModified -= LoadedMaps_BeatmapsModified;

            LoadedMaps = instance;
            LoadedMaps.BeatmapsModified += LoadedMaps_BeatmapsModified;
            ReprocessBeatmaps();
        }

        private void LoadedMaps_BeatmapsModified(object sender, System.EventArgs e)
        {
            ReprocessBeatmaps();
        }

        private void ReprocessBeatmaps()
        {
            if (_beatmapHashes.Count <= 0)
                return;

            var tempBeatmaps = new Beatmaps();
            tempBeatmaps.AddRange(this.AllBeatmaps());
            UnknownBeatmaps.Clear();
            KnownBeatmaps.Clear();
            DownloadableBeatmaps.Clear();


            foreach (var beatmap in tempBeatmaps)
            {
                ProcessNewlyAddedMap(beatmap);
            }

        }


        public IEnumerable<BeatmapExtension> AllBeatmaps()
        {
            for (int i = 0; i < this.KnownBeatmaps.Count; i++)
            {
                yield return this.KnownBeatmaps[i];
            }
            foreach (var beatmap in NotKnownBeatmaps())
            {
                yield return beatmap;
            }

        }
        public IEnumerable<BeatmapExtension> NotKnownBeatmaps()
        {
            for (int i = 0; i < this.DownloadableBeatmaps.Count; i++)
            {
                yield return this.DownloadableBeatmaps[i];
            }
            for (int i = 0; i < this.UnknownBeatmaps.Count; i++)
            {
                yield return this.UnknownBeatmaps[i];
            }
        }

        public string Name { get; set; } = ".";

        public Collection(MapCacher instance)
        {
            SetLoadedMaps(instance);
        }
        public void AddBeatmap(Beatmap map)
        {
            var exMap = new BeatmapExtension();
            exMap.CloneValues(map);
            AddBeatmap(exMap);
        }


        public void AddBeatmap(BeatmapExtension map)
        {
            if (_beatmapHashes.Contains(map.Md5))
                return;
            _beatmapHashes.Add(map.Md5);
            ProcessNewlyAddedMap(map);
        }


        public void AddBeatmapByHash(string hash)
        {
            if (_beatmapHashes.Contains(hash))
                return;
            this.AddBeatmap(new BeatmapExtension() { Md5 = hash });
        }

        private void ProcessAdditionalProps(BeatmapExtension src, BeatmapExtension dest)
        {
            dest.UserComment = src.UserComment;
        }
        private void ProcessNewlyAddedMap(BeatmapExtension map)
        {
            lock (LoadedMaps.LockingObject)
            {
                if (LoadedMaps.Beatmaps.Count == 0)
                {
                    UnknownBeatmaps.Add(map);
                    return;
                }
                if (LoadedMaps.LoadedBeatmapsMd5Dict.ContainsKey(map.Md5))
                {
                    var knownMap = LoadedMaps.LoadedBeatmapsMd5Dict[map.Md5];
                    ProcessAdditionalProps(map,knownMap);
                    KnownBeatmaps.Add(knownMap);
                    return;
                }
                if (map.MapId > 10 && LoadedMaps.LoadedBeatmapsMapIdDict.ContainsKey(map.MapId))
                {
                    //Remove previously added map hash
                    _beatmapHashes.Remove(map.Md5);
                    //Get our local version of the map
                    var knownMap = LoadedMaps.LoadedBeatmapsMapIdDict[map.MapId];
                    //And add that instead.
                    _beatmapHashes.Add(knownMap.Md5);
                    ProcessAdditionalProps(map, knownMap);
                    KnownBeatmaps.Add(knownMap);
                    knownMap.LocalVersionDiffers = true;

                    return;
                }
                if (map.MapSetId != 0)
                {
                    DownloadableBeatmaps.Add(map);

                }
                else
                {
                    UnknownBeatmaps.Add(map);
                }

                map.LocalBeatmapMissing = true;
            }
        }

        public void RemoveBeatmap(string hash)
        {
            if (_beatmapHashes.Contains(hash))
            {
                foreach (var map in AllBeatmaps())
                {
                    if (map.Md5 == hash)
                    {
                        UnknownBeatmaps.Remove(map);
                        KnownBeatmaps.Remove(map);
                        DownloadableBeatmaps.Remove(map);
                        _beatmapHashes.Remove(hash);
                        break;
                    }
                }

            }


        }
        public IEnumerator GetEnumerator()
        {
            return this.AllBeatmaps().GetEnumerator();
        }

    }
}