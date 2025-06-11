namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides arguments for editing collections, as well as factory methods for actions.
/// </summary>
public class CollectionEditArgs : EventArgs
{

    /// <summary>
    /// The type of action being performed.
    /// </summary>
    public CollectionEdit Action { get; protected set; }

    /// <summary>
    /// The name of the new collection, if applicable.
    /// </summary>
    public string NewName { get; protected set; }

    /// <summary>
    /// The names of the existing collections involved in the action.
    /// </summary>
    public IReadOnlyList<string> CollectionNames { get; protected set; } = [];

    /// <summary>
    /// The beatmaps involved in the action, if applicable.
    /// </summary>
    public IEnumerable<Beatmap> Beatmaps { get; protected set; } = [];

    public OsuCollections NewCollections { get; protected set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionEditArgs"/> class with the specified action.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    public CollectionEditArgs(CollectionEdit action)
    {
        Action = action;
    }

    /// <summary>
    /// Creates arguments for adding new collections.
    /// </summary>
    /// <param name="collections">The collections to add.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the add action.</returns>
    public static CollectionEditArgs AddCollections(OsuCollections collections) => new(CollectionEdit.Add)
    {
        NewCollections = collections
    };

    /// <summary>
    /// Creates arguments for removing collections.
    /// </summary>
    /// <param name="collectionNames">The names of the collections to remove.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the remove action.</returns>
    public static CollectionEditArgs RemoveCollections(IReadOnlyList<string> collectionNames) => new(CollectionEdit.Remove)
    {
        CollectionNames = collectionNames
    };

    /// <summary>
    /// Creates arguments for renaming a collection.
    /// </summary>
    /// <param name="oldName">The current name of the collection.</param>
    /// <param name="NewName">The new name for the collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the rename action.</returns>
    public static CollectionEditArgs RenameCollection(string oldName, string NewName) => new(CollectionEdit.Rename)
    {
        CollectionNames = [oldName],
        NewName = NewName
    };

    /// <summary>
    /// Creates arguments for renaming a collection by <see cref="IOsuCollection"/>.
    /// </summary>
    /// <param name="collection">The collection to rename.</param>
    /// <param name="newName">The new name for the collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the rename action.</returns>
    public static CollectionEditArgs RenameCollection(IOsuCollection collection, string newName) => RenameCollection(collection.Name, newName);

    /// <summary>
    /// Creates arguments for merging collections.
    /// </summary>
    /// <param name="collections">The collections to merge.</param>
    /// <param name="newName">The name for the merged collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the merge action.</returns>
    public static CollectionEditArgs MergeCollections(IReadOnlyList<string> collectionNames, string newName) => new(CollectionEdit.Merge)
    {
        CollectionNames = collectionNames,
        NewName = newName
    };

    /// <summary>
    /// Creates arguments for intersecting collections.
    /// </summary>
    /// <param name="collectionNames">The collections to intersect.</param>
    /// <param name="newName">The name for the intersected collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the intersect action.</returns>
    public static CollectionEditArgs IntersectCollections(IReadOnlyList<string> collectionNames, string newName) => new(CollectionEdit.Intersect)
    {
        CollectionNames = collectionNames,
        NewName = newName
    };

    /// <summary>
    /// Creates arguments for inverting collections.
    /// </summary>
    /// <param name="collectionNames">The collections to invert.</param>
    /// <param name="newName">The name for the inverted collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the inverse action.</returns>
    public static CollectionEditArgs InverseCollections(IReadOnlyList<string> collectionNames, string newName) => new(CollectionEdit.Inverse)
    {
        CollectionNames = collectionNames,
        NewName = newName
    };

    /// <summary>
    /// Creates arguments for finding the difference between collections.
    /// </summary>
    /// <param name="collectionNames">The collections to compare.</param>
    /// <param name="newName">The name for the resulting collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the difference action.</returns>
    public static CollectionEditArgs DifferenceCollections(IReadOnlyList<string> collectionNames, string newName) => new(CollectionEdit.Difference)
    {
        CollectionNames = collectionNames,
        NewName = newName
    };

    /// <summary>
    /// Creates arguments for clearing all collections.
    /// </summary>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the clear action.</returns>
    public static CollectionEditArgs ClearCollections() => new(CollectionEdit.Clear);

    /// <summary>
    /// Creates arguments for adding beatmaps to a collection.
    /// </summary>
    /// <param name="collectionName">The name of the collection to add beatmaps to.</param>
    /// <param name="beatmaps">The beatmaps to add.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the add beatmaps action.</returns>
    public static CollectionEditArgs AddBeatmaps(string collectionName, IEnumerable<Beatmap> beatmaps) => new(CollectionEdit.AddBeatmaps)
    {
        Beatmaps = beatmaps,
        CollectionNames = [collectionName]
    };

    /// <summary>
    /// Creates arguments for removing beatmaps from a collection.
    /// </summary>
    /// <param name="collectionName">The name of the collection to remove beatmaps from.</param>
    /// <param name="beatmaps">The beatmaps to remove.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the remove beatmaps action.</returns>
    public static CollectionEditArgs RemoveBeatmaps(string collectionName, IEnumerable<Beatmap> beatmaps) => new(CollectionEdit.RemoveBeatmaps)
    {
        Beatmaps = beatmaps,
        CollectionNames = [collectionName]
    };

    /// <summary>
    /// Creates arguments for adding or merging collections if they already exist.
    /// </summary>
    /// <param name="collectionNames">The names of the collections to add or merge.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the add or merge action.</returns>
    public static CollectionEditArgs AddOrMergeCollections(IReadOnlyList<string> collectionNames) => new(CollectionEdit.AddOrMergeIfExists)
    {
        CollectionNames = collectionNames
    };

    /// <summary>
    /// Creates arguments for duplicating a collection.
    /// </summary>
    /// <param name="collectionName">The name of the collection to duplicate.</param>
    /// <param name="newName">The new name for the duplicated collection.</param>
    /// <returns>A <see cref="CollectionEditArgs"/> instance for the duplicate action.</returns>
    public static CollectionEditArgs DuplicateCollection(string collectionName, string newName) => new(CollectionEdit.Duplicate)
    {
        CollectionNames = [collectionName],
        NewName = newName
    };

    /// <inheritdoc cref="CollectionReorderEditArgs.ReorderCollections(IReadOnlyList{string}, string, bool, string, SortOrder)"/>
    public static CollectionReorderEditArgs ReorderCollections(IReadOnlyList<string> collectionNames, string targetCollectionSortName, bool placeBefore, string sortColumn, SortOrder sortOrder)
        => CollectionReorderEditArgs.ReorderCollections(collectionNames, targetCollectionSortName, placeBefore, sortColumn, sortOrder);
}