namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class GetCollectionByNameTests
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
    public void WhenNameMatchesExactlyThenReturnsThatCollection()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        IOsuCollection result = manager.GetCollectionByName("Favorites");

        _ = result.Should().NotBeNull();
        _ = result.Name.Should().Be("Favorites");
    }

    [Fact]
    public void WhenNameDoesNotMatchThenReturnsNull()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.GetCollectionByName("Pending").Should().BeNull();
    }

    [Fact]
    public void WhenNoCollectionsLoadedThenReturnsNull()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetCollectionByName("Anything").Should().BeNull();
    }

    [Fact]
    public void WhenNameIsSubstringOfAnotherThenReturnsExactMatchOnly()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keep", "Keeper");

        IOsuCollection result = manager.GetCollectionByName("Keep");

        _ = result.Should().NotBeNull();
        _ = result.Name.Should().Be("Keep");
    }

    [Fact]
    public void WhenMultipleCollectionsShareNameThenReturnsFirstRegistered()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Dup", "Dup");

        _ = manager.GetCollectionByName("Dup").Should().BeSameAs(manager.LoadedCollections[0]);
    }
}
