namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.AddBeatmapsStrategy;

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

    [Fact]
    public void WhenAddingBeatmapToExistingCollectionThenBeatmapIsAdded()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "My");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", [BeatmapWith("h1", 1)]));

        _ = collection.NumberOfBeatmaps.Should().Be(1);
        _ = collection.BeatmapHashes.Should().ContainSingle().Which.Should().Be("h1");
    }

    [Fact]
    public void WhenAddingMultipleBeatmapsThenAllAreAdded()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "My");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", [BeatmapWith("h1", 1), BeatmapWith("h2", 2)]));

        _ = collection.NumberOfBeatmaps.Should().Be(2);
        _ = collection.BeatmapHashes.Should().BeEquivalentTo(["h1", "h2"]);
    }

    [Fact]
    public void WhenAppendingToCollectionWithExistingBeatmapsThenAppendsPreservingExisting()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "My");
        collection.AddBeatmapByHash("existing");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", [BeatmapWith("h1", 1)]));

        _ = collection.NumberOfBeatmaps.Should().Be(2);
        _ = collection.BeatmapHashes.Should().BeEquivalentTo(["existing", "h1"]);
    }

    [Fact]
    public void WhenAddingBeatmapAlreadyPresentByHashThenStaysUnique()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "My");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", [BeatmapWith("h1", 1)]));
        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", [BeatmapWith("h1", 1)]));

        _ = collection.NumberOfBeatmaps.Should().Be(1);
        _ = collection.BeatmapHashes.Should().ContainSingle().Which.Should().Be("h1");
    }

    [Fact]
    public void WhenAddingToMissingCollectionThenIsNoOp()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection keep = Register(manager, "Keep");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("Missing", [BeatmapWith("h1", 1)]));

        _ = manager.LoadedCollections.Should().ContainSingle();
        _ = manager.GetCollectionByName("Missing").Should().BeNull();
        _ = keep.NumberOfBeatmaps.Should().Be(0);
    }

    [Fact]
    public void WhenAddingEmptyBeatmapListThenIsNoOp()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "My");
        collection.AddBeatmapByHash("existing");
        AddBeatmapsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("My", []));

        _ = collection.NumberOfBeatmaps.Should().Be(1);
        _ = collection.BeatmapHashes.Should().ContainSingle().Which.Should().Be("existing");
    }

    [Fact]
    public void WhenAddingMd5VersionForExistingMapIdOnlyBeatmapThenKeepsBothEntries()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "Favorites");
        collection.AddBeatmapByMapId(15);

        AddBeatmapsStrategy strategy = new();
        strategy.Execute(manager, CollectionEditArgs.AddBeatmaps("Favorites", [new BeatmapExtension { Md5 = "realhash", MapId = 15 }]));

        _ = collection.AllBeatmaps().Should().HaveCount(2);
        _ = collection.AllBeatmaps().Select(beatmap => beatmap.MapId).Should().OnlyContain(mapId => mapId == 15);
        _ = collection.AllBeatmaps().Select(beatmap => beatmap.Md5)
            .Should().BeEquivalentTo(["manually-added|15|0", "realhash"]);
    }
}
