namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.RenameStrategy;

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

    private static void RunRename(CollectionsManagerWithCounts manager, string oldName, string newName)
        => new RenameStrategy().Execute(manager, CollectionEditArgs.RenameCollection(oldName, newName));

    [Fact]
    public void WhenRenamingCollectionThenUpdatesNameAndPreservesBeatmapsIdAndPosition()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "Favorites");
        collection.Id = 7;
        collection.AddBeatmap(BeatmapWith("hash1", 100));
        collection.AddBeatmap(BeatmapWith("hash2", 101));

        RunRename(manager, "Favorites", "Archive");

        _ = collection.Name.Should().Be("Archive");
        _ = collection.Id.Should().Be(7);
        _ = collection.AllBeatmaps().Should().HaveCount(2)
            .And.Contain(beatmap => beatmap.Md5 == "hash1")
            .And.Contain(beatmap => beatmap.Md5 == "hash2");
        _ = manager.LoadedCollections.Should().HaveCount(1);
        _ = manager.LoadedCollections[0].Should().BeSameAs(collection);
        _ = manager.GetCollectionByName("Favorites").Should().BeNull();
        _ = manager.GetCollectionByName("Archive").Should().BeSameAs(collection);
    }

    [Fact]
    public void WhenRenamingToExistingDifferentCollectionNameThenDisambiguatesWithSuffix()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "Favorites");
        _ = Register(manager, "Archive");

        RunRename(manager, "Favorites", "Archive");

        _ = collection.Name.Should().Be("Archive_0");
        _ = manager.GetCollectionByName("Archive").Should().NotBeNull();
        _ = manager.GetCollectionByName("Favorites").Should().BeNull();
        _ = manager.LoadedCollections.Should().HaveCount(2);
    }

    [Fact]
    public void WhenBothDesiredAndSuffixedNamesTakenThenIncrementsSuffix()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "Favorites");
        _ = Register(manager, "Archive");
        _ = Register(manager, "Archive_0");

        RunRename(manager, "Favorites", "Archive");

        _ = collection.Name.Should().Be("Archive_1");
        _ = manager.LoadedCollections.Should().HaveCount(3);
    }

    [Fact]
    public void WhenRenamingCollectionToItsOwnCurrentNameThenIsNoOp()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = Register(manager, "Favorites");
        collection.Id = 5;

        RunRename(manager, "Favorites", "Favorites");

        _ = collection.Name.Should().Be("Favorites");
        _ = collection.Id.Should().Be(5);
        _ = manager.LoadedCollections.Should().HaveCount(1);
        _ = manager.GetCollectionByName("Favorites").Should().BeSameAs(collection);
        _ = manager.GetCollectionByName("Favorites_0").Should().BeNull();
    }

    [Fact]
    public void WhenRenamingMissingCollectionThenIsNoOpAndDoesNotThrow()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        Action renameAction = () => RunRename(manager, "Ghost", "Anything");

        _ = renameAction.Should().NotThrow();
        _ = manager.LoadedCollections.Should().BeEmpty();
        _ = manager.GetCollectionByName("Anything").Should().BeNull();
    }
}
