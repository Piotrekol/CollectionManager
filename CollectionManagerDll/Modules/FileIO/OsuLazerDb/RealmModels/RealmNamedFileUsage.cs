﻿using Realms;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class RealmNamedFileUsage
    : IEmbeddedObject
{
    public RealmFile File { get; set; } = null!;

    public string Filename { get; set; } = null!;
}
