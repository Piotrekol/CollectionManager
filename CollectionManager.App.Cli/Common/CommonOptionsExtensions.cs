namespace CollectionManager.App.Cli.Common;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using System.IO;

internal static class CommonOptionsExtensions
{
    public static OsuFileIo LoadOsuDatabase(this CommonOptions options)
    {
        OsuFileIo osuFileIo = new(new BeatmapExtension());

        if (options.SkipOsuLocation)
        {
            return osuFileIo;
        }

        string osuLocation = ResolveOsuLocation(options);

        if (string.IsNullOrWhiteSpace(osuLocation))
        {
            throw new InvalidOperationException("Could not find osu!");
        }

        Console.WriteLine($"Using osu! database found at \"{osuLocation}\".");
        _ = osuFileIo.OsuDatabase.Load(osuLocation, progress: null, cancellationToken: default);

        return osuFileIo;
    }

    private static string ResolveOsuLocation(CommonOptions options)
    {
        string path = options.OsuLocation;

        if (string.IsNullOrWhiteSpace(path))
        {
            OsuPathResult osuPath = OsuPathResolver.GetOsuOrLazerPath();

            if (osuPath.Type is OsuType.None)
            {
                return null;
            }

            path = Path.Combine(osuPath.Path, osuPath.Type.GetDatabaseFileName());
        }

        if (Path.HasExtension(path))
        {
            return path;
        }

        if (OsuPathResolver.IsOsuStableDirectory(path))
        {
            return Path.Combine(path, OsuType.Stable.GetDatabaseFileName());
        }

        if (OsuPathResolver.IsOsuLazerDataDirectory(path))
        {
            return Path.Combine(path, OsuType.Lazer.GetDatabaseFileName());
        }

        return path;
    }
}
