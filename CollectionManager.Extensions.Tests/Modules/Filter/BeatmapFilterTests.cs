namespace CollectionManager.Extensions.Tests.Modules.Filter;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Filter;
using System.Linq;
using Xunit;

public sealed class BeatmapFilterTests
{
    [Fact]
    public void FilterShouldDisplayAllMapsByDefault()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
        ];
        Scores scores = [];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        Assert.Equal(2, visible.Count);
    }

    [Fact]
    public void HasScoreWithModsShouldReturnBeatmapsWithMatchingMods()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
        ];
        Scores scores =
        [
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=HD");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        _ = Assert.Single(visible);
        Assert.Equal("HD Map", visible[0].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldReturnEmptyIfNoMatchingMods()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
        ];
        Scores scores =
        [
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=DT");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        Assert.Empty(visible);
    }

    [Fact]
    public void HasScoreWithModsShouldReturnSingleMapForNomod()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
        ];
        Scores scores =
        [
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=NM");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        _ = Assert.Single(visible);
        Assert.Equal("NM Map", visible[0].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldReturnMultipleBeatmapsWithMatchingMods()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map 1", Md5 = "hd1" },
            new BeatmapExtension { TitleRoman = "HD Map 2", Md5 = "hd2" },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hd2", Mods = (int)Mods.Hd | (int)Mods.Hr },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm }
        ];

        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=HD");
        List<Beatmap> visible = beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5]).ToList();

        Assert.Equal(2, visible.Count);
        Assert.Contains(visible, b => b.TitleRoman == "HD Map 1");
        Assert.Contains(visible, b => b.TitleRoman == "HD Map 2");
    }

    [Fact]
    public void FilterAndOperatorShouldBeImplicit()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1", ApproachRate = 8, OverallDifficulty = 8 },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1", ApproachRate = 8, OverallDifficulty = 5 },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1", ApproachRate = 1, OverallDifficulty = 8 }
        ];
        Scores scores = [];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("ar=8 od=5");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        _ = Assert.Single(visible);
        Assert.Equal("HR Map", visible[0].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldWorkWithAndOperator()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1" },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hr1", Mods = (int)Mods.Hr | (int)Mods.Hd },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=HD AND hasscorewithmods=HR");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        _ = Assert.Single(visible);
        Assert.Equal("HR Map", visible[0].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldWorkWithOrOperator()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1" },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hr1", Mods = (int)Mods.Hr | (int)Mods.Hd },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=HD OR hasscorewithmods=HR");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        Assert.Equal(2, visible.Count);
        Assert.Equal("HD Map", visible[0].TitleRoman);
        Assert.Equal("HR Map", visible[1].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldWorkWithBothOrAndCombined()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1" },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "DT Map", Md5 = "dt1" }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hr1", Mods = (int)Mods.Hr | (int)Mods.Hd },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "dt1", Mods = (int)Mods.Dt | (int)Mods.Hr}
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=NM OR hasscorewithmods=DT AND hasscorewithmods=HR");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        Assert.Equal(2, visible.Count);
        Assert.Equal("NM Map", visible[0].TitleRoman);
        Assert.Equal("DT Map", visible[1].TitleRoman);
    }

    [Fact]
    public void HasScoreWithModsShouldAndModsWithinCondition()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1" },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1" },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1" },
            new BeatmapExtension { TitleRoman = "DT Map", Md5 = "dt1" },
            new BeatmapExtension { TitleRoman = "DT Map 2", Md5 = "dt2" }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hr1", Mods = (int)Mods.Hr | (int)Mods.Hd },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm },
            new Score { MapHash = "dt1", Mods = (int)Mods.Dt | (int)Mods.Hr},
            new Score { MapHash = "dt2", Mods = (int)Mods.Dt }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("hasscorewithmods=DTHR");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        _ = Assert.Single(visible);
    }

    [Fact]
    public void ModsShouldNotAffectSearchedMapAr()
    {
        Beatmaps beatmaps =
        [
            new BeatmapExtension { TitleRoman = "HD Map", Md5 = "hd1", ApproachRate = 9 },
            new BeatmapExtension { TitleRoman = "HR Map", Md5 = "hr1", ApproachRate = 9 },
            new BeatmapExtension { TitleRoman = "NM Map", Md5 = "nm1", ApproachRate = 9 }
        ];
        Scores scores =
        [
            new Score { MapHash = "hd1", Mods = (int)Mods.Hd },
            new Score { MapHash = "hr1", Mods = (int)Mods.Hr | (int)Mods.Hd },
            new Score { MapHash = "nm1", Mods = (int)Mods.Nm }
        ];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("mods=DT ar=9");
        List<Beatmap> visible = [.. beatmaps.Where(b => !filter.BeatmapHashHidden[b.Md5])];

        Assert.Equal(Mods.Dt, filter.MainMods);
        Assert.Equal(3, visible.Count);
    }

    [Fact]
    public void ModsShouldSetMainMods()
    {
        Beatmaps beatmaps = [];
        Scores scores = [];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("mods=DTHR ar>9");

        Assert.Equal(Mods.Dt | Mods.Hr, filter.MainMods);
    }

    [Fact]
    public void ModsShouldNotSetMainModsWhenDefinedMoreThanOnce()
    {
        Beatmaps beatmaps = [];
        Scores scores = [];
        BeatmapFilter filter = new(beatmaps, scores, new BeatmapExtension());

        filter.UpdateSearch("mods=DTHR mods=HD ar>9");

        Assert.Equal(Mods.Dt | Mods.Hr, filter.MainMods);
    }
}
