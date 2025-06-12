namespace CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using System.Text;

public class ReorderStrategy : ICollectionEditStrategy
{
    private readonly string ReorderCharsString;
    private const string reorderSeparator = "| ";
    private readonly Lazy<Dictionary<string, Func<IOsuCollection, object>>> CollectionFieldSortSelectors = new(() => new()
    {
        { nameof(IOsuCollection.Id), c => c.Id },
        { nameof(IOsuCollection.Name), c => c.Name },
        { nameof(IOsuCollection.NumberOfMissingBeatmaps), c => c.NumberOfMissingBeatmaps },
        { nameof(IOsuCollection.NumberOfBeatmaps), c => c.NumberOfBeatmaps },
    });

    public ReorderStrategy(string reorderChars = "0123456789")
    {
        ReorderCharsString = reorderChars;
    }

    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        if (args is not CollectionReorderEditArgs reorderEditArgs)
        {
            throw new InvalidOperationException("Invalid args for reorder action.");
        }

        List<IOsuCollection> argCollections = manager.GetCollectionByNames(args.CollectionNames);

        if (!CollectionFieldSortSelectors.Value.TryGetValue(reorderEditArgs.SortColumn, out Func<IOsuCollection, object> sortFieldSelector))
        {
            throw new InvalidOperationException("Unrecognized collection sort column");
        }

        List<IOsuCollection> collectionsToReorder = argCollections.OrderByDescending(sortFieldSelector).ToList();
        List<IOsuCollection> orderedLoadedCollections = manager.LoadedCollections.OrderByDescending(sortFieldSelector).ToList();

        if (reorderEditArgs.SortOrder == SortOrder.Ascending)
        {
            collectionsToReorder.Reverse();
            orderedLoadedCollections.Reverse();
        }

        IOsuCollection targetCollection = manager.GetCollectionByName(reorderEditArgs.TargetSortCollectionName);

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