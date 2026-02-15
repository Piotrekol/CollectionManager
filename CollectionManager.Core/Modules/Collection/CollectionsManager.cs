namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using System.Linq;

public class CollectionsManager : ICollectionEditor, ICollectionNameValidator
{
    public OsuCollections LoadedCollections { get; } = [];
    private readonly Dictionary<CollectionEdit, ICollectionEditStrategy> _strategies;

    private static Dictionary<CollectionEdit, ICollectionEditStrategy> CreateDefaultStrategies(MapCacher mapCacher) => new() {
            { CollectionEdit.Add, new AddStrategy() },
            { CollectionEdit.AddOrMergeIfExists, new AddOrMergeIfExistsStrategy() },
            { CollectionEdit.Remove, new RemoveStrategy() },
            { CollectionEdit.Merge, new MergeStrategy() },
            { CollectionEdit.Intersect, new IntersectStrategy(mapCacher) },
            { CollectionEdit.Inverse, new InverseStrategy(mapCacher) },
            { CollectionEdit.Difference, new DifferenceStrategy(mapCacher) },
            { CollectionEdit.Clear, new ClearStrategy() },
            { CollectionEdit.Reorder, new ReorderStrategy() },
            { CollectionEdit.Rename, new RenameStrategy() },
            { CollectionEdit.AddBeatmaps, new AddBeatmapsStrategy() },
            { CollectionEdit.RemoveBeatmaps, new RemoveBeatmapsStrategy() },
            { CollectionEdit.Duplicate, new DuplicateStrategy(mapCacher) }
        };

    public CollectionsManager(MapCacher loadedBeatmaps)
        : this(loadedBeatmaps, [])
    {
    }

    public CollectionsManager(MapCacher loadedBeatmaps, Dictionary<CollectionEdit, ICollectionEditStrategy> collectionEditStrategies)
    {
        _strategies = CreateDefaultStrategies(loadedBeatmaps);

        if (collectionEditStrategies?.Count is not 0)
        {
            foreach (KeyValuePair<CollectionEdit, ICollectionEditStrategy> strategy in collectionEditStrategies)
            {
                _strategies[strategy.Key] = strategy.Value;
            }
        }
    }

    /// <summary>
    /// Performs an edit on the collections based on the provided <see cref="CollectionEditArgs"/>.
    /// </summary>
    /// <param name="args">Arguments containing the action to perform.</param>
    /// <param name="suspendRefresh">Whether to suspend the refresh of collections after the edit. Use when performing several edit actions in quick succession.</param>
    public void EditCollection(CollectionEditArgs args, bool suspendRefresh = false)
    {
        CollectionEdit action = args.Action;

        if (_strategies.TryGetValue(action, out ICollectionEditStrategy strategy))
        {
            strategy.Execute(this, args);
        }

        if (!suspendRefresh)
        {
            AfterCollectionsEdit();
        }
    }

    public void EditCollection(CollectionEditArgs args) => EditCollection(args, false);

    public void EditCollection(IReadOnlyList<CollectionEditArgs> args)
    {
        if (args is null || args.Count is 0)
        {
            return;
        }

        foreach (CollectionEditArgs arg in args)
        {
            EditCollection(arg, true);
        }

        AfterCollectionsEdit();
    }

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
    {
        OsuCollections collections = [];
        string[] hashes = beatmaps.Select(b => b.Md5).ToArray();
        collections.AddRange(LoadedCollections.Where(c => hashes.Intersect(c.BeatmapHashes).Any()));
        return collections;
    }

    public IOsuCollection GetCollectionByName(string collectionName) =>
        LoadedCollections.FirstOrDefault(c => c.Name == collectionName);

    public List<IOsuCollection> GetCollectionByNames(IReadOnlyList<string> collectionNames) =>
            [.. collectionNames.Select(GetCollectionByName)];

    public string GetValidCollectionName(string desiredName, IReadOnlyList<string> additionalReservedNames = null)
    {
        int c = 0;
        string newName = desiredName;
        IReadOnlyList<string> reservedNames = additionalReservedNames ?? Array.Empty<string>();

        while (!IsCollectionNameValid(newName) || reservedNames.Contains(newName))
        {
            newName = $"{desiredName}_{c++}";
        }

        return newName;
    }

    public bool CollectionNameExists(string name)
    {
        foreach (IOsuCollection collection in LoadedCollections)
        {
            if (collection.Name == name)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCollectionNameValid(string name) => !(string.IsNullOrEmpty(name) || CollectionNameExists(name));

    protected virtual void AfterCollectionsEdit() => LoadedCollections.CallReset();
}