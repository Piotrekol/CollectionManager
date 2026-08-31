namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.AddStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    [Fact]
    public void WhenAddingCollectionsThenAssignsSequentialIdsAndPreservesUniqueNames()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddCollections([
            new OsuCollection(_maps) { Name = "First" },
            new OsuCollection(_maps) { Name = "Second" }
        ]));

        _ = manager.LoadedCollections.Should().HaveCount(2);
        _ = manager.LoadedCollections[0].Name.Should().Be("First");
        _ = manager.LoadedCollections[0].Id.Should().Be(0);
        _ = manager.LoadedCollections[1].Name.Should().Be("Second");
        _ = manager.LoadedCollections[1].Id.Should().Be(1);
    }

    [Fact]
    public void WhenAddingDuplicateNameAcrossBatchesThenSecondGetsDisambiguated()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddCollections([new OsuCollection(_maps) { Name = "Dup" }]));
        strategy.Execute(manager, CollectionEditArgs.AddCollections([new OsuCollection(_maps) { Name = "Dup" }]));

        _ = manager.GetCollectionByName("Dup").Should().NotBeNull();
        _ = manager.GetCollectionByName("Dup_0").Should().NotBeNull();
        _ = manager.LoadedCollections.Should().HaveCount(2);
    }

    [Fact]
    public void WhenAddingDuplicateNamesWithinSingleBatchThenEachGetsIncrementingSuffix()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddCollections([
            new OsuCollection(_maps) { Name = "Dup" },
            new OsuCollection(_maps) { Name = "Dup" },
            new OsuCollection(_maps) { Name = "Dup" }
        ]));

        _ = manager.GetCollectionByName("Dup").Should().NotBeNull();
        _ = manager.GetCollectionByName("Dup_0").Should().NotBeNull();
        _ = manager.GetCollectionByName("Dup_1").Should().NotBeNull();
        _ = manager.LoadedCollections.Should().HaveCount(3);
    }

    [Fact]
    public void WhenInvokedMultipleTimesThenIdsContinueIncrementingAcrossExecutes()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddCollections([new OsuCollection(_maps) { Name = "First" }]));
        strategy.Execute(manager, CollectionEditArgs.AddCollections([new OsuCollection(_maps) { Name = "Second" }]));

        _ = manager.LoadedCollections[0].Id.Should().Be(0);
        _ = manager.LoadedCollections[1].Id.Should().Be(1);
    }

    [Fact]
    public void WhenAddingNoCollectionsThenLeavesLoadedCollectionsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddCollections([]));

        _ = manager.LoadedCollections.Should().BeEmpty();
    }
}
