﻿using Realms;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class BeatmapDifficulty
    : IEmbeddedObject
{
    public const float DEFAULT_DIFFICULTY = 5;

    public float DrainRate { get; set; }
    public float CircleSize { get; set; }
    public float OverallDifficulty { get; set; }
    public float ApproachRate { get; set; }

    public double SliderMultiplier { get; set; } = 1.4;
    public double SliderTickRate { get; set; } = 1;
}
