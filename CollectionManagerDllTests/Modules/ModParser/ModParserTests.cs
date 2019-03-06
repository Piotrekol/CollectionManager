using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CollectionManager.DataTypes;

namespace CollectionManager.Modules.ModParser.Tests
{
    [TestClass()]
    public class ModParserTests
    {
        private ModParser _modParser;
        [TestMethod()]
        public void IsModHidden2()
        {
            _modParser = new ModParser();
            Assert.IsFalse(_modParser.IsModHidden(Mods.Dt));
        }
        [TestMethod()]
        public void GetModsFromInt1()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Hd;
            var result = _modParser.GetModsFromInt((int)mods);
            Assert.IsTrue(mods == result);
        }
        [TestMethod()]
        public void GetModsFromInt2()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Hd;
            if (!_modParser.IsModHidden(Mods.Hd))
                _modParser.HiddenMods.Add(_modParser.AllMods.First(t => t.Value == Mods.Hd));

            var result = _modParser.GetModsFromInt((int)mods);
            Assert.IsTrue(result == Mods.Dt);
        }
        [TestMethod()]
        public void GetModsFromEnum1()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Hd | Mods.Hr;
            var result = _modParser.GetModsFromEnum((int)mods);

            Assert.IsTrue(result.Count(s => s == ',') == 2);
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Dt).LongMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hd).LongMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hr).LongMod));
        }
        [TestMethod()]
        public void GetModsFromEnum2()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Hd | Mods.Hr;
            var result = _modParser.GetModsFromEnum((int)mods, true);

            Assert.IsTrue(result.Count(s => s == ',') == 2);
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hr).ShortMod));
        }
        [TestMethod()]
        public void GetModsFromEnum3()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Hd | Mods.Hr;
            if (!_modParser.IsModHidden(Mods.Hr))
                _modParser.HiddenMods.Add(_modParser.AllMods.First(t => t.Value == Mods.Hr));


            var result = _modParser.GetModsFromEnum((int)mods, true);

            Assert.IsTrue(result.Count(s => s == ',') == 1);
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod));
        }
        [TestMethod()]
        public void ModsHiding1()
        {
            _modParser = new ModParser();
            var mods = Mods.Dt | Mods.Nc | Mods.Hd | Mods.Hr | Mods.Pf | Mods.Sd | Mods.Cm | Mods.Au;

            var result = _modParser.GetModsFromEnum((int)mods, true);

            Assert.IsTrue(result.Count(s => s == ',') == 4);
            Assert.IsFalse(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Dt).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Nc).ShortMod));

            Assert.IsFalse(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Sd).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Pf).ShortMod));

            Assert.IsFalse(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Au).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Cm).ShortMod));


            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hd).ShortMod));
            Assert.IsTrue(result.Contains(_modParser.AllMods.First(t => t.Value == Mods.Hr).ShortMod));




            Assert.IsTrue(result.Substring(result.Length - 1) != ",");
            Assert.IsTrue(result.Substring(result.Length - 1) != " ");
        }
        [TestMethod()]
        public void NoModText1()
        {
            _modParser = new ModParser();
            var mods = Mods.Omod;
            var result = _modParser.GetModsFromEnum((int)mods);

            Assert.IsTrue(result.Count(s => s == ',') == 0);
            Assert.IsTrue(result == _modParser.LongNoModText);
        }
        [TestMethod()]
        public void NoModText2()
        {
            _modParser = new ModParser();
            var mods = Mods.Omod;
            var result = _modParser.GetModsFromEnum((int)mods, true);

            Assert.IsTrue(result.Count(s => s == ',') == 0);
            Assert.IsTrue(result == _modParser.ShortNoModText);
        }
        [TestMethod()]
        public void NoModText3()
        {
            _modParser = new ModParser();
            var mods = Mods.Omod;
            _modParser.LongNoModText = "No mods enabled";
            Assert.IsTrue(_modParser.LongNoModText == "No mods enabled");
            _modParser.ShortNoModText = "N/A";
            Assert.IsTrue(_modParser.ShortNoModText == "N/A");

            var result = _modParser.GetModsFromEnum((int)mods);

            Assert.IsTrue(result.Count(s => s == ',') == 0);
            Assert.IsTrue(result == _modParser.LongNoModText);
        }
        [TestMethod()]
        public void NoModText4()
        {
            _modParser = new ModParser();
            var mods = Mods.Omod;
            _modParser.LongNoModText = "No mods enabled";
            Assert.IsTrue(_modParser.LongNoModText == "No mods enabled");
            _modParser.ShortNoModText = "N/A";
            Assert.IsTrue(_modParser.ShortNoModText == "N/A");

            var result = _modParser.GetModsFromEnum((int)mods, true);

            Assert.IsTrue(result.Count(s => s == ',') == 0);
            Assert.IsTrue(result == _modParser.ShortNoModText);
        }
    }
}