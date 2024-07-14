﻿using System;

public enum HitResult
{
    None,
    Miss,
    Meh,
    Ok,
    Good,
    Great,
    Perfect,
    SmallTickMiss,
    SmallTickHit,
    LargeTickMiss,
    LargeTickHit,
    SmallBonus,
    LargeBonus,
    IgnoreMiss,
    IgnoreHit,
    ComboBreak,
    SliderTailHit,
    [Obsolete("Do not use.")]
    LegacyComboIncrease = 99
}