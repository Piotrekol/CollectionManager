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
            public string LongMod { get; }
            public string ShortMod { get; }

            public OsuMod(Mods value, string shortMod, string longMod)
            {
                Value = value;
                LongMod = longMod;
                ShortMod = shortMod;
            }
        }

        public ReadOnlyCollection<OsuMod> AllMods => _mods.AsReadOnly();
        public List<OsuMod> HiddenMods { get; } = new List<OsuMod>
        {
            new OsuMod(Mods.Nv,"NV","NoVideo")
        };
        private readonly List<OsuMod> _mods = new List<OsuMod>()
        {
            new OsuMod(Mods.Omod, "None", "None"),
            new OsuMod(Mods.Nf, "NF", "NoFail"),
            new OsuMod(Mods.Ez, "EZ", "Easy"),
            new OsuMod(Mods.Nv,"NV","NoVideo"),
            new OsuMod(Mods.Hd, "HD", "Hidden"),
            new OsuMod(Mods.Hr, "HR", "HardRock"),
            new OsuMod(Mods.Sd, "SD", "SuddenDeath"),
            new OsuMod(Mods.Dt, "DT", "DoubleTime"),
            new OsuMod(Mods.RX, "RX", "Relax"),
            new OsuMod(Mods.Ht, "HT", "HalfTime"),
            new OsuMod(Mods.Nc, "NC", "Nightcore"),
            new OsuMod(Mods.Fl, "FL", "Flashlight"),
            new OsuMod(Mods.Ap, "AP", "Autoplay"),
            new OsuMod(Mods.So, "SO", "SpunOut"),
            new OsuMod(Mods.Rx2, "RX2", "Relax2"),
            new OsuMod(Mods.Pf, "PF", "Perfect"),
            new OsuMod(Mods.K4, "K4", "Key4"),
            new OsuMod(Mods.K5, "K5", "Key5"),
            new OsuMod(Mods.K6, "K6", "Key6"),
            new OsuMod(Mods.K7, "K7", "Key7"),
            new OsuMod(Mods.K8, "K8", "Key8"),
            new OsuMod(Mods.Fi, "FI", "FadeIn"),
            new OsuMod(Mods.Rn, "RN", "Random"),
            new OsuMod(Mods.Lm, "LM", "LastMod"),
            //new OsuMod(Mods.=, "--", "--"),
            new OsuMod(Mods.K9, "K9", "Key9"),
            new OsuMod(Mods.Coop, "Coop", "Coop"),
            new OsuMod(Mods.K1, "K1", "Key1"),
            new OsuMod(Mods.K3, "K3", "Key3"),
            new OsuMod(Mods.K2, "K2", "Key2")
        };

        public bool IsModHidden(OsuMod mod) => HiddenMods.Exists(m => m.Value == mod.Value);


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
            }
            else
                retVal = modStr.Append(shortMod ? _mods[0].ShortMod : _mods[0].LongMod).ToString();

            return retVal;
        }
    }
}