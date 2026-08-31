namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.RemoveBeatmapsStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    private static BeatmapExtension BeatmapWith(string md5) => new() { Md5 = md5 };

    private OsuCollection Register(CollectionsManagerWithCounts manager, string name)
    {
        OsuCollection collection = new(_maps) { Name = name };
        manager.LoadedCollections.Add(collection);
        return collection;
    }

    [Fact]
    public void WhenRemovingAnExistingBeatmapThenItIsGoneButCollectionRemains()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "C");
        collection.AddBeatmapByHash("aaa");
        collection.AddBeatmapByHash("bbb");
        RemoveBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("C", [BeatmapWith("aaa")]));

        _ = collection.BeatmapHashes.Should().ContainSingle()
            .Which.Should().Be("bbb");
        _ = manager.LoadedCollections.Should().ContainSingle();
    }

    [Fact]
    public void WhenRemovingEveryBeatmapThenCollectionIsEmptiedButStillLoaded()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "C");
        collection.AddBeatmapByHash("aaa");
        collection.AddBeatmapByHash("bbb");
        RemoveBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("C", [BeatmapWith("aaa"), BeatmapWith("bbb")]));

        _ = collection.BeatmapHashes.Should().BeEmpty();
        _ = collection.AllBeatmaps().Should().BeEmpty();
        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("C");
    }

    [Fact]
    public void WhenRemovingMultipleBeatmapsInOneCallThenOnlyThoseAreRemoved()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "C");
        collection.AddBeatmapByHash("aaa");
        collection.AddBeatmapByHash("bbb");
        collection.AddBeatmapByHash("ccc");
        RemoveBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("C", [BeatmapWith("aaa"), BeatmapWith("ccc")]));

        _ = collection.BeatmapHashes.Should().ContainSingle()
            .Which.Should().Be("bbb");
    }

    [Fact]
    public void WhenRemovingABeatmapThatIsNotPresentThenCollectionIsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "C");
        collection.AddBeatmapByHash("aaa");
        RemoveBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("C", [BeatmapWith("zzz")]));

        _ = collection.BeatmapHashes.Should().ContainSingle()
            .Which.Should().Be("aaa");
    }

    [Fact]
    public void WhenCollectionIsMissingThenOperationIsNoOp()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection keep = Register(manager, "Keep");
        keep.AddBeatmapByHash("aaa");
        RemoveBeatmapsStrategy strategy = new();

        Action act = () => strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("Ghost", [BeatmapWith("aaa")]));

        _ = act.Should().NotThrow();
        _ = keep.BeatmapHashes.Should().ContainSingle()
            .Which.Should().Be("aaa");
        _ = manager.LoadedCollections.Should().ContainSingle();
    }

    [Fact]
    public void WhenBeatmapListIsEmptyThenCollectionIsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "C");
        collection.AddBeatmapByHash("aaa");
        RemoveBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveBeatmaps("C", []));

        _ = collection.BeatmapHashes.Should().ContainSingle()
            .Which.Should().Be("aaa");
    }
}
