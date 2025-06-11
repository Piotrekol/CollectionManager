﻿namespace CollectionManager.Core.Types;

using System;

[Flags]
public enum Mods
{
    Nm = 0,
    Nf = 1 << 0,
    Ez = 1 << 1,
    Td = 1 << 2, //Touch device
    Hd = 1 << 3,
    Hr = 1 << 4,
    Sd = 1 << 5,
    Dt = 1 << 6,
    RX = 1 << 7,
    Ht = 1 << 8,
    Nc = 1 << 9,
    Fl = 1 << 10,
    Au = 1 << 11, //auto play
    So = 1 << 12,
    Ap = 1 << 13, //Auto pilot
    Pf = 1 << 14,
    K4 = 1 << 15,
    K5 = 1 << 16,
    K6 = 1 << 17,
    K7 = 1 << 18,
    K8 = 1 << 19,
    Fi = 1 << 20,
    Rn = 1 << 21,
    Cm = 1 << 22,
    Tp = 1 << 23,
    K9 = 1 << 24,
    Coop = 1 << 25,
    K1 = 1 << 26,
    K3 = 1 << 27,
    K2 = 1 << 28,
    V2 = 1 << 29, // score v2
    Lm = 1 << 30,
    SpeedChanging = Dt | Ht | Nc,
    MapChanging = Hr | Ez | SpeedChanging
}