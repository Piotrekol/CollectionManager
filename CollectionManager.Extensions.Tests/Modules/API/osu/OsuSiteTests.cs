namespace CollectionManager.Extensions.Tests.Modules.API.osu;

using CollectionManager.Extensions.Modules.API.osu;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

[TestClass()]
public class OsuSiteTests
{
    [TestMethod()]
    public void GetUsernamesFirstPage()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(1, 50, null));
        Assert.IsTrue(output.Count == 50);
    }
    [TestMethod()]
    public void GetUsernames1()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(300, 800, null));
        Assert.IsTrue(output.Count == 500);
    }

    [TestMethod()]
    public void GetUsernames2Pages()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(1, 100, null));
        Assert.IsTrue(output.Count == 100);
    }
    [TestMethod()]
    public void GetUsernamesSingleUser1()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(51, 51, null));
        Assert.IsTrue(output.Count == 1);
    }
    [TestMethod()]
    public void GetUsernamesSingleUser2()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(1, 1, null));
        Assert.IsTrue(output.Count == 1);
    }
    [TestMethod()]
    public void GetUsernamesFullFirstPartialSecondPage()
    {
        OsuSite obj = new();
        List<string> output = Run(() => obj.GetUsernames(1, 67, null));
        Assert.IsTrue(output.Count == 67);
    }
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void GetUsernamesRankBoundary1()
    {
        OsuSite obj = new();

        _ = obj.GetUsernames(-1, 50, null);
    }
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void GetUsernamesRankBoundary2()
    {
        OsuSite obj = new();

        _ = obj.GetUsernames(9951, 10001, null);
    }

    private static T Run<T>(Func<T> func)
        => RunFlaky<WebException, T>(func);

    private static T RunFlaky<TEx, T>(Func<T> func)
        where TEx : Exception
    {
        try
        {
            return func();
        }
        catch (TEx ex)
        {
            Assert.Inconclusive(ex.ToString());
        }

        return default;
    }
}