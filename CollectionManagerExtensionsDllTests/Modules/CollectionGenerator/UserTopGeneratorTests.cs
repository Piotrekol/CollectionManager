using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollectionManagerExtensionsDll.Modules.CollectionGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollectionManager.Modules.FileIO;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionGenerator.Tests
{
    [TestClass()]
    public class UserTopGeneratorTests
    {
        public string apiKey = "S N I P";
        public string osuDbPath = @"D:\Gry\osu!\osu!.db";
        private OsuFileIo fileIO;
        public void Prepare()
        {
            fileIO = new OsuFileIo();
            fileIO.OsuDatabase.Load(osuDbPath);
        }
        [TestMethod()]
        public void GetPlayerCollectionsTest1()
        {
            Prepare();
            var obj = new UserTopGenerator(apiKey,fileIO.LoadedMaps);
            var cfg = new ScoreSaveConditions();
            /*var c = obj.GetPlayersCollections("Piotrekol","tops.{0}", cfg);
            Assert.IsTrue(c.Count==1);
            Assert.IsTrue(c[0].AllBeatmaps().Count()==100);
            Assert.IsTrue(c[0].Name == "tops.Piotrekol");*/
        }
        [TestMethod()]
        public void GetPlayerCollectionsTest2()
        {
            var fileIO = new OsuFileIo();
            fileIO.OsuDatabase.Load(osuDbPath);
            var obj = new UserTopGenerator(apiKey, fileIO.LoadedMaps);
            var cfg = new ScoreSaveConditions();
            /*var c = obj.GetPlayerCollections("Piotrekol", "tops.{0}{1}", cfg);
            Assert.IsTrue(c.Count >3);
            Assert.IsTrue(c[0].Name.Contains("tops.Piotrekol"));
            var mapCount = c.AllBeatmaps().Count();
            Assert.IsTrue(mapCount == 100);
            Assert.IsTrue(c[0].Name != "tops.Piotrekol");*/
        }
    }
}