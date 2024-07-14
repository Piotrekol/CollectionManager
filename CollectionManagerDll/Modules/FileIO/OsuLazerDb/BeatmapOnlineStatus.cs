namespace CollectionManager.Modules.FileIO.OsuLazerDb;

internal enum BeatmapOnlineStatus
{
    LocallyModified = -4,
    None = -3,
    Graveyard = -2,
    WIP = -1,
    Pending = 0,
    Ranked = 1,
    Approved = 2,
    Qualified = 3,
    Loved = 4,
}
