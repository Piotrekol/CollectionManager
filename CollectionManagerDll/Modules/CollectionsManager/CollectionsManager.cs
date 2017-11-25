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
        public CollectionsManager(Beatmaps loadedBeatmaps)
        {
            LoadedBeatmaps = loadedBeatmaps;
        }
        /*
         add collection
         remove collection
         edit collection name
         merge x collections
         clear collections
         add beatmaps to collection
         remove beatmaps from collection
         */

        private void EditCollection(CollectionEditArgs args, bool suspendRefresh = false)
        {
            var action = args.Action;
            if (action == CollectionEdit.Add)
            {
                foreach (var collection in args.Collections)
                {
                    collection.Name = GetValidCollectionName(collection.Name);
                }
                LoadedCollections.AddRange(args.Collections);
            }
            else if (action == CollectionEdit.AddOrMergeIfExists)
            {
                foreach (var collection in args.Collections)
                {
                    if (CollectionNameExists(collection.Name))
                        EditCollection(CollectionEditArgs.MergeCollections(
                            new Collections() { GetCollectionByName(collection.Name),collection }, collection.Name), true);
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
            else if (action == CollectionEdit.Clear)
            {
                LoadedCollections.Clear();
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
            }
            if (!suspendRefresh)
                AfterCollectionsEdit();
        }
        public void EditCollection(CollectionEditArgs args)
        {
            EditCollection(args, false);
        }

        protected virtual void AfterCollectionsEdit()
        {
            LoadedCollections.CallReset();
        }
        public Collection GetCollectionByName(string collectionName)
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

        private string GetValidCollectionName(string desiredName)
        {
            var newName = desiredName;
            int c = 0;
            while (CollectionNameExists(newName))
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

        public virtual bool IsCollectionNameValid(string name)
        {
            return !CollectionNameExists(name);
        }
    }
}