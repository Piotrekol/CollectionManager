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
    /// Contains all beatmap hashes contained in this collection
    /// </summary>
    private readonly HashSet<string> _beatmapHashes = [];

    /// <summary>
    /// <inheritdoc cref="_beatmapHashes"/>
    /// </summary>
    public IReadOnlyCollection<string> BeatmapHashes => _beatmapHashes;
    /// <summary>
    /// Contains beatmaps that did not find a match in LoadedMaps
    /// nor had additional data(MapSetId)
    /// </summary>
    public Beatmaps UnknownBeatmaps { get; } = [];
    /// <summary>
    /// Contains beatmaps that did not find a match in LoadedMaps
    /// but contain enough information(MapSetId) to be able to issue new download
    /// </summary>
    /// <remarks>.osdb files contain this data since v2</remarks>
    public Beatmaps DownloadableBeatmaps { get; } = [];
    /// <summary>
    /// Contains beatmap with data from LoadedMaps
    /// </summary>
    public Beatmaps KnownBeatmaps { get; } = [];

    public override string ToString() => $"osu! map Collection: \"{Name}\" Count: {BeatmapHashes.Count}";

    /// <summary>
    /// Total number of beatmaps contained in this collection
    /// </summary>
    public virtual int NumberOfBeatmaps
    {
        get => UnknownBeatmaps.Count + KnownBeatmaps.Count + DownloadableBeatmaps.Count;
        set { }
    }

    public virtual int NumberOfMissingBeatmaps => UnknownBeatmaps.Count + DownloadableBeatmaps.Count;
    /// <summary>
    /// Username of last person editing this collection
    /// </summary>
    public string LastEditorUsername { get; set; }

    public int OnlineId { get; set; }

    public int Id { get; set; }

    public Guid LazerId { get; set; }

    public void SetLoadedMaps(MapCacher instance)
    {
        if (instance == null)
        {
            throw new BeatmapCacherNotInitalizedException();
        }

        if (LoadedMaps != null)
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
        if (_beatmapHashes.Count <= 0)
        {
            return;
        }

        Beatmaps tempBeatmaps = [.. AllBeatmaps()];
        UnknownBeatmaps.Clear();
        KnownBeatmaps.Clear();
        DownloadableBeatmaps.Clear();

        foreach (Beatmap beatmap in tempBeatmaps)
        {
            ProcessNewlyAddedMap((BeatmapExtension)beatmap);
        }
    }

    public IEnumerable<BeatmapExtension> AllBeatmaps()
    {
        for (int i = 0; i < KnownBeatmaps.Count; i++)
        {
            yield return (BeatmapExtension)KnownBeatmaps[i];
        }

        foreach (BeatmapExtension beatmap in NotKnownBeatmaps())
        {
            yield return beatmap;
        }
    }
    public IEnumerable<BeatmapExtension> NotKnownBeatmaps()
    {
        for (int i = 0; i < DownloadableBeatmaps.Count; i++)
        {
            yield return (BeatmapExtension)DownloadableBeatmaps[i];
        }

        for (int i = 0; i < UnknownBeatmaps.Count; i++)
        {
            yield return (BeatmapExtension)UnknownBeatmaps[i];
        }
    }

    public string Name { get; set; } = ".";

    public OsuCollection(MapCacher instance)
    {
        SetLoadedMaps(instance);
    }
    public void AddBeatmap(Beatmap map)
    {
        BeatmapExtension exMap = new();
        exMap.CloneValues(map);
        AddBeatmap(exMap);
    }

    public void AddBeatmap(BeatmapExtension map)
    {
        if (string.IsNullOrEmpty(map.Md5))
        {
            map.Md5 = "semiRandomHash:" + map.MapId + "|" + map.MapSetId;
        }

        if (_beatmapHashes.Contains(map.Md5))
        {
            return;
        }

        ProcessNewlyAddedMap(map);
    }

    public void AddBeatmapByHash(string hash)
    {
        if (_beatmapHashes.Contains(hash))
        {
            return;
        }

        AddBeatmap(new BeatmapExtension() { Md5 = hash });
    }

    public void AddBeatmapByMapId(int mapId)
    {
        if (AllBeatmaps().Any(m => m.MapId == mapId))
        {
            return;
        }

        AddBeatmap(new BeatmapExtension() { MapId = mapId });
    }

    private static void ProcessAdditionalProps(BeatmapExtension src, BeatmapExtension dest) => dest.UserComment = src.UserComment;
    protected virtual void ProcessNewlyAddedMap(BeatmapExtension map)
    {
        lock (LoadedMaps.LockingObject)
        {
            _ = _beatmapHashes.Add(map.Md5);

            BeatmapExtension knownMap = (BeatmapExtension)LoadedMaps.GetByHash(map.Md5);
            if (knownMap != null)
            {
                ProcessAdditionalProps(map, knownMap);
                KnownBeatmaps.Add(knownMap);
                return;
            }

            knownMap = (BeatmapExtension)LoadedMaps.GetByMapId(map.MapId);
            if (map.MapId > 10 && knownMap != null)
            {
                //Remove previously added map hash
                _ = _beatmapHashes.Remove(map.Md5);
                //And add our local version of the map that instead.
                _ = _beatmapHashes.Add(knownMap.Md5);
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

    public void ReplaceBeatmap(string hash, Beatmap newBeatmap)
    {
        if (RemoveBeatmap(hash))
        {
            AddBeatmap(newBeatmap);
        }
    }
    public void ReplaceBeatmap(int mapId, Beatmap newBeatmap)
    {
        BeatmapExtension map = AllBeatmaps().FirstOrDefault(m => m.MapId == mapId);

        if (map != null && RemoveBeatmap(map.Md5))
        {
            AddBeatmap(newBeatmap);
        }
    }
    public virtual bool RemoveBeatmap(string hash)
    {
        if (_beatmapHashes.Contains(hash))
        {
            foreach (BeatmapExtension map in AllBeatmaps())
            {
                if (map.Md5 == hash)
                {
                    _ = UnknownBeatmaps.Remove(map);
                    _ = KnownBeatmaps.Remove(map);
                    _ = DownloadableBeatmaps.Remove(map);
                    _ = _beatmapHashes.Remove(hash);
                    return true;
                }
            }
        }

        return false;
    }
    public IEnumerator GetEnumerator() => AllBeatmaps().GetEnumerator();

}