namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class IsCollectionNameValidTests
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
    public void WhenNameIsUniqueAndNonEmptyThenReturnsTrue()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.IsCollectionNameValid("Fresh").Should().BeTrue();
    }

    [Fact]
    public void WhenNameIsEmptyThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.IsCollectionNameValid("").Should().BeFalse();
    }

    [Fact]
    public void WhenNameIsNullThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.IsCollectionNameValid(null).Should().BeFalse();
    }

    [Fact]
    public void WhenNameAlreadyExistsThenReturnsFalse()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.IsCollectionNameValid("Favorites").Should().BeFalse();
    }

    [Fact]
    public void WhenNameDiffersOnlyByCaseFromExistingThenReturnsTrue()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Favorites");

        _ = manager.IsCollectionNameValid("favorites").Should().BeTrue();
    }
}
