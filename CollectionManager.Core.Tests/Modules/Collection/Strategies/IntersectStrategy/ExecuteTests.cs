namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.IntersectStrategy;

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

    private IOsuCollection RunIntersect(CollectionsManagerWithCounts manager, IReadOnlyList<string> names)
    {
        new IntersectStrategy(_maps).Execute(manager, CollectionEditArgs.IntersectCollections(names, "Intersect"));
        return manager.GetCollectionByName("Intersect");
    }

    [Fact]
    public void WhenIntersectingTwoOverlappingCollectionsThenKeepsOnlySharedBeatmaps()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("shared", 1));
        firstCollection.AddBeatmap(BeatmapWith("onlyInA", 2));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("shared", 1));
        secondCollection.AddBeatmap(BeatmapWith("onlyInB", 3));

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("shared");
    }

    [Fact]
    public void WhenThereAreNoMapsToIntersectThenProducesEmptyResultCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "A");
        _ = Register(manager, "B");

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenCollectionsShareNoBeatmapsThenResultIsEmpty()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("onlyInA", 1));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("onlyInB", 2));

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenBeatmapsShareValidMapIdButDifferInHashThenTheyAreIntersected()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("hashA", 15));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("hashB", 15));

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().ContainSingle();
    }

    [Fact]
    public void WhenBeatmapsShareHashWithPlaceholderMapIdsThenTheyAreIntersected()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("same", 0));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("same", 0));

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("same");
    }

    [Fact]
    public void WhenBeatmapsDifferInBothHashAndValidMapIdThenNotIntersected()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("hashA", 15));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("hashB", 16));

        IOsuCollection result = RunIntersect(manager, ["A", "B"]);

        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenIntersectingMoreThanTwoCollectionsThenKeepsBeatmapsPresentInAll()
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

        IOsuCollection result = RunIntersect(manager, ["A", "B", "C"]);

        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("shared");
    }
}
