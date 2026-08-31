namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using Xunit;

public sealed class GetCollectionsByIdTests
{
    private readonly MapCacher _maps = new();

    private CollectionsManagerWithCounts ManagerWith(params (string Name, int Id)[] entries)
    {
        CollectionsManagerWithCounts manager = new(_maps);

        foreach ((string name, int collectionId) in entries)
        {
            manager.LoadedCollections.Add(new OsuCollection(_maps) { Name = name, Id = collectionId });
        }

        return manager;
    }

    [Fact]
    public void WhenAllIdsMatchThenReturnsCollectionsInRequestedOrder()
    {
        CollectionsManagerWithCounts manager = ManagerWith(("A", 0), ("B", 1), ("C", 2));

        List<IOsuCollection> result = manager.GetCollectionsById([2, 0, 1]);

        _ = result.Should().HaveCount(3);
        _ = result[0].Name.Should().Be("C");
        _ = result[1].Name.Should().Be("A");
        _ = result[2].Name.Should().Be("B");
    }

    [Fact]
    public void WhenSomeIdsAreMissingThenNullEntriesPreservePosition()
    {
        CollectionsManagerWithCounts manager = ManagerWith(("A", 0));

        List<IOsuCollection> result = manager.GetCollectionsById([0, 99]);

        _ = result.Should().HaveCount(2);
        _ = result[0].Name.Should().Be("A");
        _ = result[1].Should().BeNull();
    }

    [Fact]
    public void WhenIdListIsEmptyThenReturnsEmptyList()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetCollectionsById([]).Should().BeEmpty();
    }

    [Fact]
    public void WhenNoCollectionsLoadedThenEveryEntryIsNull()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        List<IOsuCollection> result = manager.GetCollectionsById([1, 2]);

        _ = result.Should().HaveCount(2);
        _ = result.Should().OnlyContain(collection => collection == null);
    }
}
