namespace CollectionManager.Core.Tests.Modules.FileIo;

using AwesomeAssertions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Modules.FileIo.OsuLazerDb;
using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
using NSubstitute;
using Realms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

public partial class OsuRealmReaderTests : IDisposable
{
    [Explicit, MapTo("RealmOnlineAsset")]
    public partial class RealmOnlineAssetFixture
        : IRealmObject
    {
        [PrimaryKey] public string Hash { get; set; } = string.Empty;
    }

    private readonly string _directory = Path.Combine(Path.GetTempPath(), "cm-realm-tests", Guid.NewGuid().ToString("N"));

    private static readonly DateTimeOffset SeededLastModified = new(2020, 1, 2, 3, 4, 5, TimeSpan.FromHours(6));

    [Fact]
    public void LoadReadsScoresAndBeatmapsFromSchema51File()
    {
        string realmFilePath = CreateRealmFile(51, withNewerStreamTable: false);
        IMapDataManager mapDataManager = Substitute.For<IMapDataManager>();
        IScoreDataManager scoreDataManager = Substitute.For<IScoreDataManager>();
        _ = scoreDataManager.Scores.Returns([]);

        new OsuLazerDatabase(mapDataManager, scoreDataManager).Load(realmFilePath, null, TestContext.Current.CancellationToken);

        scoreDataManager.Received(1).Store(Arg.Any<LazerReplay>());
        mapDataManager.Received(1).StoreBeatmap(Arg.Any<LazerBeatmap>());
    }

    [Fact]
    public void LoadReadsScoresAndBeatmapsFromSchema52File()
    {
        string realmFilePath = CreateRealmFile(52, withNewerStreamTable: true);
        IMapDataManager mapDataManager = Substitute.For<IMapDataManager>();
        IScoreDataManager scoreDataManager = Substitute.For<IScoreDataManager>();
        _ = scoreDataManager.Scores.Returns([]);

        new OsuLazerDatabase(mapDataManager, scoreDataManager).Load(realmFilePath, null, TestContext.Current.CancellationToken);

        scoreDataManager.Received(1).Store(Arg.Any<LazerReplay>());
        mapDataManager.Received(1).StoreBeatmap(Arg.Any<LazerBeatmap>());
    }

    [Theory]
    [InlineData(51, false)]
    [InlineData(52, true)]
    public void CollectionRoundtripWorksOnBothSchemaVersions(ulong schemaVersion, bool withNewerStreamTable)
    {
        string realmFilePath = CreateRealmFile(schemaVersion, withNewerStreamTable);
        LazerCollectionHandler handler = new();

        OsuCollections read = [.. handler.Read(realmFilePath, new MapCacher())];

        _ = read.Should().HaveCount(1);
        _ = read[0].Name.Should().Be("test collection");
        _ = read[0].LazerId.Should().NotBe(Guid.Empty);

        handler.Write(read, realmFilePath);

        OsuCollections reread = [.. handler.Read(realmFilePath, new MapCacher())];

        _ = reread.Should().HaveCount(1);
        _ = reread[0].Name.Should().Be("test collection");
        _ = reread[0].AllBeatmaps().Should().HaveCount(read[0].AllBeatmaps().Count());
        _ = reread[0].LazerId.Should().Be(read[0].LazerId, "saving must keep existing collection IDs");

        using LazerRealm opened = TestRealmReader.Open(realmFilePath);
        _ = opened.Realm.All<BeatmapCollection>().First().LastModified.Should().Be(SeededLastModified,
            "an unchanged collection must keep its LastModified");
    }

    [Fact]
    public void LoadUnknownSchemaVersionThrowsWithActualVersion()
    {
        string realmFilePath = CreateRealmFile(99, withNewerStreamTable: false);
        IMapDataManager mapDataManager = Substitute.For<IMapDataManager>();
        IScoreDataManager scoreDatabase = Substitute.For<IScoreDataManager>();

        Action load = () => new OsuLazerDatabase(mapDataManager, scoreDatabase).Load(realmFilePath, null, default);

        _ = load.Should().Throw<RealmNotValidatedException>()
            .WithMessage("*Supported schema versions: '*'*got: '99'*");
    }

    [Fact]
    public void WriteNewFileDefaultsToLastLoadedVersion()
    {
        LazerCollectionHandler handler = new();

        string source51 = CreateRealmFile(51, withNewerStreamTable: false);
        OsuCollections collections51 = [.. handler.Read(source51, new MapCacher())];
        string newFrom51 = Path.Combine(_directory, "new-from-51.realm");
        handler.Write(collections51, newFrom51);
        _ = TestRealmReader.Open(newFrom51).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V51);

        string source52 = CreateRealmFile(52, withNewerStreamTable: true);
        OsuCollections collections52 = [.. handler.Read(source52, new MapCacher())];
        string newFrom52 = Path.Combine(_directory, "new-from-52.realm");
        handler.Write(collections52, newFrom52);
        _ = TestRealmReader.Open(newFrom52).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V52);
    }

    [Fact]
    public void WriteNewFileExplicitTargetVersionOverridesLastLoaded()
    {
        string source51 = CreateRealmFile(51, withNewerStreamTable: false);
        LazerCollectionHandler handler = new();
        OsuCollections collections = [.. handler.Read(source51, new MapCacher())];

        string explicitV52Path = Path.Combine(_directory, "explicit-v52.realm");
        handler.Write(collections, explicitV52Path, LazerRealmSchemaVersion.V52);
        _ = TestRealmReader.Open(explicitV52Path).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V52);

        string latestPath = Path.Combine(_directory, "latest.realm");
        handler.Write(collections, latestPath, LazerRealmSchemaVersion.Latest);
        _ = TestRealmReader.Open(latestPath).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V52);
    }

    [Fact]
    public void WriteExistingFileKeepsItsVersionRegardlessOfTarget()
    {
        string file51 = CreateRealmFile(51, withNewerStreamTable: false);
        string file52 = CreateRealmFile(52, withNewerStreamTable: true);
        LazerCollectionHandler handler = new();

        OsuCollections collections51 = [.. handler.Read(file51, new MapCacher())];
        handler.Write(collections51, file51, LazerRealmSchemaVersion.V52);
        _ = TestRealmReader.Open(file51).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V51);

        OsuCollections collections52 = [.. handler.Read(file52, new MapCacher())];
        handler.Write(collections52, file52, LazerRealmSchemaVersion.V51);
        _ = TestRealmReader.Open(file52).SchemaVersion.Should().Be(LazerRealmSchemaVersion.V52);
    }

    [Fact]
    public void LoadAfterWriteToNewFileDoesNotCrash()
    {
        string source51 = CreateRealmFile(51, withNewerStreamTable: false);
        LazerCollectionHandler handler = new();
        OsuCollections collections = [.. handler.Read(source51, new MapCacher())];

        string exportedPath = Path.Combine(_directory, "exported.realm");
        handler.Write(collections, exportedPath);

        IMapDataManager mapDataManager = Substitute.For<IMapDataManager>();
        IScoreDataManager scoreDataManager = Substitute.For<IScoreDataManager>();
        _ = scoreDataManager.Scores.Returns([]);

        // Loading enumerates Score/BeatmapSet, so a newly created file must contain those tables.
        // Realm kills the process with a native access violation otherwise.
        new OsuLazerDatabase(mapDataManager, scoreDataManager).Load(exportedPath, null, TestContext.Current.CancellationToken);

        scoreDataManager.DidNotReceive().Store(Arg.Any<LazerReplay>());
        mapDataManager.DidNotReceive().StoreBeatmap(Arg.Any<LazerBeatmap>());
    }

    [Fact]
    public void WriteWhileReadIteratorHoldsFileOpenThrowsClearError()
    {
        string realmFilePath = CreateRealmFile(51, withNewerStreamTable: false);
        LazerCollectionHandler handler = new();

        using IEnumerator<OsuCollection> heldOpenRead = handler.Read(realmFilePath, new MapCacher()).GetEnumerator();
        _ = heldOpenRead.MoveNext();

        Action write = () => handler.Write([], realmFilePath);

        _ = write.Should().Throw<RealmNotValidatedException>()
            .WithMessage("*already open in this process*");
    }

    private sealed class TestRealmReader
        : OsuRealmReader
    {
        public static LazerRealm Open(string realmFilePath)
            => OpenRealm(realmFilePath);
    }

    private string CreateRealmFile(ulong schemaVersion, bool withNewerStreamTable)
    {
        _ = Directory.CreateDirectory(_directory);
        string path = Path.Combine(_directory, $"client_{schemaVersion}{(withNewerStreamTable ? "-b" : string.Empty)}.realm");
        List<Type> types = [.. new LazerRealmAdapter51().ObjectTypes];
        if (withNewerStreamTable)
        {
            types.Add(typeof(RealmOnlineAssetFixture));
        }

        RealmConfiguration config = new(path) { SchemaVersion = schemaVersion, Schema = types.ToArray() };
        using Realm realm = Realm.GetInstance(config);

        realm.Write(() =>
        {
            RulesetInfo ruleset = new("osu", "osu!", "osu.Game.Rulesets.Osu", 0);

            BeatmapSetInfo beatmapSet = new()
            {
                OnlineID = 1,
                Beatmaps = { NewBeatmap(ruleset) },
                Files = { new RealmNamedFileUsage { File = new RealmFile { Hash = "filehash1" }, Filename = "audio.mp3" } },
            };
            beatmapSet.Beatmaps[0].BeatmapSet = beatmapSet;

            ScoreInfo score = new()
            {
                Ruleset = ruleset,
                RealmUser = new RealmUser { Username = "testuser" },
                BeatmapHash = beatmapSet.Beatmaps[0].MD5Hash,
                TotalScore = 123_456,
                ClientVersion = "2026.1.1.0",
                Hash = "scorehash1",
                RankInt = 5, // ScoreRank.SH
            };

            BeatmapCollection collection = new() { ID = Guid.NewGuid(), Name = "test collection", LastModified = SeededLastModified };
            collection.BeatmapMD5Hashes.Add(beatmapSet.Beatmaps[0].MD5Hash);

            _ = realm.Add(beatmapSet);
            _ = realm.Add(score);
            _ = realm.Add(collection);
        });

        return path;
    }

    private static BeatmapInfo NewBeatmap(RulesetInfo ruleset)
        => new()
        {
            MD5Hash = "md5hash1",
            Hash = "hash1",
            DifficultyName = "Normal",
            OnlineID = 2,
            Ruleset = ruleset,
            Metadata = new BeatmapMetadata
            {
                Title = "Title",
                Artist = "Artist",
                Author = new RealmUser { Username = "mapper" },
                AudioFile = "audio.mp3",
                BackgroundFile = "bg.jpg",
            },
            Difficulty = new BeatmapDifficulty(),
            UserSettings = new BeatmapUserSettings(),
        };

    public void Dispose()
    {
        try
        {
            Directory.Delete(_directory, true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
