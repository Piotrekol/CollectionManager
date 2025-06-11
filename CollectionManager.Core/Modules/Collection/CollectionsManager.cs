namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CollectionsManager : ICollectionEditor, ICollectionNameValidator
{
    public OsuCollections LoadedCollections { get; } = [];
    private MapCacher MapCacher;
    private readonly string ReorderCharsString;
    private const string reorderSeparator = "| ";
    private int CollectionLoadId;
    private readonly Lazy<Dictionary<string, Func<IOsuCollection, object>>> CollectionFieldSortSelectors = new(() => new()
    {
        { nameof(IOsuCollection.Id), c => c.Id },
        { nameof(IOsuCollection.Name), c => c.Name },
        { nameof(IOsuCollection.NumberOfMissingBeatmaps), c => c.NumberOfMissingBeatmaps },
        { nameof(IOsuCollection.NumberOfBeatmaps), c => c.NumberOfBeatmaps },
    });

    public CollectionsManager(MapCacher loadedBeatmaps)
    {
        MapCacher = loadedBeatmaps;
        ReorderCharsString = "0123456789";//!$)]},.? ABCDEFGHIJKLMNOPQRSTUVWXYZ @#%^&*(+[{;':\\\"<>/
    }

    private void EditCollection(CollectionEditArgs args, bool suspendRefresh = false)
    {
        CollectionEdit action = args.Action;
        if ((int)action >= 100)
        {
            return;
        }

        List<IOsuCollection> argCollections = [.. args.CollectionNames.Select(GetCollectionByName).Where(c => c != null)];

        if (action == CollectionEdit.Add)
        {
            List<string> collectionNames = [];
            foreach (IOsuCollection collection in args.NewCollections)
            {
                string name = GetValidCollectionName(collection.Name, collectionNames);

                collection.Name = name;
                collection.Id = CollectionLoadId++;
                collectionNames.Add(name);
            }

            LoadedCollections.AddRange(args.NewCollections);
        }
        else if (action == CollectionEdit.AddOrMergeIfExists)
        {
            foreach (IOsuCollection collection in argCollections)
            {
                if (CollectionNameExists(collection.Name))
                {
                    EditCollection(CollectionEditArgs.MergeCollections([collection.Name, collection.Name], collection.Name), true);
                }
                else
                {
                    EditCollection(CollectionEditArgs.AddCollections([collection]), true);
                }
            }
        }
        else if (action == CollectionEdit.Remove)
        {
            foreach (string collectionName in args.CollectionNames)
            {
                IOsuCollection collection = GetCollectionByName(collectionName);
                if (collection != null)
                {
                    LoadedCollections.SilentRemove(collection);
                }
            }
        }
        else if (action == CollectionEdit.Merge)
        {
            string newCollectionName = args.NewName;
            if (argCollections.Count > 0)
            {
                IOsuCollection masterCollection = argCollections[0];
                for (int i = 1; i < argCollections.Count; i++)
                {
                    IOsuCollection collectionToMerge = argCollections[i];
                    foreach (BeatmapExtension beatmap in collectionToMerge.AllBeatmaps())
                    {
                        masterCollection.AddBeatmap(beatmap);
                    }

                    LoadedCollections.SilentRemove(collectionToMerge);
                }

                LoadedCollections.SilentRemove(masterCollection);

                masterCollection.Name = GetValidCollectionName(newCollectionName);
                EditCollection(CollectionEditArgs.AddCollections([masterCollection]), true);
            }
        }
        else if (action == CollectionEdit.Intersect)
        {
            OsuCollection targetCollection = new(MapCacher) { Name = args.NewName };
            IOsuCollection mainCollection = argCollections[0];
            argCollections.RemoveAt(0);
            IEnumerable<BeatmapExtension> beatmaps = mainCollection.AllBeatmaps();
            foreach (IOsuCollection collection in argCollections)
            {
                beatmaps = beatmaps.Intersect(collection.AllBeatmaps(), new CollectionBeatmapComparer()).ToList();
            }

            foreach (BeatmapExtension beatmap in beatmaps)
            {
                targetCollection.AddBeatmap(beatmap);
            }

            EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
        }
        else if (action == CollectionEdit.Inverse)
        {
            OsuCollection targetCollection = new(MapCacher) { Name = args.NewName };
            IEnumerable<BeatmapExtension> beatmaps = MapCacher.Beatmaps.AsEnumerable().Cast<BeatmapExtension>();
            foreach (IOsuCollection collection in argCollections)
            {
                beatmaps = beatmaps.Except(collection.AllBeatmaps(), new CollectionBeatmapComparer());
            }

            foreach (BeatmapExtension beatmap in beatmaps)
            {
                targetCollection.AddBeatmap(beatmap);
            }

            EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
        }
        else if (action == CollectionEdit.Difference)
        {
            OsuCollection targetCollection = new(MapCacher) { Name = args.NewName };
            IOsuCollection mainCollection = argCollections[0];
            argCollections.RemoveAt(0);
            IEnumerable<BeatmapExtension> beatmaps = mainCollection.AllBeatmaps();
            foreach (IOsuCollection collection in argCollections)
            {
                beatmaps = beatmaps.Concat(collection.AllBeatmaps());
            }

            List<string> differenceMd5 = beatmaps.GroupBy(x => x.Md5).Where(group => group.Count() == 1).Select(group => group.Key).ToList();
            List<int> differenceMapId = beatmaps.GroupBy(x => x.MapId).Where(group => group.Count() == 1).Select(group => group.Key).ToList();

            foreach (BeatmapExtension beatmap in beatmaps)
            {
                if (differenceMd5.Contains(beatmap.Md5) || differenceMapId.Contains(beatmap.MapId))
                {
                    targetCollection.AddBeatmap(beatmap);
                }
            }

            EditCollection(CollectionEditArgs.AddCollections([targetCollection]), true);
        }
        else if (action == CollectionEdit.Clear)
        {
            LoadedCollections.Clear();
        }
        else if (action == CollectionEdit.Reorder && args is CollectionReorderEditArgs reorderEditArgs)
        {
            if (!CollectionFieldSortSelectors.Value.TryGetValue(reorderEditArgs.SortColumn, out Func<IOsuCollection, object> sortFieldSelector))
            {
                throw new InvalidOperationException("Unrecognized collection sort column");
            }

            List<IOsuCollection> collectionsToReorder = argCollections.OrderByDescending(sortFieldSelector).ToList();
            List<IOsuCollection> orderedLoadedCollections = LoadedCollections.OrderByDescending(sortFieldSelector).ToList();
            if (reorderEditArgs.SortOrder == SortOrder.Ascending)
            {
                collectionsToReorder.Reverse();
                orderedLoadedCollections.Reverse();
            }

            IOsuCollection targetCollection = GetCollectionByName(reorderEditArgs.TargetSortCollectionName);
            foreach (IOsuCollection coll in collectionsToReorder)
            {
                _ = orderedLoadedCollections.Remove(coll);
            }

            int targetCollectionIndex = orderedLoadedCollections.IndexOf(targetCollection);
            orderedLoadedCollections.InsertRange(reorderEditArgs.PlaceCollectionsBefore ? targetCollectionIndex : targetCollectionIndex + 1, collectionsToReorder);
            int amountOfCharactersRequired = 0;
            int variations = 0;
            while (orderedLoadedCollections.Count > variations)
            {
                variations = Enumerable.Range(1, ++amountOfCharactersRequired).Aggregate(0, (acc, i) => Convert.ToInt32(Math.Pow(ReorderCharsString.Length, i)) + acc);
            }

            List<string> reorderStrings = new(variations);
            for (int i = 1; i <= amountOfCharactersRequired; i++)
            {
                reorderStrings.AddRange(CombinationsWithRepetition(ReorderCharsString, i));
            }

            reorderStrings.Sort();
            int collectionIndex = 0;
            foreach (IOsuCollection collection in orderedLoadedCollections)
            {
                if (collection.Name.Contains(reorderSeparator))
                {
                    collection.Name = collection.Name.Substring(collection.Name.IndexOf(reorderSeparator) + reorderSeparator.Length + 1);
                }

                collection.Name = $"{reorderStrings[collectionIndex++]}{reorderSeparator} {collection.Name}";
            }
        }
        else
        {
            IOsuCollection collection = GetCollectionByName(args.CollectionNames[0]);

            if (action == CollectionEdit.Rename)
            {
                collection.Name = GetValidCollectionName(args.NewName);
            }
            else if (action == CollectionEdit.AddBeatmaps)
            {
                if (collection != null)
                {
                    foreach (Beatmap beatmap in args.Beatmaps)
                    {
                        collection.AddBeatmap(beatmap);
                    }
                }
            }
            else if (action == CollectionEdit.RemoveBeatmaps)
            {
                if (collection != null)
                {
                    foreach (Beatmap beatmap in args.Beatmaps)
                    {
                        _ = collection.RemoveBeatmap(beatmap.Md5);
                    }
                }
            }
            else if (action == CollectionEdit.Duplicate)
            {
                IOsuCollection sourceCollection = GetCollectionByName(args.CollectionNames[0]);
                OsuCollection newCollection = new(MapCacher) { Name = args.NewName };

                foreach (BeatmapExtension beatmap in sourceCollection.AllBeatmaps())
                {
                    newCollection.AddBeatmap(beatmap);
                }

                EditCollection(CollectionEditArgs.AddCollections([newCollection]), true);
            }
        }

        if (!suspendRefresh)
        {
            AfterCollectionsEdit();
        }
    }
    public void EditCollection(CollectionEditArgs args) => EditCollection(args, false);

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
    {
        OsuCollections collections = [];
        string[] hashes = beatmaps.Select(b => b.Md5).ToArray();
        collections.AddRange(LoadedCollections.Where(c => hashes.Intersect(c.BeatmapHashes).Any()));
        return collections;
    }

    protected virtual void AfterCollectionsEdit() => LoadedCollections.CallReset();
    public IOsuCollection GetCollectionByName(string collectionName)
    {
        for (int i = 0; i < LoadedCollections.Count; i++)
        {
            if (LoadedCollections[i].Name == collectionName)
            {
                return LoadedCollections[i];
            }
        }

        return null;
    }

    public string GetValidCollectionName(string desiredName, List<string> aditionalNames = null)
    {
        string newName = desiredName;
        int c = 0;
        while (CollectionNameExists(newName) || (aditionalNames != null && aditionalNames.Contains(newName)))
        {
            newName = desiredName + "_" + c++;
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

    public bool IsCollectionNameValid(string name) => !CollectionNameExists(name);

    private static string Combinations(string symbols, int number, int stringLength)
    {
        StringBuilder stringBuilder = new();
        int len = symbols.Length;
        char nullSym = symbols[0];
        while (number > 0)
        {
            int index = number % len;
            number /= len;
            _ = stringBuilder.Insert(0, symbols[index]);
        }

        return stringBuilder.ToString().PadLeft(stringLength, nullSym);
    }

    private static IEnumerable<string> CombinationsWithRepetition(string symbols, int stringLength)
    {
        for (int i = 0; i < Math.Pow(symbols.Length, stringLength); i++)
        {
            yield return Combinations(symbols, i, stringLength);
        }
    }
}