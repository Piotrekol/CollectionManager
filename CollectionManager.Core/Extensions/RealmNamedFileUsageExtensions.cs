namespace CollectionManager.Core.Extensions;

using CollectionManager.Core.Types;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;

internal static class RealmNamedFileUsageExtensions
{
    public static LazerFile ToLazerFile(this RealmNamedFileUsage realmNamedFile)
        => new(realmNamedFile.File.Hash, realmNamedFile.Filename);
}
