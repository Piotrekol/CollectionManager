namespace CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System;

public record StableOsuDatabaseData(
    int FileDate,
    int FolderCount,
    bool AccountUnlocked,
    DateTime UnlockDate,
    string Username,
    int NumberOfBeatmaps,
    Beatmaps Beatmaps,
    int Permissions)
{
    public bool IsValid => FileDate > 0
                && Beatmaps is not null
                && Beatmaps.Count == NumberOfBeatmaps;
}
