using CollectionManager.DataTypes;
using CollectionManager.Enums;
using System;
using System.Collections.Generic;

namespace CollectionManager.Modules.CollectionsManager
{
    public class CollectionEditArgs : EventArgs
    {
        public CollectionEdit Action { get; protected set; }
        public string OrginalName { get; protected set; }
        public string NewName { get; protected set; }
        public Collections Collections { get; protected set; }
        public Collection TargetCollection { get; protected set; }
        public Beatmaps Beatmaps { get; protected set; }
        public IList<string> CollectionNames { get; protected set; }
        public bool PlaceCollectionsBefore { get; protected set; }
        public string SortColumn { get; private set; }
        public int SortOrder { get; private set; }

        public CollectionEditArgs(CollectionEdit action)
        {
            Action = action;
        }
        #region Add Collection
        public static CollectionEditArgs AddCollections(Collections collections)
        {
            return new CollectionEditArgs(CollectionEdit.Add)
            {
                Collections = collections
            };
        }
        #endregion
        #region Remove Collection
        public static CollectionEditArgs RemoveCollections(IList<string> collectionNames)
        {
            return new CollectionEditArgs(CollectionEdit.Remove)
            {
                CollectionNames = collectionNames
            };
        }
        public static CollectionEditArgs RemoveCollections(Collections collections)
        {
            var names = new List<string>();
            foreach (var collection in collections)
            {
                names.Add(collection.Name);
            }
            return RemoveCollections(names);
        }
        #endregion
        #region Rename Collection
        public static CollectionEditArgs RenameCollection(string oldName, string NewName)
        {
            return new CollectionEditArgs(CollectionEdit.Rename)
            {
                OrginalName = oldName,
                NewName = NewName
            };
        }
        public static CollectionEditArgs RenameCollection(ICollection collection, string newName)
        {
            return RenameCollection(collection.Name, newName);
        }
        #endregion
        #region merge Collections
        public static CollectionEditArgs MergeCollections(Collections collections, string newName)
        {
            return new CollectionEditArgs(CollectionEdit.Merge)
            {
                Collections = collections,
                NewName = newName
            };
        }
        #endregion
        #region intersect Collections
        public static CollectionEditArgs IntersectCollections(Collections collections, string newName)
        {
            return new CollectionEditArgs(CollectionEdit.Intersect)
            {
                Collections = collections,
                NewName = newName
            };
        }
        #endregion
        #region Inverse Collections
        public static CollectionEditArgs InverseCollections(Collections collections, string newName)
        {
            return new CollectionEditArgs(CollectionEdit.Inverse)
            {
                Collections = collections,
                NewName = newName
            };
        }
        #endregion
        #region Difference Collections
        public static CollectionEditArgs DifferenceCollections(Collections collections, string newName)
        {
            return new CollectionEditArgs(CollectionEdit.Difference)
            {
                Collections = collections,
                NewName = newName
            };
        }
        #endregion
        #region Clear Collections
        public static CollectionEditArgs ClearCollections()
        {
            return new CollectionEditArgs(CollectionEdit.Clear);
        }
        #endregion
        #region Add Beatmaps to collection
        public static CollectionEditArgs AddBeatmaps(string collectionName, Beatmaps beatmaps)
        {
            return new CollectionEditArgs(CollectionEdit.AddBeatmaps)
            {
                Beatmaps = beatmaps,
                OrginalName = collectionName
            };
        }

        #endregion
        #region Reorder collections using special characters placed at the begining of the name, this modifies ALL collection names
        public static CollectionEditArgs ReorderCollections(Collections collections, Collection targetCollection, bool placeBefore, string sortColumn, int sortOrder)
        {
            return new CollectionEditArgs(CollectionEdit.Reorder)
            {
                Collections = collections,
                TargetCollection = targetCollection,
                PlaceCollectionsBefore = placeBefore,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };
        }
        #endregion
        #region Remove beatmaps from collection
        public static CollectionEditArgs RemoveBeatmaps(string collectionName, Beatmaps beatmaps)
        {
            return new CollectionEditArgs(CollectionEdit.RemoveBeatmaps)
            {
                Beatmaps = beatmaps,
                OrginalName = collectionName
            };
        }
        #endregion
        #region Add or merge if exists
        public static CollectionEditArgs AddOrMergeCollections(Collections collections)
        {
            return new CollectionEditArgs(CollectionEdit.AddOrMergeIfExists)
            {
                Collections = collections
            };
        }
        #endregion

        #region duplicate
        public static CollectionEditArgs DuplicateCollection(ICollection collection)
        {
            return new CollectionEditArgs(CollectionEdit.Duplicate)
            {
                OrginalName = collection.Name,
                Collections = new Collections() { collection }
            };
        }

        #endregion

    }
}