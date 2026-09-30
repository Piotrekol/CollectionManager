namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.InverseStrategy;

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

    private IOsuCollection RunInverse(CollectionsManagerWithCounts manager, IReadOnlyList<string> names)
    {
        new InverseStrategy(_maps).Execute(manager, CollectionEditArgs.InverseCollections(names, "Inverse"));
        return manager.GetCollectionByName("Inverse");
    }

    [Fact]
    public void WhenInvertingSingleCollectionThenKeepsLoadedMapsNotInIt()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _maps.StoreBeatmap(BeatmapWith("a", 11));
        _maps.StoreBeatmap(BeatmapWith("b", 12));
        _maps.StoreBeatmap(BeatmapWith("c", 13));

        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 11));

        IOsuCollection result = RunInverse(manager, ["Source"]);

        _ = result.AllBeatmaps().Should().HaveCount(2)
            .And.Contain(beatmap => beatmap.Md5 == "b")
            .And.Contain(beatmap => beatmap.Md5 == "c")
            .And.NotContain(beatmap => beatmap.Md5 == "a");
        _ = manager.LoadedCollections.Should().HaveCount(2);
    }

    [Fact]
    public void WhenInvertingMultipleCollectionsThenExcludesUnionOfTheirBeatmaps()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _maps.StoreBeatmap(BeatmapWith("a", 11));
        _maps.StoreBeatmap(BeatmapWith("b", 12));
        _maps.StoreBeatmap(BeatmapWith("c", 13));
        _maps.StoreBeatmap(BeatmapWith("d", 14));

        OsuCollection first = Register(manager, "First");
        first.AddBeatmap(BeatmapWith("a", 11));
        OsuCollection second = Register(manager, "Second");
        second.AddBeatmap(BeatmapWith("c", 13));

        IOsuCollection result = RunInverse(manager, ["First", "Second"]);

        _ = result.AllBeatmaps().Should().HaveCount(2)
            .And.Contain(beatmap => beatmap.Md5 == "b")
            .And.Contain(beatmap => beatmap.Md5 == "d");
    }

    [Fact]
    public void WhenNoLoadedMapsThenProducesEmptyResultCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 11));

        IOsuCollection result = RunInverse(manager, ["Source"]);

        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenAllLoadedMapsAreInTheCollectionThenResultIsEmpty()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _maps.StoreBeatmap(BeatmapWith("a", 11));
        _maps.StoreBeatmap(BeatmapWith("b", 12));

        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 11));
        source.AddBeatmap(BeatmapWith("b", 12));

        IOsuCollection result = RunInverse(manager, ["Source"]);

        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenCollectionHoldsMapByValidMapIdOnlyThenLoadedMapIsExcluded()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _maps.StoreBeatmap(BeatmapWith("loaded", 15));
        _maps.StoreBeatmap(BeatmapWith("other", 16));

        OsuCollection source = Register(manager, "Source");
        source.AddBeatmapByMapId(15);

        IOsuCollection result = RunInverse(manager, ["Source"]);

        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("other");
    }

    [Fact]
    public void WhenCollectionHoldsMapByHashWithPlaceholderMapIdThenLoadedMapIsExcluded()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _maps.StoreBeatmap(BeatmapWith("same", 5));
        _maps.StoreBeatmap(BeatmapWith("other", 16));

        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("same", 5));

        IOsuCollection result = RunInverse(manager, ["Source"]);

        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.Md5.Should().Be("other");
    }
}
