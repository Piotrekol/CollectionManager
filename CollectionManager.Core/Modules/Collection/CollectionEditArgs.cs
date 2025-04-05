namespace CollectionManager.Core.Modules.Collection;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;

public class CollectionEditArgs : EventArgs
{
    public CollectionEdit Action { get; protected set; }
    public string OrginalName { get; protected set; }
    public string NewName { get; protected set; }
    public OsuCollections Collections { get; protected set; }
    public OsuCollection TargetCollection { get; protected set; }
    public Beatmaps Beatmaps { get; protected set; }
    public IList<string> CollectionNames { get; protected set; }
    public bool PlaceCollectionsBefore { get; protected set; }
    public string SortColumn { get; private set; }
    public SortOrder SortOrder { get; private set; }

    public CollectionEditArgs(CollectionEdit action)
    {
        Action = action;
    }
    #region Add Collection
    public static CollectionEditArgs AddCollections(OsuCollections collections) => new(CollectionEdit.Add)
    {
        Collections = collections
    };
    #endregion
    #region Remove Collection
    public static CollectionEditArgs RemoveCollections(IList<string> collectionNames) => new(CollectionEdit.Remove)
    {
        CollectionNames = collectionNames
    };
    public static CollectionEditArgs RemoveCollections(OsuCollections collections)
    {
        List<string> names = [];
        foreach (IOsuCollection collection in collections)
        {
            names.Add(collection.Name);
        }

        return RemoveCollections(names);
    }
    #endregion
    #region Rename Collection
    public static CollectionEditArgs RenameCollection(string oldName, string NewName) => new(CollectionEdit.Rename)
    {
        OrginalName = oldName,
        NewName = NewName
    };
    public static CollectionEditArgs RenameCollection(IOsuCollection collection, string newName) => RenameCollection(collection.Name, newName);
    #endregion
    #region merge Collections
    public static CollectionEditArgs MergeCollections(OsuCollections collections, string newName) => new(CollectionEdit.Merge)
    {
        Collections = collections,
        NewName = newName
    };
    #endregion
    #region intersect Collections
    public static CollectionEditArgs IntersectCollections(OsuCollections collections, string newName) => new(CollectionEdit.Intersect)
    {
        Collections = collections,
        NewName = newName
    };
    #endregion
    #region Inverse Collections
    public static CollectionEditArgs InverseCollections(OsuCollections collections, string newName) => new(CollectionEdit.Inverse)
    {
        Collections = collections,
        NewName = newName
    };
    #endregion
    #region Difference Collections
    public static CollectionEditArgs DifferenceCollections(OsuCollections collections, string newName) => new(CollectionEdit.Difference)
    {
        Collections = collections,
        NewName = newName
    };
    #endregion
    #region Clear Collections
    public static CollectionEditArgs ClearCollections() => new(CollectionEdit.Clear);
    #endregion
    #region Add Beatmaps to collection
    public static CollectionEditArgs AddBeatmaps(string collectionName, Beatmaps beatmaps) => new(CollectionEdit.AddBeatmaps)
    {
        Beatmaps = beatmaps,
        OrginalName = collectionName
    };

    #endregion
    #region Reorder collections using special characters placed at the begining of the name, this modifies ALL collection names
    public static CollectionEditArgs ReorderCollections(OsuCollections collections, OsuCollection targetCollection, bool placeBefore, string sortColumn, SortOrder sortOrder) => new(CollectionEdit.Reorder)
    {
        Collections = collections,
        TargetCollection = targetCollection,
        PlaceCollectionsBefore = placeBefore,
        SortColumn = sortColumn,
        SortOrder = sortOrder
    };
    #endregion
    #region Remove beatmaps from collection
    public static CollectionEditArgs RemoveBeatmaps(string collectionName, Beatmaps beatmaps) => new(CollectionEdit.RemoveBeatmaps)
    {
        Beatmaps = beatmaps,
        OrginalName = collectionName
    };
    #endregion
    #region Add or merge if exists
    public static CollectionEditArgs AddOrMergeCollections(OsuCollections collections) => new(CollectionEdit.AddOrMergeIfExists)
    {
        Collections = collections
    };
    #endregion

    #region duplicate
    public static CollectionEditArgs DuplicateCollection(IOsuCollection collection) => new(CollectionEdit.Duplicate)
    {
        OrginalName = collection.Name,
        Collections = [collection]
    };

    #endregion

}