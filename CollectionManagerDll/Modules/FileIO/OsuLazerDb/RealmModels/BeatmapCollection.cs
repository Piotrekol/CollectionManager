﻿using System;
using System.Collections.Generic;
using Realms;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class BeatmapCollection 
    : IRealmObject
{
    [PrimaryKey]
    public Guid ID { get; set; }

    public string Name { get; set; } = string.Empty;

    public IList<string> BeatmapMD5Hashes { get; }

    public DateTimeOffset LastModified { get; set; }
}
