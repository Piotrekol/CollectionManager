namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class GetCollectionByIdTests
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
    public void WhenIdMatchesThenReturnsThatCollection()
    {
        CollectionsManagerWithCounts manager = ManagerWith(("Favorites", 5));

        IOsuCollection result = manager.GetCollectionById(5);

        _ = result.Should().NotBeNull();
        _ = result.Name.Should().Be("Favorites");
        _ = result.Id.Should().Be(5);
    }

    [Fact]
    public void WhenIdDoesNotMatchThenReturnsNull()
    {
        CollectionsManagerWithCounts manager = ManagerWith(("Favorites", 5));

        _ = manager.GetCollectionById(99).Should().BeNull();
    }

    [Fact]
    public void WhenNoCollectionsLoadedThenReturnsNull()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetCollectionById(0).Should().BeNull();
    }

    [Fact]
    public void WhenMultipleCollectionsShareIdThenReturnsFirstRegistered()
    {
        CollectionsManagerWithCounts manager = ManagerWith(("First", 7), ("Second", 7));

        _ = manager.GetCollectionById(7).Should().BeSameAs(manager.LoadedCollections[0]);
    }
}
