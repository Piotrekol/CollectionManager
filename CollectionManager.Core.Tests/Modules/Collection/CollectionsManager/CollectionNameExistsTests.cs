namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class CollectionNameExistsTests
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
    public void WhenNameExistsThenReturnsTrue()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.CollectionNameExists("Favorites").Should().BeTrue();
    }

    [Fact]
    public void WhenNameDoesNotExistThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.CollectionNameExists("Pending").Should().BeFalse();
    }

    [Fact]
    public void WhenNoCollectionsLoadedThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.CollectionNameExists("Anything").Should().BeFalse();
    }

    [Fact]
    public void WhenNameIsSubstringOfExistingThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Keeper");

        _ = manager.CollectionNameExists("Keep").Should().BeFalse();
    }

    [Fact]
    public void WhenNameDiffersOnlyByCaseThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.CollectionNameExists("favorites").Should().BeFalse();
    }
}
