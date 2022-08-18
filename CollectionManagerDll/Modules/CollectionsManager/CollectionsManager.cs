using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Interfaces;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;

namespace CollectionManager.Modules.CollectionsManager
{
    public class CollectionsManager : ICollectionEditor, ICollectionNameValidator
    {
        public readonly Collections LoadedCollections = new Collections();
        public Beatmaps LoadedBeatmaps;
        private readonly string ReorderCharsString;
        private const string reorderSeparator = "| ";
        private int CollectionLoadId = 0;
        private readonly Lazy<Dictionary<string, Func<ICollection, object>>> CollectionFieldSortSelectors = new(() => new()
        {
            { nameof(ICollection.Id), c => c.Id },
            { nameof(ICollection.Name), c => c.Name },
            { nameof(ICollection.NumberOfMissingBeatmaps), c => c.NumberOfMissingBeatmaps },
            { nameof(ICollection.NumberOfBeatmaps), c => c.NumberOfBeatmaps },
        });

        public CollectionsManager(Beatmaps loadedBeatmaps)
        {
            LoadedBeatmaps = loadedBeatmaps;
            ReorderCharsString = "0123456789";//!$)]},.? ABCDEFGHIJKLMNOPQRSTUVWXYZ @#%^&*(+[{;':\\\"<>/
        }

        /*
         add collection
         remove collection
         edit collection name
         merge x collections
         intersect x collections
         inverse map sum of x collections
         difference x collections
         clear collections
         reorder collections
         add beatmaps to collection
         remove beatmaps from collection
         */
        private void EditCollection(CollectionEditArgs args, bool suspendRefresh = false)
        {
            var action = args.Action;
            if ((int)action >= 100)
                return;

            if (action == CollectionEdit.Add)
            {
                List<string> collectionNames = new List<string>();
                foreach (var collection in args.Collections)
                {
                    var name = GetValidCollectionName(collection.Name, collectionNames);

                    collection.Name = name;
                    collection.Id = CollectionLoadId++;
                    collectionNames.Add(name);
                }
                LoadedCollections.AddRange(args.Collections);
            }
            else if (action == CollectionEdit.AddOrMergeIfExists)
            {
                foreach (var collection in args.Collections)
                {
                    if (CollectionNameExists(collection.Name))
                        EditCollection(CollectionEditArgs.MergeCollections(
                            new Collections() { GetCollectionByName(collection.Name), collection }, collection.Name), true);
                    else
                        EditCollection(CollectionEditArgs.AddCollections(new Collections() { collection }), true);
                }
            }
            else if (action == CollectionEdit.Remove)
            {
                foreach (var collectionName in args.CollectionNames)
                {
                    var collection = GetCollectionByName(collectionName);
                    if (collection != null)
                        LoadedCollections.SilentRemove(collection);
                }
            }
            else if (action == CollectionEdit.Merge)
            {
                var collections = args.Collections;
                var newCollectionName = args.NewName;
                if (collections.Count > 0)
                {
                    var masterCollection = collections[0];
                    for (int i = 1; i < collections.Count; i++)
                    {
                        var collectionToMerge = collections[i];
                        foreach (var beatmap in collectionToMerge.AllBeatmaps())
                        {
                            masterCollection.AddBeatmap(beatmap);
                        }
                        LoadedCollections.SilentRemove(collectionToMerge);
                    }
                    LoadedCollections.SilentRemove(masterCollection);

                    masterCollection.Name = GetValidCollectionName(newCollectionName);
                    EditCollection(CollectionEditArgs.AddCollections(new Collections() { masterCollection }), true);
                }
            }
            else if (action == CollectionEdit.Intersect)
            {
                var targetCollection = args.Collections.Last();
                args.Collections.RemoveAt(args.Collections.Count - 1);
                var mainCollection = args.Collections[0];
                args.Collections.RemoveAt(0);
                var beatmaps = mainCollection.AllBeatmaps();
                foreach (var collection in args.Collections)
                {
                    beatmaps = beatmaps.Intersect(collection.AllBeatmaps(), new CollectionBeatmapComparer()).ToList();
                }

                foreach (var beatmap in beatmaps)
                {
                    targetCollection.AddBeatmap(beatmap);
                }

                EditCollection(CollectionEditArgs.AddCollections(new Collections() { targetCollection }), true);
            }
            else if (action == CollectionEdit.Inverse)
            {
                var targetCollection = args.Collections.Last();
                args.Collections.RemoveAt(args.Collections.Count - 1);
                var beatmaps = LoadedBeatmaps.AsEnumerable().Cast<BeatmapExtension>();
                foreach (var collection in args.Collections)
                {
                    beatmaps = beatmaps.Except(collection.AllBeatmaps(), new CollectionBeatmapComparer());
                }

                foreach (var beatmap in beatmaps)
                {
                    targetCollection.AddBeatmap(beatmap);
                }

                EditCollection(CollectionEditArgs.AddCollections(new Collections() { targetCollection }), true);
            }
            else if (action == CollectionEdit.Difference)
            {
                var targetCollection = args.Collections.Last();
                args.Collections.RemoveAt(args.Collections.Count - 1);
                var mainCollection = args.Collections[0];
                args.Collections.RemoveAt(0);
                var beatmaps = mainCollection.AllBeatmaps();
                foreach (var collection in args.Collections)
                {
                    beatmaps = beatmaps.Concat(collection.AllBeatmaps());
                }

                var differenceMd5 = beatmaps.GroupBy(x => x.Md5).Where(group => group.Count() == 1).Select(group => group.Key).ToList();
                var differenceMapId = beatmaps.GroupBy(x => x.MapId).Where(group => group.Count() == 1).Select(group => group.Key).ToList();

                foreach (var beatmap in beatmaps)
                {
                    if (differenceMd5.Contains(beatmap.Md5) || differenceMapId.Contains(beatmap.MapId))
                    {
                        targetCollection.AddBeatmap(beatmap);
                    }
                }

                EditCollection(CollectionEditArgs.AddCollections(new Collections() { targetCollection }), true);
            }
            else if (action == CollectionEdit.Clear)
            {
                LoadedCollections.Clear();
            }
            else if (action == CollectionEdit.Reorder)
            {
                if (!CollectionFieldSortSelectors.Value.TryGetValue(args.SortColumn, out var sortFieldSelector))
                    throw new InvalidOperationException("Unrecognized collection sort column");

                var collectionsToReorder = args.Collections.OrderByDescending(sortFieldSelector).ToList();
                var orderedLoadedCollections = LoadedCollections.OrderByDescending(sortFieldSelector).ToList();
                if (args.SortOrder == SortOrder.Ascending)
                {
                    collectionsToReorder.Reverse();
                    orderedLoadedCollections.Reverse();
                }

                var targetCollection = args.TargetCollection;
                foreach (var coll in collectionsToReorder)
                    orderedLoadedCollections.Remove(coll);

                var targetCollectionIndex = orderedLoadedCollections.IndexOf(targetCollection);
                orderedLoadedCollections.InsertRange(args.PlaceCollectionsBefore ? targetCollectionIndex : targetCollectionIndex + 1, collectionsToReorder);
                var amountOfCharactersRequired = 0;
                var variations = 0;
                while (orderedLoadedCollections.Count() > variations)
                    variations = Enumerable.Range(1, ++amountOfCharactersRequired).Aggregate(0, (acc, i) => Convert.ToInt32(Math.Pow(ReorderCharsString.Length, i)) + acc);

                List<string> reorderStrings = new List<string>(variations);
                for (int i = 1; i <= amountOfCharactersRequired; i++)
                    reorderStrings.AddRange(CombinationsWithRepetition(ReorderCharsString, i));

                reorderStrings.Sort();
                var collectionIndex = 0;
                foreach (var collection in orderedLoadedCollections)
                {
                    if (collection.Name.Contains(reorderSeparator))
                        collection.Name = collection.Name.Substring(collection.Name.IndexOf(reorderSeparator) + reorderSeparator.Length + 1);

                    collection.Name = $"{reorderStrings[collectionIndex++]}{reorderSeparator} {collection.Name}";
                }
            }
            else
            {
                var collection = GetCollectionByName(args.OrginalName);

                if (action == CollectionEdit.Rename)
                {
                    collection.Name = GetValidCollectionName(args.NewName);
                }
                else if (action == CollectionEdit.AddBeatmaps)
                {
                    if (collection != null)
                    {
                        foreach (var beatmap in args.Beatmaps)
                        {
                            collection.AddBeatmap(beatmap);
                        }
                    }
                }
                else if (action == CollectionEdit.RemoveBeatmaps)
                {
                    if (collection != null)
                    {
                        foreach (var beatmap in args.Beatmaps)
                        {
                            collection.RemoveBeatmap(beatmap.Md5);
                        }
                    }
                }
                else if (action == CollectionEdit.Duplicate)
                {
                    throw new NotImplementedException("Call AddCollections followed with AddBeatmaps instead");
                }

            }
            if (!suspendRefresh)
                AfterCollectionsEdit();
        }
        public void EditCollection(CollectionEditArgs args)
        {
            EditCollection(args, false);
        }

        public Collections GetCollectionsForBeatmaps(Beatmaps beatmaps)
        {
            var collections = new Collections();
            var hashes = beatmaps.Select(b => b.Md5).ToArray();
            collections.AddRange(LoadedCollections.Where(c => hashes.Intersect(c.BeatmapHashes).Any()));
            return collections;
        }

        protected virtual void AfterCollectionsEdit()
        {
            LoadedCollections.CallReset();
        }
        public ICollection GetCollectionByName(string collectionName)
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
            var newName = desiredName;
            int c = 0;
            while (CollectionNameExists(newName) || (aditionalNames != null && aditionalNames.Contains(newName)))
            {
                newName = desiredName + "_" + c++;
            }

            return newName;
        }

        public bool CollectionNameExists(string name)
        {
            foreach (var collection in LoadedCollections)
            {
                if (collection.Name == name)
                    return true;
            }
            return false;
        }

        public bool IsCollectionNameValid(string name)
        {
            return !CollectionNameExists(name);
        }

        private static string Combinations(string symbols, int number, int stringLength)
        {
            var stringBuilder = new StringBuilder();
            var len = symbols.Length;
            var nullSym = symbols[0];
            while (number > 0)
            {
                var index = number % len;
                number = number / len;
                stringBuilder.Insert(0, symbols[index]);
            }

            return stringBuilder.ToString().PadLeft(stringLength, nullSym);
        }

        private static IEnumerable<string> CombinationsWithRepetition(string symbols, int stringLength)
        {
            for (var i = 0; i < Math.Pow(symbols.Length, stringLength); i++)
                yield return Combinations(symbols, i, stringLength);
        }
    }
}