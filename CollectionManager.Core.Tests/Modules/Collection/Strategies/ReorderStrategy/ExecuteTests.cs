namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.ReorderStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    private OsuCollection Register(CollectionsManagerWithCounts manager, string name)
    {
        OsuCollection collection = new(_maps) { Name = name };
        manager.LoadedCollections.Add(collection);
        return collection;
    }

    [Fact]
    public void WhenReorderingAscendingWhileMovingAfterAnchorThenAssignsSequentialRankPrefixesAndKeepsListOrder()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection banana = Register(manager, "Banana");
        OsuCollection apple = Register(manager, "Apple");
        OsuCollection cherry = Register(manager, "Cherry");
        OsuCollection date = Register(manager, "Date");
        ReorderStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ReorderCollections(["Cherry"], "Apple", placeBefore: false, "Name", SortOrder.Ascending));
        _ = manager.LoadedCollections.Should().HaveCount(4);
        _ = manager.LoadedCollections[0].Should().BeSameAs(banana);
        _ = manager.LoadedCollections[1].Should().BeSameAs(apple);
        _ = manager.LoadedCollections[2].Should().BeSameAs(cherry);
        _ = manager.LoadedCollections[3].Should().BeSameAs(date);
        _ = apple.Name.Should().Be("0|  Apple");
        _ = cherry.Name.Should().Be("1|  Cherry");
        _ = banana.Name.Should().Be("2|  Banana");
        _ = date.Name.Should().Be("3|  Date");
    }

    [Fact]
    public void WhenReorderingDescendingWhileMovingBeforeAnchorThenAssignsRankPrefixesInDescendingOrder()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection banana = Register(manager, "Banana");
        OsuCollection apple = Register(manager, "Apple");
        OsuCollection cherry = Register(manager, "Cherry");
        OsuCollection date = Register(manager, "Date");
        ReorderStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ReorderCollections(["Banana"], "Cherry", placeBefore: true, "Name", SortOrder.Descending));

        _ = manager.LoadedCollections.Should().HaveCount(4);
        _ = manager.LoadedCollections[0].Should().BeSameAs(banana);
        _ = manager.LoadedCollections[1].Should().BeSameAs(apple);
        _ = manager.LoadedCollections[2].Should().BeSameAs(cherry);
        _ = manager.LoadedCollections[3].Should().BeSameAs(date);
        _ = date.Name.Should().Be("0|  Date");
        _ = banana.Name.Should().Be("1|  Banana");
        _ = cherry.Name.Should().Be("2|  Cherry");
        _ = apple.Name.Should().Be("3|  Apple");
    }

    [Fact]
    public void WhenCollectionAlreadyHasRankPrefixThenStripsItBeforeApplyingNewPrefix()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection apple = Register(manager, "5|  Apple");
        OsuCollection banana = Register(manager, "5|  Banana");
        OsuCollection cherry = Register(manager, "5|  Cherry");
        ReorderStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ReorderCollections(["5|  Banana"], "5|  Cherry", placeBefore: false, "Name", SortOrder.Ascending));
        _ = apple.Name.Should().Be("0|  Apple");
        _ = cherry.Name.Should().Be("1|  Cherry");
        _ = banana.Name.Should().Be("2|  Banana");
    }

    [Fact]
    public void WhenMovingNoCollectionsWhileSortingThenAssignsRankPrefixesBySortOrder()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection banana = Register(manager, "Banana");
        OsuCollection apple = Register(manager, "Apple");
        OsuCollection cherry = Register(manager, "Cherry");
        ReorderStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.ReorderCollections([], "Apple", placeBefore: false, "Name", SortOrder.Ascending));

        _ = manager.LoadedCollections.Should().HaveCount(3);
        _ = manager.LoadedCollections[0].Should().BeSameAs(banana);
        _ = manager.LoadedCollections[1].Should().BeSameAs(apple);
        _ = manager.LoadedCollections[2].Should().BeSameAs(cherry);
        _ = apple.Name.Should().Be("0|  Apple");
        _ = banana.Name.Should().Be("1|  Banana");
        _ = cherry.Name.Should().Be("2|  Cherry");
    }

    [Fact]
    public void WhenGivenArgsAreNotReorderArgsThenThrowsInvalidOperationException()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "Apple");
        ReorderStrategy strategy = new();

        Action act = () => strategy.Execute(manager, CollectionEditArgs.ClearCollections());

        _ = act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenSortColumnIsUnrecognizedThenThrowsInvalidOperationException()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "Apple");
        ReorderStrategy strategy = new();

        Action act = () => strategy.Execute(manager, CollectionEditArgs.ReorderCollections(["Apple"], "Apple", placeBefore: true, "Nonexistent", SortOrder.Ascending));

        _ = act.Should().Throw<InvalidOperationException>();
    }
}
