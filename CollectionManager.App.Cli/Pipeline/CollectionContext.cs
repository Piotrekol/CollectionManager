namespace CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Shared state container for pipeline commands.
/// </summary>
internal sealed partial class CollectionContext : IDisposable
{
    private static readonly ILogger Logger = Program.Logger;
    private readonly OsuFileIo _fileIo;

    public CollectionsManagerWithCounts Manager { get; }

    public OsuCollections Collections => Manager.LoadedCollections;
    public MapCacher LoadedMaps => _fileIo.LoadedMaps;

    public CollectionContext()
    {
        _fileIo = new OsuFileIo(new BeatmapExtension());
        Manager = new(_fileIo.LoadedMaps);
    }

    public bool EnsureOsuDatabaseLoaded(PipelineOptions options)
        => EnsureOsuDatabaseLoaded(options.OsuLocation, options.SkipOsu, options.PreferStable, options.PreferLazer);

    private bool EnsureOsuDatabaseLoaded(string? explicitPath = default, bool skip = false, bool preferStable = false, bool preferLazer = false)
    {
        bool mapsAlreadyLoaded = LoadedMaps.Beatmaps.Count > 0;

        if (mapsAlreadyLoaded || skip)
        {
            return mapsAlreadyLoaded;
        }

        string? path = ResolveOsuLocation(explicitPath, preferStable, preferLazer);

        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            LogOsuDatabaseFound(path);
            _ = _fileIo.OsuDatabase.Load(path, progress: null, cancellationToken: default);
            StableOsuDatabaseData dbData = _fileIo.OsuDatabase.StableOsuDatabaseData;
            int beatmapCount = LoadedMaps.Beatmaps.Count;
            int beatmapSetCount = dbData?.FolderCount ?? LoadedMaps.Beatmaps.Select(b => b.MapSetId).Distinct().Count();
            LogOsuDatabaseLoaded(beatmapCount, beatmapSetCount);

            return true;
        }

        if (explicitPath != default)
        {
            LogOsuDatabaseNotFound(explicitPath);
        }

        return false;
    }

    private static string? ResolveOsuLocation(string? path, bool preferStable, bool preferLazer)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            OsuPathResult osuPath = OsuPathResolver.GetOsuOrLazerPath();

            if (osuPath.Type is OsuType.None)
            {
                return null;
            }

            OsuType selectedType = osuPath.Type;
            if (preferStable && osuPath.StablePath != default)
            {
                selectedType = OsuType.Stable;
                path = osuPath.StablePath;
            }
            else if (preferLazer && osuPath.LazerPath != default)
            {
                selectedType = OsuType.Lazer;
                path = osuPath.LazerPath;
            }
            else
            {
                path = osuPath.Path;
            }

            return Path.Combine(path, selectedType.GetDatabaseFileName());
        }

        if (Path.HasExtension(path))
        {
            return path;
        }

        if (OsuPathResolver.IsOsuStableDirectory(path))
        {
            return Path.Combine(path, OsuType.Stable.GetDatabaseFileName());
        }

        if (OsuPathResolver.IsOsuLazerDataDirectory(path))
        {
            return Path.Combine(path, OsuType.Lazer.GetDatabaseFileName());
        }

        return path;
    }

    public CollectionLoadResult LoadCollectionsFromFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Collection file not found: {path}");
        }

        CollectionLoadResult loaded = _fileIo.CollectionLoader.LoadCollection(path);

        if (loaded.Collections.Count > 0)
        {
            CollectionEditArgs args = CollectionEditArgs.AddCollections(loaded.Collections);
            Manager.EditCollection(args);
        }

        return loaded;
    }

    public void SaveCollectionsToFile(string path, LazerRealmSchemaVersion targetSchemaVersion)
        => _fileIo.CollectionLoader.SaveCollection(Collections, path, targetSchemaVersion);

    public StableOsuDatabaseData? GetStableOsuDatabaseData() => _fileIo.OsuDatabase.StableOsuDatabaseData;

    public IOsuCollection? GetCollection(int? id = default, string? name = default)
    {
        if (id.HasValue)
        {
            return Manager.GetCollectionById(id.Value);
        }

        if (!string.IsNullOrEmpty(name))
        {
            return Manager.GetCollectionByName(name);
        }

        return null;
    }

    public IEnumerable<IOsuCollection> GetCollections<T>(IEnumerable<T> identifiers, Func<IOsuCollection, T> selector)
    {
        HashSet<T> identifierSet = [.. identifiers];
        return Collections.Where(c => identifierSet.Contains(selector(c)));
    }

    public void Dispose() => _fileIo?.Dispose();

    [LoggerMessage(Level = LogLevel.Information, Message = "Using osu! database found at \"{Path}\".")]
    private partial void LogOsuDatabaseFound(string path);

    [LoggerMessage(Level = LogLevel.Information, Message = "osu! database loaded successfully ({BeatmapCount} beatmaps, {BeatmapSetCount} beatmap sets).")]
    private partial void LogOsuDatabaseLoaded(int beatmapCount, int beatmapSetCount);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Could not find osu! database at \"{Path}\".")]
    private partial void LogOsuDatabaseNotFound(string path);
}
