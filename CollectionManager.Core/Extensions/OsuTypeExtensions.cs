namespace CollectionManager.Core.Extensions;

using CollectionManager.Core.Types;

public static class OsuTypeExtensions
{
    public static string GetDatabaseFileName(this OsuType osuType)
        => osuType switch
        {
            OsuType.Lazer => "client.realm",
            OsuType.Stable => "osu!.db",
            OsuType.None => null,
            _ => throw new NotImplementedException()
        };
}
