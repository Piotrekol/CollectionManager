namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.RemoveStrategy;

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
    public void WhenRemovingNamedCollectionThenLeavesOthersIntact()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keep", "Drop");
        RemoveStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveCollections(["Drop"]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Keep");
        _ = manager.GetCollectionByName("Drop").Should().BeNull();
    }

    [Fact]
    public void WhenRemovingUnknownNameThenDoesNotThrow()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keep");
        RemoveStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveCollections(["DoesNotExist"]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Keep");
    }

    [Fact]
    public void WhenRemovingMultipleNamesThenRemovesAll()
    {
        CollectionsManagerWithCounts manager = ManagerWith("A", "B", "C");
        RemoveStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveCollections(["A", "C"]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("B");
    }

    [Fact]
    public void WhenNameIsSubstringOfAnotherThenOnlyExactMatchIsRemoved()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keep", "Keeper");
        RemoveStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveCollections(["Keep"]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Keeper");
    }

    [Fact]
    public void WhenNoCollectionsAreLoadedThenRemovingDoesNotThrow()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        RemoveStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.RemoveCollections(["Anything"]));

        _ = manager.LoadedCollections.Should().BeEmpty();
    }
}
