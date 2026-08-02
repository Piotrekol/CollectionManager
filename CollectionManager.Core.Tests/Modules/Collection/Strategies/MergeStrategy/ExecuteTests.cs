namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.MergeStrategy;

using AwesomeAssertions;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.Collection.Strategies;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Xunit;

public sealed class ExecuteTests
{
    private readonly MapCacher _maps = new();

    private static BeatmapExtension BeatmapWith(string md5, int mapId) => new() { Md5 = md5, MapId = mapId };

    private OsuCollection Register(CollectionsManagerWithCounts manager, string name)
    {
        OsuCollection collection = new(_maps) { Name = name };
        manager.LoadedCollections.Add(collection);
        return collection;
    }

    private CollectionsManagerWithCounts ManagerWith(params string[] names)
    {
        CollectionsManagerWithCounts manager = new(_maps);

        foreach (string name in names)
        {
            manager.LoadedCollections.Add(new OsuCollection(_maps) { Name = name });
        }

        return manager;
    }

    private static void RunMerge(CollectionsManagerWithCounts manager, IReadOnlyList<string> names, string newName)
        => new MergeStrategy().Execute(manager, CollectionEditArgs.MergeCollections(names, newName));

    [Fact]
    public void WhenMergingTwoCollectionsThenUnionsBeatmapsIntoRenamedResultAndRemovesSources()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("h1", 1));
        firstCollection.AddBeatmap(BeatmapWith("h2", 2));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("h3", 3));

        RunMerge(manager, ["A", "B"], "Merged");

        _ = manager.LoadedCollections.Should().ContainSingle();
        IOsuCollection result = manager.GetCollectionByName("Merged");
        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().HaveCount(3);
        _ = manager.GetCollectionByName("A").Should().BeNull();
        _ = manager.GetCollectionByName("B").Should().BeNull();
    }

    [Fact]
    public void WhenMergingOverlappingBeatmapsThenDeduplicatesByCanonicalIdentity()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("hashA", 15));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("hashB", 15));

        RunMerge(manager, ["A", "B"], "Merged");

        IOsuCollection result = manager.GetCollectionByName("Merged");
        _ = result.AllBeatmaps().Should().ContainSingle();
    }

    [Fact]
    public void WhenMergingThreeOrMoreCollectionsThenUnionsAllIntoRenamedResult()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("h1", 1));
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("h2", 2));
        OsuCollection thirdCollection = Register(manager, "C");
        thirdCollection.AddBeatmap(BeatmapWith("h3", 3));
        thirdCollection.AddBeatmap(BeatmapWith("h4", 4));

        RunMerge(manager, ["A", "B", "C"], "Merged");

        _ = manager.LoadedCollections.Should().ContainSingle();
        IOsuCollection result = manager.GetCollectionByName("Merged");
        _ = result.AllBeatmaps().Should().HaveCount(4);
        _ = manager.GetCollectionByName("A").Should().BeNull();
        _ = manager.GetCollectionByName("B").Should().BeNull();
        _ = manager.GetCollectionByName("C").Should().BeNull();
    }

    [Fact]
    public void WhenMergingSingleCollectionThenRenamesAndPreservesBeatmaps()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("h1", 1));
        firstCollection.AddBeatmap(BeatmapWith("h2", 2));

        RunMerge(manager, ["A"], "Renamed");

        _ = manager.LoadedCollections.Should().ContainSingle();
        IOsuCollection result = manager.GetCollectionByName("Renamed");
        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().HaveCount(2);
        _ = manager.GetCollectionByName("A").Should().BeNull();
    }

    [Fact]
    public void WhenNewNameAlreadyTakenThenResultNameIsDisambiguated()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmap(BeatmapWith("h1", 1));
        _ = Register(manager, "Merged");

        RunMerge(manager, ["A"], "Merged");

        IOsuCollection result = manager.GetCollectionByName("Merged_0");
        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().ContainSingle();
        _ = manager.GetCollectionByName("Merged").Should().NotBeNull();
        _ = manager.GetCollectionByName("A").Should().BeNull();
        _ = manager.LoadedCollections.Should().HaveCount(2);
    }

    [Fact]
    public void WhenNameListIsEmptyThenIsNoOp()
    {
        CollectionsManagerWithCounts manager = ManagerWith("A", "B");

        RunMerge(manager, [], "Merged");

        _ = manager.LoadedCollections.Should().HaveCount(2);
        _ = manager.GetCollectionByName("Merged").Should().BeNull();
    }

    [Fact]
    public void WhenAllSourcesAreEmptyThenResultIsEmpty()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        _ = Register(manager, "A");
        _ = Register(manager, "B");

        RunMerge(manager, ["A", "B"], "Merged");

        _ = manager.LoadedCollections.Should().ContainSingle();
        IOsuCollection result = manager.GetCollectionByName("Merged");
        _ = result.Should().NotBeNull();
        _ = result.AllBeatmaps().Should().BeEmpty();
    }

    [Fact]
    public void WhenBeatmapsShareMapIdAcrossRepresentationsThenMergeToSingleBeatmap()
    {
        CollectionsManagerWithCounts manager = ManagerWith();
        OsuCollection firstCollection = Register(manager, "A");
        firstCollection.AddBeatmapByMapId(15);
        OsuCollection secondCollection = Register(manager, "B");
        secondCollection.AddBeatmap(BeatmapWith("realhash", 15));

        RunMerge(manager, ["A", "B"], "Merged");

        IOsuCollection result = manager.GetCollectionByName("Merged");
        _ = result.AllBeatmaps().Should().ContainSingle()
            .Which.MapId.Should().Be(15);
    }
}
