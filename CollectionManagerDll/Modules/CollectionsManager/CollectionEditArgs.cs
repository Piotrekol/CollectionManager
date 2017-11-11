using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Enums;

namespace CollectionManager.Modules.CollectionsManager
{
    public class CollectionEditArgs : EventArgs
    {
        public CollectionEdit Action { get; private set; }
        public string OrginalName { get; private set; }
        public string NewName { get; set; }
        public Collections Collections { get; private set; }
        public Beatmaps Beatmaps { get; private set; }
        public IList<string> CollectionNames { get; private set; }
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
        public static CollectionEditArgs RenameCollection(Collection collection, string newName)
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
        #region Clear Collections
        public static CollectionEditArgs ClearCollections()
        {
            return new CollectionEditArgs(CollectionEdit.Clear);
        }
        #endregion
        #region Add Beatmaps to collection
        public static CollectionEditArgs AddBeatmaps(string collectionName,Beatmaps beatmaps)
        {
            return new CollectionEditArgs(CollectionEdit.AddBeatmaps)
            {
                Beatmaps = beatmaps,
                OrginalName = collectionName
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

    }
}