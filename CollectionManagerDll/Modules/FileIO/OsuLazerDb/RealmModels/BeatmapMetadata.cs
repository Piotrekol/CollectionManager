﻿using Realms;

namespace CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;
internal partial class BeatmapMetadata 
    : IRealmObject
{
    public string Title { get; set; } = string.Empty;

    public string TitleUnicode { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    public string ArtistUnicode { get; set; } = string.Empty;

    public RealmUser Author { get; set; } = null!;

    public string Source { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty;

    public int PreviewTime { get; set; } = -1;

    public string AudioFile { get; set; } = string.Empty;

    public string BackgroundFile { get; set; } = string.Empty;
}
