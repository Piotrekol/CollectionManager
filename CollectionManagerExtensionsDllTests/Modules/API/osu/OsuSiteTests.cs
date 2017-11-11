using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollectionManagerExtensionsDll.Modules.API.osu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionManagerExtensionsDll.Modules.API.osu.Tests
{
    [TestClass()]
    public class OsuSiteTests
    {
        [TestMethod()]
        public void GetUsernamesFirstPage()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(1, 50, null);
            Assert.IsTrue(output.Count == 50);
        }
        [TestMethod()]
        public void GetUsernames1()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(300, 800, null);
            Assert.IsTrue(output.Count == 500);
        }

        [TestMethod()]
        public void GetUsernames2Pages()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(1, 100, null);
            Assert.IsTrue(output.Count == 100);
        }
        [TestMethod()]
        public void GetUsernamesSingleUser1()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(51, 51, null);
            Assert.IsTrue(output.Count == 1);
        }
        [TestMethod()]
        public void GetUsernamesSingleUser2()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(1, 1, null);
            Assert.IsTrue(output.Count == 1);
        }
        [TestMethod()]
        public void GetUsernamesFullFirstPartialSecondPage()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(1, 67, null);
            Assert.IsTrue(output.Count == 67);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUsernamesRankBoundary1()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(-1, 50, null);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUsernamesRankBoundary2()
        {
            var obj = new OsuSite();
            var output = obj.GetUsernames(9951, 10001, null);
        }
    }
}