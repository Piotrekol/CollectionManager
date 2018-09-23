using System;
using System.Collections.Generic;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules
{
    public class DifficultyCalculator
    {
        public class DifficultyCalculatorResult
        {
            public float Od { get; set; }
            public float Ar { get; set; }
            public float Cs { get; set; }
            public float Hp { get; set; }
            public double MinBpm { get; set; }
            public double MaxBpm { get; set; }
        }
        readonly float od0_ms = 79.5f,
           od10_ms = 19.5f,
           ar0_ms = 1800f,
           ar5_ms = 1200f,
           ar10_ms = 450f;

        readonly float od_ms_step = 6,
            ar_ms_step1 = 120, // ar0-5 
            ar_ms_step2 = 150; // ar5-10 

        public DifficultyCalculatorResult ApplyMods(Beatmap map, Mods mods, DifficultyCalculatorResult result=null)
        {
            if(result==null)
                result = new DifficultyCalculatorResult();

            result.Od = map.OverallDifficulty;
            result.Ar = map.ApproachRate;
            result.Cs = map.CircleSize;
            result.Hp = map.HpDrainRate;
            result.MinBpm = map.MinBpm;
            result.MaxBpm = map.MaxBpm;

            if ((mods & Mods.MapChanging) == 0)
            {
                return result;
            }

            float speed = 1;
            if ((mods & Mods.Dt) != 0 || (mods & Mods.Nc) != 0)
                speed *= 1.5f;
            if ((mods & Mods.Ht) != 0)
                speed *= 0.75f;

            float od_multiplier = 1;
            if ((mods & Mods.Hr) != 0)
                od_multiplier *= 1.4f;
            if ((mods & Mods.Ez) != 0)
                od_multiplier *= 0.5f;

            result.Od *= od_multiplier;
            float odms = od0_ms - (float)Math.Ceiling(od_ms_step * result.Od);
            //hp
            if ((mods & Mods.Ez) != 0)
                result.Hp *= 0.5f;
            else if ((mods & Mods.Hr) != 0)
                result.Hp *= 1.4f;

            //bpm
            double modifier = 1;
            if ((mods & Mods.Dt) != 0)
            {
                modifier *= 1.5;
            }
            else if ((mods & Mods.Ht) != 0)
            {
                modifier *= 0.75;
            }

            result.MinBpm *= modifier;
            result.MaxBpm *= modifier;

            //ar 
            float ar_multiplier = 1;

            if ((mods & Mods.Hr) != 0)
                ar_multiplier *= 1.4f;
            if ((mods & Mods.Ez) != 0)
                ar_multiplier *= 0.5f;

            result.Ar *= ar_multiplier;
            float arms = result.Ar <= 5
            ? (ar0_ms - ar_ms_step1 * result.Ar)
            : (ar5_ms - ar_ms_step2 * (result.Ar - 5));

            //cs 
            float cs_multiplier = 1;
            if ((mods & Mods.Hr) != 0)
                cs_multiplier *= 1.3f;
            if ((mods & Mods.Ez) != 0)
                cs_multiplier *= 0.5f;

            // stats must be capped to 0-10 before HT/DT which bring them to a range 
            // of -4.42 to 11.08 for OD and -5 to 11 for AR 
            odms = Math.Min(od0_ms, Math.Max(od10_ms, odms));
            arms = Math.Min(ar0_ms, Math.Max(ar10_ms, arms));

            // apply speed-changing mods 
            odms /= speed;
            arms /= speed;

            // convert OD and AR back into their stat form 
            //od = (-(odms - od0_ms)) / od_ms_step; 
            result.Od = (od0_ms - odms) / od_ms_step;
            result.Ar = result.Ar <= 5.0f
                ? ((ar0_ms - arms) / ar_ms_step1)
                : (5.0f + (ar5_ms - arms) / ar_ms_step2);

            result.Cs *= cs_multiplier;
            result.Cs = Math.Max(0.0f, Math.Min(10.0f, result.Cs));
            
            return result;
        }

    }
}