namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.DuplicateStrategy;

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

    private IOsuCollection Duplicate(CollectionsManagerWithCounts manager, string source, string newName)
    {
        new DuplicateStrategy(_maps).Execute(manager, CollectionEditArgs.DuplicateCollection(source, newName));
        return manager.LoadedCollections[^1];
    }

    [Fact]
    public void WhenDuplicatingCollectionThenCopiesAllBeatmapsToNewNamedCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 1));
        source.AddBeatmap(BeatmapWith("b", 2));

        IOsuCollection copy = Duplicate(manager, "Source", "Copy");

        _ = copy.Name.Should().Be("Copy");
        _ = copy.AllBeatmaps().Should().HaveCount(2)
            .And.Contain(beatmap => beatmap.Md5 == "a")
            .And.Contain(beatmap => beatmap.Md5 == "b");
    }

    [Fact]
    public void WhenDuplicatingCollectionThenSourceCollectionRemainsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 1));
        source.AddBeatmap(BeatmapWith("b", 2));

        _ = Duplicate(manager, "Source", "Copy");

        _ = source.AllBeatmaps().Should().HaveCount(2);
        _ = manager.GetCollectionByName("Source").Should().BeSameAs(source);
    }

    [Fact]
    public void WhenNewNameIsUniqueThenKeepsRequestedName()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 1));

        IOsuCollection copy = Duplicate(manager, "Source", "Fresh");

        _ = copy.Name.Should().Be("Fresh");
        _ = manager.LoadedCollections.Should().HaveCount(2);
    }

    [Fact]
    public void WhenNewNameCollidesWithExistingCollectionThenNameIsDisambiguated()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "Copy");
        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 1));

        IOsuCollection copy = Duplicate(manager, "Source", "Copy");

        _ = copy.Name.Should().Be("Copy_0");
        _ = manager.GetCollectionByName("Copy").Should().NotBeNull();
        _ = manager.GetCollectionByName("Copy_0").Should().NotBeNull();
    }

    [Fact]
    public void WhenNewNameEqualsSourceNameThenNameIsDisambiguated()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection source = Register(manager, "Orig");
        source.AddBeatmap(BeatmapWith("a", 1));

        IOsuCollection copy = Duplicate(manager, "Orig", "Orig");

        _ = copy.Name.Should().Be("Orig_0");
        _ = manager.GetCollectionByName("Orig").Should().NotBeNull();
        _ = manager.GetCollectionByName("Orig_0").Should().NotBeNull();
    }

    [Fact]
    public void WhenDuplicatingEmptyCollectionThenYieldsEmptyNamedCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "Empty");

        IOsuCollection copy = Duplicate(manager, "Empty", "Copy");

        _ = copy.Name.Should().Be("Copy");
        _ = copy.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenDuplicatingTwiceThenYieldsIncrementingDisambiguatedNames()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection source = Register(manager, "Source");
        source.AddBeatmap(BeatmapWith("a", 1));

        IOsuCollection first = Duplicate(manager, "Source", "Source");
        IOsuCollection second = Duplicate(manager, "Source", "Source");

        _ = first.Name.Should().Be("Source_0");
        _ = second.Name.Should().Be("Source_1");
        _ = manager.LoadedCollections.Should().HaveCount(3);
    }
}
