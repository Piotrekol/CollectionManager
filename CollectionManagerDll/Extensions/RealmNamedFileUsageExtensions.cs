using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels;

namespace CollectionManager.Extensions;
internal static class RealmNamedFileUsageExtensions
{
    public static LazerFile ToLazerFile(this RealmNamedFileUsage realmNamedFile)
        => new(realmNamedFile.File.Hash, realmNamedFile.Filename);
}
