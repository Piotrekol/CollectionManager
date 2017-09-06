using System;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;

namespace CollectionManager
{
    internal class Class1
    {
        internal void Run()
        {
            var osuFileIo = new OsuFileIo(new BeatmapExtension());

            //Automatic Detection of osu! directory location
            string dir = osuFileIo.OsuPathResolver.GetOsuDir(ThisPathIsCorrect, SelectDirectoryDialog);



            string osuPath = @"E:\osu!\";
            string osuDbFileName = "osu!.db";
            string ExampleCollectionFileLocation = @"E:\osuCollections\SomeCollectionThatExists.db";




            osuFileIo.OsuDatabase.Load(osuPath+osuDbFileName);
            
            //osu! configuration file is currently only used for getting a songs folder location
            osuFileIo.OsuSettings.Load(osuPath);

            //Data loaded using next 2 functions are going to be automatically correlated
            //with currently avalable beatmap data.
            osuFileIo.CollectionLoader.LoadOsuCollection(ExampleCollectionFileLocation);
            //osuFileIo.CollectionLoader.LoadOsdbCollection("");


            string osuSongsFolderLocation = osuFileIo.OsuSettings.CustomBeatmapDirectoryLocation;
            
            //Create Collection manager instance
            var collectionManager = new CollectionsManagerWithCounts(osuFileIo.OsuDatabase.LoadedMaps.Beatmaps);
            //or just this:
            //var collectionManager = new CollectionsManager(osuFileIo.OsuDatabase.LoadedMaps.Beatmaps);

            collectionManager.LoadedCollections.CollectionChanged += LoadedCollections_CollectionChanged;

            //Create some dummy collections
            Collection ourCollection = new Collection(osuFileIo.LoadedMaps) { Name = "Example collection1" };
            Collection ourSecondCollection = new Collection(osuFileIo.LoadedMaps) { Name = "Example collection2" };

            //Add these to our manager
            collectionManager.EditCollection(
                CollectionEditArgs.AddCollections(new Collections() { ourCollection, ourSecondCollection })
                );

            //Add some beatmaps to ourSecondCollection
            collectionManager.EditCollection(
                CollectionEditArgs.AddBeatmaps("Example collection2",new Beatmaps()
                {
                    new BeatmapExtension() {Md5 = "'known' md5"},
                    new BeatmapExtension() {Md5 = "another 'known' md5"},
                    new BeatmapExtension() {Md5 = "md5 that we have no idea about"}
                }));

            //Trying to issue any action on collection with unknown name will be just ignored
            collectionManager.EditCollection(
                CollectionEditArgs.AddBeatmaps("Collection that doesn't exist", new Beatmaps()
                {
                    new BeatmapExtension() {Md5 = "1234567890,yes I know these aren't valid md5s"}
                }));

            //Merge our collections into one.
            //Note that while I do not impose a limit on collection name length, osu! does and it will be truncated upon opening collection in osu! client.
            collectionManager.EditCollection(
                CollectionEditArgs.MergeCollections(new Collections(){ourCollection,ourSecondCollection}
                , "Collection created from 2 Example collections")
                );

            //true
            bool isNameTaken = collectionManager.CollectionNameExists("Collection created from 2 Example collections");

            Collection ourMergedCollection = collectionManager.GetCollectionByName("Collection created from 2 Example collections");
            
            //These are avaliable only when using CollectionsManagerWithCounts
            var TotalBeatmapCount = collectionManager.BeatmapsInCollectionsCount;
            var MissingBeatmapsCount = collectionManager.MissingBeatmapCount;

            //Lets save our collections after edits
            //as .osdb
            osuFileIo.CollectionLoader.SaveOsdbCollection(collectionManager.LoadedCollections, ExampleCollectionFileLocation);
            //or .db
            osuFileIo.CollectionLoader.SaveOsuCollection(collectionManager.LoadedCollections, ExampleCollectionFileLocation);
        }

        private string SelectDirectoryDialog(string s)
        {
           //If result of ThisPathIsCorrect was false this would be executed
           //ask user to provide path themself?
            return string.Empty;
        }

        private bool ThisPathIsCorrect(string s)
        {
            //check with user that path provided in s is correct?
            return true;
        }

        private void LoadedCollections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //This gets called once per every collectionManager.EditCollection() call

            //Do some things
        }
    }
}
