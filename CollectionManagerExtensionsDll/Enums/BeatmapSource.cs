namespace CollectionManagerExtensionsDll.Enums
{
    public enum BeatmapSource
    {
        OsuDb,
        OsuApi,
        OsustatsApi,
        File,
        Api = OsuApi | OsustatsApi,
        Local = OsuDb | File
    }
}