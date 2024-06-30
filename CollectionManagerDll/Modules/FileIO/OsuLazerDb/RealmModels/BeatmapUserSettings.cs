﻿using Realms;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class BeatmapUserSettings
    : IEmbeddedObject
{
    /// <summary>
    /// An audio offset that can be used for timing adjustments.
    /// </summary>
    public double Offset { get; set; }
}
