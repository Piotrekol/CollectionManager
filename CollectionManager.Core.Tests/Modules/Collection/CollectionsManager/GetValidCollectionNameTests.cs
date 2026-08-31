namespace CollectionManager.Core.Tests.Modules.Collection.CollectionsManager;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class GetValidCollectionNameTests
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
    public void WhenNameIsUniqueThenReturnedAsIs()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetValidCollectionName("Fresh").Should().Be("Fresh");
    }

    [Fact]
    public void WhenNameCollidesWithLoadedCollectionThenAppendsZeroSuffix()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Dup");

        _ = manager.GetValidCollectionName("Dup").Should().Be("Dup_0");
    }

    [Fact]
    public void WhenSuffixedNamesAlreadyExistThenIncrementsUntilUnique()
    {
        CollectionsManagerWithCounts manager = ManagerWith("Dup", "Dup_0");

        _ = manager.GetValidCollectionName("Dup").Should().Be("Dup_1");
    }

    [Fact]
    public void WhenNameIsInReservedNamesThenAppendsZeroSuffix()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetValidCollectionName("Fresh", ["Fresh"]).Should().Be("Fresh_0");
    }

    [Fact]
    public void WhenSuffixedCandidateIsReservedThenContinuesIncrementing()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetValidCollectionName("Fresh", ["Fresh", "Fresh_0"]).Should().Be("Fresh_1");
    }

    [Fact]
    public void WhenReservedNamesIsNullThenTreatedAsEmpty()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetValidCollectionName("Fresh", null).Should().Be("Fresh");
    }

    [Fact]
    public void WhenDesiredNameIsEmptyThenLoopProducesSuffixedCandidate()
    {
        CollectionsManagerWithCounts manager = new(_maps);

        _ = manager.GetValidCollectionName("").Should().Be("_0");
    }
}
