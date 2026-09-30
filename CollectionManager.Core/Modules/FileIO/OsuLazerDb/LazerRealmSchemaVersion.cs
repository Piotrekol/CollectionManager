namespace CollectionManager.Core.Modules.FileIo.OsuLazerDb;

/// <summary>
/// osu!lazer realm schema version used when writing collection files.
/// Applies to newly created files only. Existing files always keep their current schema version.
/// </summary>
public enum LazerRealmSchemaVersion
{
    /// <summary>
    /// Use the schema version of the last opened realm file.
    /// Falls back to <see cref="Latest"/> when no realm file was opened beforehand.
    /// </summary>
    LastLoaded = -2,

    /// <summary>
    /// The newest supported osu!lazer realm schema version.
    /// </summary>
    Latest = -1,

    V51 = 51,
    V52 = 52,
}
