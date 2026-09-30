namespace CollectionManager.Core.Tests.Modules.Collection.Strategies.AddOrMergeIfExistsStrategy;

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

    [Fact]
    public void WhenNameIsNewThenCollectionIsAddedWithItsBeatmaps()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection input = new(_maps) { Name = "BrandNew" };
        input.AddBeatmap(BeatmapWith("hash1", 11));
        input.AddBeatmap(BeatmapWith("hash2", 12));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([input]));

        IOsuCollection added = manager.GetCollectionByName("BrandNew");
        _ = added.Should().NotBeNull();
        _ = added.Id.Should().Be(0);
        _ = added.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["hash1", "hash2"]);
    }

    [Fact]
    public void WhenNameAlreadyExistsThenMergesBeatmapsIntoExistingMaster()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection master = Register(manager, "Existing");
        master.AddBeatmap(BeatmapWith("masterHash", 11));
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection input = new(_maps) { Name = "Existing" };
        input.AddBeatmap(BeatmapWith("addedHash", 12));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([input]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Existing");
        _ = master.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["masterHash", "addedHash"]);
    }

    [Fact]
    public void WhenBatchMixesNewAndExistingNamesThenAddsNewAndMergesExisting()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection master = Register(manager, "Existing");
        master.AddBeatmap(BeatmapWith("masterHash", 11));
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection existingInput = new(_maps) { Name = "Existing" };
        existingInput.AddBeatmap(BeatmapWith("mergedHash", 12));
        OsuCollection newInput = new(_maps) { Name = "Fresh" };
        newInput.AddBeatmap(BeatmapWith("freshHash", 13));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([existingInput, newInput]));

        _ = manager.LoadedCollections.Should().HaveCount(2);
        IOsuCollection existing = manager.GetCollectionByName("Existing");
        IOsuCollection fresh = manager.GetCollectionByName("Fresh");
        _ = existing.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["masterHash", "mergedHash"]);
        _ = fresh.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["freshHash"]);
    }

    [Fact]
    public void WhenInputSharesBeatmapHashWithMasterThenKeepsSingleCopy()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection master = Register(manager, "Existing");
        master.AddBeatmap(BeatmapWith("sharedHash", 11));
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection input = new(_maps) { Name = "Existing" };
        input.AddBeatmap(BeatmapWith("sharedHash", 11));
        input.AddBeatmap(BeatmapWith("extraHash", 12));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([input]));

        _ = master.AllBeatmaps().Should().HaveCount(2);
        _ = master.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["sharedHash", "extraHash"]);
    }

    [Fact]
    public void WhenTwoInputsShareSameNewNameThenSecondMergesIntoFirst()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection first = new(_maps) { Name = "Shared" };
        first.AddBeatmap(BeatmapWith("firstHash", 11));
        OsuCollection second = new(_maps) { Name = "Shared" };
        second.AddBeatmap(BeatmapWith("secondHash", 12));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([first, second]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Shared");
        _ = manager.GetCollectionByName("Shared_0").Should().BeNull();
        _ = manager.GetCollectionByName("Shared").AllBeatmaps().Select(beatmap => beatmap.Md5)
            .Should().BeEquivalentTo(["firstHash", "secondHash"]);
    }

    [Fact]
    public void WhenInputListIsEmptyThenLeavesLoadedCollectionsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection master = Register(manager, "Existing");
        master.AddBeatmap(BeatmapWith("keepHash", 11));
        AddOrMergeIfExistsStrategy strategy = new();

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([]));

        _ = manager.LoadedCollections.Should().ContainSingle()
            .Which.Name.Should().Be("Existing");
        _ = master.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["keepHash"]);
    }

    [Fact]
    public void WhenMergingInputWithNoBeatmapsThenExistingMasterIsUnchanged()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        OsuCollection master = Register(manager, "Existing");
        master.AddBeatmap(BeatmapWith("keepHash", 11));
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection input = new(_maps) { Name = "Existing" };

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([input]));

        _ = manager.LoadedCollections.Should().ContainSingle();
        _ = master.AllBeatmaps().Select(beatmap => beatmap.Md5).Should().BeEquivalentTo(["keepHash"]);
    }

    [Fact]
    public void WhenInputNameIsSubstringOfExistingThenTreatedAsNewCollection()
    {
        CollectionsManagerWithCounts manager = new(_maps);
        _ = Register(manager, "Existing");
        AddOrMergeIfExistsStrategy strategy = new();

        OsuCollection input = new(_maps) { Name = "Exist" };
        input.AddBeatmap(BeatmapWith("hash", 11));

        strategy.Execute(manager, CollectionEditArgs.AddOrMergeCollections([input]));

        _ = manager.LoadedCollections.Should().HaveCount(2);
        _ = manager.GetCollectionByName("Exist").Should().NotBeNull();
        _ = manager.GetCollectionByName("Existing").Should().NotBeNull();
    }
}
