namespace CollectionManager.App.Cli.Logging;

using CollectionManager.Core.Types;

internal static class CollectionLogger
{
    public static string FormatCollection(IOsuCollection collection)
        => FormatCollection(collection.Id, collection.Name);

    public static string FormatCollection(int id, string name)
        => $"[Id:{id}] {name}";

    public static string FormatCollectionWithCounts(IOsuCollection collection)
    {
        int total = collection.NumberOfBeatmaps;
        int missing = collection.NumberOfMissingBeatmaps;

        if (missing > 0)
        {
            return $"{FormatCollection(collection)} ({total} maps, {missing} missing)";
        }

        return $"{FormatCollection(collection)} ({total} maps)";
    }

    public static string FormatCollection(IOsuCollection collection, bool includeCounts)
        => includeCounts
            ? FormatCollectionWithCounts(collection)
            : FormatCollection(collection);
}
