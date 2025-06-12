namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using System.Collections.Generic;

public class CollectionReorderEditArgs : CollectionEditArgs
{
    private CollectionReorderEditArgs() : base(CollectionEdit.Reorder)
    {
    }

    /// <summary>
    /// Indicates whether to place the collections before the target collection.
    /// </summary>
    public bool PlaceCollectionsBefore { get; protected set; }

    /// <summary>
    /// The name of the collection to use as the sort target.
    /// </summary>
    public string TargetSortCollectionName { get; protected set; }

    /// <summary>
    /// The column by which to sort the collections.
    /// </summary>
    public string SortColumn { get; protected set; }

    /// <summary>
    /// The order in which to sort the collections.
    /// </summary>
    public SortOrder SortOrder { get; protected set; }

    /// <summary>
    /// Creates arguments for reordering collections using special characters placed at the beginning of the name. This modifies all collection names.
    /// </summary>
    /// <param name="collectionNames">The collections to reorder/move.</param>
    /// <param name="targetCollectionSortName">The name of the collection to use as the reorder target.</param>
    /// <param name="placeBefore">Indicates whether to place the collections before the target collection.</param>
    /// <param name="sortColumn">The column by which to sort all collections.</param>
    /// <param name="sortOrder">The order in which to sort all collections.</param>
    /// <returns>A new <see cref="CollectionReorderEditArgs"/> instance.</returns>
    public static new CollectionReorderEditArgs ReorderCollections(IReadOnlyList<string> collectionNames, string targetCollectionSortName, bool placeBefore, string sortColumn, SortOrder sortOrder) => new()
    {
        CollectionNames = collectionNames,
        TargetSortCollectionName = targetCollectionSortName,
        PlaceCollectionsBefore = placeBefore,
        SortColumn = sortColumn,
        SortOrder = sortOrder
    };
}
