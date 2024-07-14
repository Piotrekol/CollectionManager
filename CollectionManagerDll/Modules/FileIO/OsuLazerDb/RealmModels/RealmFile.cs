﻿using Realms;
using System.Linq;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
[MapTo("File")]
internal partial class RealmFile
    : IRealmObject
{
    [PrimaryKey]
    public string Hash { get; set; } = string.Empty;

    [Backlink(nameof(RealmNamedFileUsage.File))]
    public IQueryable<RealmNamedFileUsage> Usages { get; } = null!;
}
