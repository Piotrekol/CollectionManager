namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.ClearStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    private CollectionsManagerWithCounts ManagerWith(params string[] names)
    {
        CollectionsManagerWithCounts manager = new(_maps);

        foreach (string name in names)
        {
            manager.LoadedCollections.Add(new OsuCollection(_maps) { Name = name });
        }

        return manager;
    }

    [Fact]
    public void WhenClearingThenRemovesAllLoadedCollections()
    {
        CollectionsManagerWithCounts manager = ManagerWith("First", "Second", "Third");
        ClearStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = manager.LoadedCollections.Should().BeEmpty();
    }

    [Fact]
    public void WhenClearingThenAllPreviouslyReachableCollectionsBecomeUnreachable()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keep", "Drop");
        ClearStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = manager.LoadedCollections.Should().BeEmpty();
        _ = manager.GetCollectionByName("Keep").Should().BeNull();
        _ = manager.GetCollectionByName("Drop").Should().BeNull();
    }

    [Fact]
    public void WhenClearingCollectionsHoldingBeatmapsThenAllCollectionsAreRemoved()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection collection = new(_maps) { Name = "Filled" };
        collection.AddBeatmap(new BeatmapExtension { Md5 = "hash1", MapId = 1 });
        collection.AddBeatmap(new BeatmapExtension { Md5 = "hash2", MapId = 2 });
        manager.LoadedCollections.Add(collection);
        ClearStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = manager.LoadedCollections.Should().BeEmpty();
    }

    [Fact]
    public void WhenNoCollectionsAreLoadedThenClearingDoesNotThrow()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        ClearStrategy strategy = new();

        Action act = () => strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = act.Should().NotThrow();
        _ = manager.LoadedCollections.Should().BeEmpty();
    }

    [Fact]
    public void WhenClearingTwiceThenSecondClearDoesNotThrow()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Only");
        ClearStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ClearCollections());
        Action act = () => strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = act.Should().NotThrow();
        _ = manager.LoadedCollections.Should().BeEmpty();
    }
}
