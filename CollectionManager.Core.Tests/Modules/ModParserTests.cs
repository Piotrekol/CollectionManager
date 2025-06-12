namespace CollectionManager.Core.Tests.Modules;

using CollectionManager.Core.Modules.Mod;
using CollectionManager.Core.Types;
using FluentAssertions;
using System.Linq;
using Xunit;

public class ModParserTests
{
    private ModParser _modParser;

    [Fact]
    public void IsModHidden2()
    {
        _modParser = new ModParser();
        _ = _modParser.IsModHidden(Mods.Dt).Should().BeFalse();
    }

    [Fact]
    public void GetModsFromInt1()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Hd;
        Mods result = _modParser.GetModsFromInt((int)mods);
        _ = result.Should().Be(mods);
    }

    [Fact]
    public void GetModsFromInt2()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Hd;
        if (!_modParser.IsModHidden(Mods.Hd))
        {
            _modParser.HiddenMods.Add(_modParser.AllMods.First(t => t.Value == Mods.Hd));
        }

        Mods result = _modParser.GetModsFromInt((int)mods);
        _ = result.Should().Be(Mods.Dt);
    }

    [Fact]
    public void GetModsFromEnum1()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Hd | Mods.Hr;
        string result = _modParser.GetModsFromEnum((int)mods);

        _ = result.Count(s => s == ',').Should().Be(2);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Dt).LongMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hd).LongMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hr).LongMod);
    }

    [Fact]
    public void GetModsFromEnum2()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Hd | Mods.Hr;
        string result = _modParser.GetModsFromEnum((int)mods, true);

        _ = result.Count(s => s == ',').Should().Be(2);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hr).ShortMod);
    }

    [Fact]
    public void GetModsFromEnum3()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Hd | Mods.Hr;
        if (!_modParser.IsModHidden(Mods.Hr))
        {
            _modParser.HiddenMods.Add(_modParser.AllMods.First(t => t.Value == Mods.Hr));
        }

        string result = _modParser.GetModsFromEnum((int)mods, true);

        _ = result.Count(s => s == ',').Should().Be(1);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod);
    }

    [Fact]
    public void ModsHiding1()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Dt | Mods.Nc | Mods.Hd | Mods.Hr | Mods.Pf | Mods.Sd | Mods.Cm | Mods.Au;

        string result = _modParser.GetModsFromEnum((int)mods, true);

        _ = result.Count(s => s == ',').Should().Be(4);
        _ = result.Should().NotContain(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Nc).ShortMod);

        _ = result.Should().NotContain(_modParser.AllMods.First(t => t.Value == Mods.Sd).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Pf).ShortMod);

        _ = result.Should().NotContain(_modParser.AllMods.First(t => t.Value == Mods.Au).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Cm).ShortMod);

        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod);
        _ = result.Should().Contain(_modParser.AllMods.First(t => t.Value == Mods.Hr).ShortMod);

        _ = result[^1].Should().NotBe(',');
        _ = result[^1].Should().NotBe(' ');
    }

    [Fact]
    public void NoModText1()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Nm;
        string result = _modParser.GetModsFromEnum((int)mods);

        _ = result.Count(s => s == ',').Should().Be(0);
        _ = result.Should().Be(_modParser.LongNoModText);
    }

    [Fact]
    public void NoModText2()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Nm;
        string result = _modParser.GetModsFromEnum((int)mods, true);

        _ = result.Count(s => s == ',').Should().Be(0);
        _ = result.Should().Be(_modParser.ShortNoModText);
    }

    [Fact]
    public void NoModText3()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Nm;
        _modParser.LongNoModText = "No mods enabled";
        _ = _modParser.LongNoModText.Should().Be("No mods enabled");
        _modParser.ShortNoModText = "N/A";
        _ = _modParser.ShortNoModText.Should().Be("N/A");

        string result = _modParser.GetModsFromEnum((int)mods);

        _ = result.Count(s => s == ',').Should().Be(0);
        _ = result.Should().Be(_modParser.LongNoModText);
    }

    [Fact]
    public void NoModText4()
    {
        _modParser = new ModParser();
        Mods mods = Mods.Nm;
        _modParser.LongNoModText = "No mods enabled";
        _ = _modParser.LongNoModText.Should().Be("No mods enabled");
        _modParser.ShortNoModText = "N/A";
        _ = _modParser.ShortNoModText.Should().Be("N/A");

        string result = _modParser.GetModsFromEnum((int)mods, true);

        _ = result.Count(s => s == ',').Should().Be(0);
        _ = result.Should().Be(_modParser.ShortNoModText);
    }
}