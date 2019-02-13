using System.Collections.Generic;
using System.Collections.ObjectModel;
using CollectionManager.DataTypes;

namespace CollectionManager.Modules.ModParser
{

    public class ModParser
    {
        public class OsuMod
        {
            public Mods Value { get; }
            public string LongMod { get; set; }
            public string ShortMod { get; set; }

            public OsuMod(Mods value, string shortMod, string longMod)
            {
                Value = value;
                LongMod = longMod;
                ShortMod = shortMod;
            }
        }

        public string ShortNoModText
        {
            get { return _mods.Find(f => f.Value == Mods.Omod).ShortMod; }
            set { _mods.Find(f => f.Value == Mods.Omod).ShortMod = value; }
        }
        public string LongNoModText
        {
            get { return _mods.Find(f => f.Value == Mods.Omod).LongMod; }
            set { _mods.Find(f => f.Value == Mods.Omod).LongMod = value; }
        }
        public ReadOnlyCollection<OsuMod> AllMods => _mods.AsReadOnly();
        public List<OsuMod> HiddenMods { get; } = new List<OsuMod>
        {
            new OsuMod(Mods.Nv,"NV","NoVideo")
        };
        private readonly List<OsuMod> _mods = new List<OsuMod>()
        {
            new OsuMod(Mods.Omod, "None", "None"),
            new OsuMod(Mods.Nf, "NF", "No Fail"),
            new OsuMod(Mods.Ez, "EZ", "Easy"),
            new OsuMod(Mods.Nv,"NV","NoVideo"), //What's the purpose of this line?; the mod is now used as "Touch Device" (as seen on the api wiki, NoVideo        = 4, // Not used anymore, but can be found on old plays like Mesita on b/78239)
            new OsuMod(Mods.Hd, "HD", "Hidden"),
            new OsuMod(Mods.Hr, "HR", "Hard Rock"),
            new OsuMod(Mods.Sd, "SD", "Sudden Death"),
            new OsuMod(Mods.Dt, "DT", "Double Time"),
            new OsuMod(Mods.RX, "RL", "Relax"),
            new OsuMod(Mods.Ht, "HT", "Half Time"),
            new OsuMod(Mods.Nc, "NC", "Nightcore"),
            new OsuMod(Mods.Fl, "FL", "Flashlight"),
            new OsuMod(Mods.Au, "AU", "AutoPlay"),
            new OsuMod(Mods.So, "SO", "Spun Out"),
            new OsuMod(Mods.Ap, "AP", "Autopilot"),
            new OsuMod(Mods.Pf, "PF", "Perfect"),
            new OsuMod(Mods.K4, "4K", "4 Keys"),
            new OsuMod(Mods.K5, "5K", "5 Keys"),
            new OsuMod(Mods.K6, "6K", "6 Keys"),
            new OsuMod(Mods.K7, "7K", "7 Keys"),
            new OsuMod(Mods.K8, "8K", "8 Keys"),
            new OsuMod(Mods.Fi, "FI", "Fade In"),
            new OsuMod(Mods.Rn, "RD", "Random"),
            new OsuMod(Mods.Cm, "CN", "Cinema"),
            new OsuMod(Mods.Tp, "TP", "Target Practice"),
            new OsuMod(Mods.K9, "9K", "9 Keys"),
            new OsuMod(Mods.Coop, "CO", "Co-Op"),
            new OsuMod(Mods.K1, "1K", "1 Key"),
            new OsuMod(Mods.K3, "3K", "3 Keys"),
            new OsuMod(Mods.K2, "2K", "2 Keys"),
            new OsuMod(Mods.Sv2, "SV2", "Score V2"),
            new OsuMod(Mods.Lm, "LM", "Last mod"),
        };

        public bool IsModHidden(OsuMod mod) => HiddenMods.Exists(m => m.Value == mod.Value);
        public bool IsModHidden(Mods mod) => HiddenMods.Exists(m => m.Value == mod);

        public Mods GetModsFromInt(int mods)
        {
            Mods eMods = Mods.Omod;
            foreach (var mod in _mods)
            {
                if (!IsModHidden(mod) && (mods & (int)mod.Value) > 0)
                {
                    eMods = eMods | mod.Value;
                }
            }
            return eMods;
        }
        public string GetModsFromEnum(int modsEnum, bool shortMod = false)
        {
            System.Text.StringBuilder modStr = new System.Text.StringBuilder();

            modsEnum = (int)GetModsFromInt(modsEnum);
            foreach (var mod in _mods)
            {
                if ((modsEnum & (int)mod.Value) > 0)
                {
                    modStr.Append(shortMod ? mod.ShortMod : mod.LongMod);
                    modStr.Append(",");
                }
            }

            string retVal;
            if (modStr.Length > 1)
            {
                modStr.Remove(modStr.Length - 1, 1);
                retVal = modStr.ToString();

                if (retVal.Contains("NC"))
                {
                    retVal = retVal.Replace("DT,", "");
                }
                if (retVal.Contains("PF"))
                {
                    retVal = retVal.Replace("SD,", "");
                }
                if (retVal.Contains("CN"))
                {
                    retVal = retVal.Replace("AU,", "");
                }
            }
            else
                retVal = modStr.Append(shortMod ? _mods[0].ShortMod : _mods[0].LongMod).ToString();

            return retVal;
        }
    }
}
