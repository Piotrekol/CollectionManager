namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.DifferenceStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    private static BeatmapExtension BeatmapWith(string md5, int mapId) => new() { Md5 = md5, MapId = mapId };

    private OsuCollection Register(CollectionsManagerWithCounts manager, string name)
    {
        OsuCollection collection = new(_maps) { Name = name };
        manager.LoadedCollections.Add(collection);
        return collection;
    }

    private IOsuCollection RunDifference(CollectionsManagerWithCounts manager, IReadOnlyList<string> names)
    {
        new DifferenceStrategy(_maps).Execute(manager, CollectionEditArgs.DifferenceCollections(names, "Diff"));
        return manager.GetCollectionByName("Diff");
    }

    [Fact]
    public void WhenDifferencingTwoOverlappingCollectionsThenExcludesBeatmapsPresentInBoth()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("shared", 1));
        firstCollection.AddBeatmap(BeatmapWith("onlyA", 2));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("shared", 1));
        secondCollection.AddBeatmap(BeatmapWith("onlyB", 3));

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["onlyA", "onlyB"]);
    }

    [Fact]
    public void WhenDifferencingThreeCollectionsThenKeepsBeatmapsInExactlyOneCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("shared", 1));
        firstCollection.AddBeatmap(BeatmapWith("onlyA", 2));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("shared", 1));
        secondCollection.AddBeatmap(BeatmapWith("onlyB", 3));
        OsuCollection thirdCollection = Register(manager, "C");
        thirdCollection.AddBeatmap(BeatmapWith("shared", 1));
        thirdCollection.AddBeatmap(BeatmapWith("onlyC", 4));

        IOsuCollection result = RunDifference(manager, ["A", "B", "C"]);

        _ = result.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["onlyA", "onlyB", "onlyC"]);
    }

    [Fact]
    public void WhenCollectionsShareNoBeatmapsThenResultKeepsAllBeatmaps()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("onlyA", 1));
        firstCollection.AddBeatmap(BeatmapWith("alsoA", 2));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("onlyB", 3));
        secondCollection.AddBeatmap(BeatmapWith("alsoB", 4));

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["onlyA", "alsoA", "onlyB", "alsoB"]);
    }

    [Fact]
    public void WhenCollectionsFullyOverlapThenResultIsEmpty()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("shared", 1));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("shared", 1));

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenDifferencingSingleCollectionThenKeepsAllOfItsBeatmaps()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("first", 1));
        firstCollection.AddBeatmap(BeatmapWith("second", 2));

        IOsuCollection result = RunDifference(manager, ["A"]);

        _ = result.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["first", "second"]);
    }

    [Fact]
    public void WhenAllCollectionsAreEmptyThenResultIsEmpty()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "A");
        _ = Register(manager, "B");

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenSameMapAppearsViaDifferentRepresentationsThenItIsTreatedAsShared()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmapByMapId(15);
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(new BeatmapExtension { Md5 = "realhash", MapId = 15 });

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenDifferencingThenPreservesSourceCollectionsAndAddsResult()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("onlyA", 1));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("onlyB", 2));

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.Should().NotBeNull();
        _ = manager.LoadedCollections.Should().HaveCount(3);
        _ = manager.GetCollectionByName("A").Should().NotBeNull();
        _ = manager.GetCollectionByName("B").Should().NotBeNull();
        _ = manager.GetCollectionByName("Diff").Should().NotBeNull();
        _ = manager.GetCollectionByName("A").AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("onlyA");
        _ = manager.GetCollectionByName("B").AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("onlyB");
    }

    [Fact]
    public void WhenBeatmapIsDuplicatedWithinASingleCollectionThenItIsKeptOnce()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("dup", 1));
        firstCollection.AddBeatmap(BeatmapWith("dup", 1));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("onlyB", 2));

        IOsuCollection result = RunDifference(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["dup", "onlyB"]);
    }
}
