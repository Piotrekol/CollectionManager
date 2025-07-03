namespace CollectionManager.Core.Extensions;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ReplayExtensions
{
    public static double CalculateAccuracy(this IReplay replay) => OsuScore.CalculateAccuracy(replay.PlayMode, replay.C50, replay.C100, replay.C300, replay.Miss, replay.Geki, replay.Katu);

    public static class OsuScore
    {
        /// <summary>
        /// Calculate unstable rate
        /// </summary>
        /// <param name="unstableRate">Current unstable rate</param>
        /// <param name="mods">mods used in the play</param>
        /// <returns></returns>
        public static double ConvertedUnstableRate(double unstableRate, Mods mods)
        {
            if ((mods & Mods.Dt) != 0)
            {
                return unstableRate / 1.5d;
            }

            if ((mods & Mods.Ht) != 0)
            {
                return unstableRate / 0.75d;
            }

            return unstableRate;
        }

        /// <summary>
        /// Calculate unstable rate
        /// </summary>
        /// <param name="hitErrors">array of hit offsets (how off user was from perfect hits)</param>
        /// <returns></returns>
        public static double UnstableRate(List<int> hitErrors)
        {
            if (hitErrors == null || hitErrors.Count == 0 || hitErrors.Any(x => x > 10000))
            {
                return 0;
            }

            double sum;
            try
            {
                sum = hitErrors.Sum();
            }
            catch (OverflowException)
            {
                return -1;
            }

            double average = sum / hitErrors.Count;
            double variance = 0;

            foreach (int hit in hitErrors)
            {
                variance += Math.Pow(hit - average, 2);
            }

            return Math.Sqrt(variance / hitErrors.Count) * 10;
        }

        /// <summary>
        /// Calculates accuracy
        /// </summary>
        /// <param name="mode">Currently used PlayMode</param>
        /// <param name="c50">number of 50 hits</param>
        /// <param name="c100">number of 100 hits</param>
        /// <param name="c300">number of 300 hits</param>
        /// <param name="cMiss">number of misses</param>
        /// <param name="countGeki">number of geki hits - mania only</param>
        /// <param name="countKatu">number of katu hits - ctb/mania only</param>
        /// <returns></returns>
        public static double CalculateAccuracy(PlayMode mode, int c50, int c100, int c300, int cMiss, int countGeki = 0, int countKatu = 0)
        {
            int totalHits = mode switch
            {
                PlayMode.CatchTheBeat => c50 + c100 + c300 + cMiss + countKatu,
                PlayMode.OsuMania => c50 + c100 + c300 + cMiss + countGeki + countKatu,
                _ => c50 + c100 + c300 + cMiss,//osu & taiko
            };
            if (totalHits <= 0)
            {
                return 1;
            }

            return mode switch
            {
                PlayMode.Taiko => (double)((float)((c100 * 150) + (c300 * 300)) / (totalHits * 300)),
                PlayMode.CatchTheBeat => (double)((float)(c50 + c100 + c300) / totalHits),
                PlayMode.OsuMania => (double)((float)((c50 * 50) + (c100 * 100) + (countKatu * 200) + ((c300 + countGeki) * 300)) / (totalHits * 300)),
                _ => (double)((float)((c50 * 50) + (c100 * 100) + (c300 * 300)) / (totalHits * 300)),
            };
        }

        /// <summary>
        /// Calculates osu grade
        /// </summary>
        /// <param name="mode">Currently used PlayMode</param>
        /// <param name="mods">Mods used in the play</param>
        /// <param name="acc">play accuracy - ctb/mania</param>
        /// <param name="c50">number of 50 hits - osu/taiko</param>
        /// <param name="c100">number of 100 hits - osu/taiko</param>
        /// <param name="c300">number of 300 hits - osu/taiko</param>
        /// <param name="cMiss">number of misses - osu/taiko</param>
        /// <returns></returns>
        public static OsuGrade CalculateGrade(PlayMode mode, Mods mods, double acc = 0, ushort c50 = 0, ushort c100 = 0, ushort c300 = 0, ushort cMiss = 0) => mode switch
        {
            PlayMode.Osu or PlayMode.Taiko => CalculateGradeOsuOrTaiko(mods, c50, c100, c300, cMiss),
            PlayMode.CatchTheBeat => CalculateGradeCatch(mods, acc),
            PlayMode.OsuMania => CalculateGradeMania(mods, acc),
            _ => OsuGrade.Null,
        };

        private static OsuGrade CalculateGradeOsuOrTaiko(Mods mods, ushort c50, ushort c100, ushort c300, ushort cMiss)
        {
            int totalHits = c50 + c100 + c300 + cMiss;
            // if 100% acc or if nothing has happened yet (osu assumes 100% acc then as well)
            if (c300 == totalHits || totalHits == 0)
            {
                // 100% acc, with Hidden or FlashLight its a Silver SS, otherwise SS
                return (mods & (Mods.Hd | Mods.Fl)) > 0 ? OsuGrade.SSH : OsuGrade.SS;
            }

            float ratio300 = (float)c300 / totalHits;
            float ratio50 = (float)c50 / totalHits;

            if (ratio300 > 0.9 && ratio50 <= 0.01 && cMiss == 0)
            {
                // with Hidden or FlashLight its a Silver S, otherwise S
                return (mods & (Mods.Hd | Mods.Fl)) > 0 ? OsuGrade.SH : OsuGrade.S;
            }

            if ((ratio300 > 0.8 && cMiss == 0) || ratio300 > 0.9)
            {
                return OsuGrade.A;
            }

            if ((ratio300 > 0.7 && cMiss == 0) || ratio300 > 0.8)
            {
                return OsuGrade.B;
            }

            if (ratio300 > 0.6)
            {
                return OsuGrade.C;
            }

            return OsuGrade.D;
        }

        private static OsuGrade CalculateGradeCatch(Mods mods, double acc)
        {
            if (Math.Abs(acc - 100) < double.Epsilon)
            {
                // 100% acc, with Hidden or FlashLight its a Silver SS, otherwise SS
                return (mods & (Mods.Hd | Mods.Fl)) > 0 ? OsuGrade.SSH : OsuGrade.SS;
            }

            if (acc > 98)
            {
                // with Hidden or FlashLight its a Silver S, otherwise S
                return (mods & (Mods.Hd | Mods.Fl)) > 0 ? OsuGrade.SH : OsuGrade.S;
            }

            if (acc > 94)
            {
                return OsuGrade.A;
            }

            if (acc > 90)
            {
                return OsuGrade.B;
            }

            if (acc > 85)
            {
                return OsuGrade.C;
            }

            return OsuGrade.D;
        }

        private static OsuGrade CalculateGradeMania(Mods mods, double acc)
        {
            if (Math.Abs(acc - 100) < double.Epsilon)
            {
                // 100% acc, with Hidden, FlashLight or FadeIn its a Silver SS, otherwise SS
                return (mods & (Mods.Hd | Mods.Fl | Mods.Fi)) > 0 ? OsuGrade.SSH : OsuGrade.SS;
            }

            if (acc > 95)
            {
                // with Hidden, FlashLight or FadeIn its a Silver S, otherwise S
                return (mods & (Mods.Hd | Mods.Fl | Mods.Fi)) > 0 ? OsuGrade.SH : OsuGrade.S;
            }

            if (acc > 90)
            {
                return OsuGrade.A;
            }

            if (acc > 80)
            {
                return OsuGrade.B;
            }

            if (acc > 70)
            {
                return OsuGrade.C;
            }

            return OsuGrade.D;
        }
    }

}
