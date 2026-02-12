namespace CollectionManager.App.Cli;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CommandLine;
using System.IO;

internal static class CommandLineRunner
{
    private static readonly char[] separator = [' ', ',', '\n', '\r', '\t'];

    public static bool Process(string[] args)
    {
        CommandLineOptions parsedArgs = Parser.Default.ParseArguments<CommandLineOptions>(args).MapResult(o => o, _ => null);

        if (parsedArgs == null)
        {
            return false;
        }

        using OsuFileIo osuFileIo = new(new BeatmapExtension());

        if (!parsedArgs.SkipOsuLocation)
        {
            PrepareUserOsuPath(parsedArgs);

            if (string.IsNullOrWhiteSpace(parsedArgs.OsuLocation))
            {
                Console.WriteLine("Could not find osu!");
            }
            else
            {
                Console.WriteLine($"Using osu! database found at \"{parsedArgs.OsuLocation}\".");
                _ = osuFileIo.OsuDatabase.Load(parsedArgs.OsuLocation, progress: null, cancellationToken: default);
            }
        }

        if (!string.IsNullOrEmpty(parsedArgs.BeatmapIds))
        {
            Console.WriteLine("Creating collections from beatmapIds.");
            if (File.Exists(parsedArgs.BeatmapIds))
            {
                parsedArgs.BeatmapIds = File.ReadAllText(parsedArgs.BeatmapIds);
            }

            CreateCollectionFromBeatmapIds(osuFileIo, parsedArgs.BeatmapIds, parsedArgs.OutputFilePath);
        }
        else if (!string.IsNullOrEmpty(parsedArgs.InputFilePath))
        {
            Console.WriteLine("Converting collections.");
            ConvertCollection(osuFileIo, parsedArgs.InputFilePath, parsedArgs.OutputFilePath);
        }
        else
        {
            Console.WriteLine("Nothing to do.");
            return false;
        }

        Console.WriteLine("Done.");
        return true;
    }

    private static void PrepareUserOsuPath(CommandLineOptions parsedArgs)
    {
        if (string.IsNullOrWhiteSpace(parsedArgs.OsuLocation))
        {
            OsuPathResult osuPath = OsuPathResolver.GetOsuOrLazerPath();
            parsedArgs.OsuLocation = Path.Combine(osuPath.Path, osuPath.Type.GetDatabaseFileName());

            return;
        }

        if (Path.HasExtension(parsedArgs.OsuLocation))
        {
            return;
        }

        if (OsuPathResolver.IsOsuStableDirectory(parsedArgs.OsuLocation))
        {
            parsedArgs.OsuLocation = Path.Combine(parsedArgs.OsuLocation, OsuType.Stable.GetDatabaseFileName());
        }
        else if (OsuPathResolver.IsOsuLazerDataDirectory(parsedArgs.OsuLocation))
        {
            parsedArgs.OsuLocation = Path.Combine(parsedArgs.OsuLocation, OsuType.Lazer.GetDatabaseFileName());
        }
    }

    private static void ConvertCollection(OsuFileIo osuFileIo, string inputFilePath, string outputFilePath)
    {
        OsuCollections collections = osuFileIo.CollectionLoader.LoadCollection(inputFilePath);
        osuFileIo.CollectionLoader.SaveCollection(collections, outputFilePath);
    }

    private static void CreateCollectionFromBeatmapIds(OsuFileIo osuFileIo, string rawBeatmapIds, string saveLocation)
    {
        string[] beatmapIds = rawBeatmapIds?.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (rawBeatmapIds == null || beatmapIds.Length == 0)
        {
            Console.WriteLine("No beatmap ids provided.");
            return;
        }

        OsuCollection collection = new(osuFileIo.LoadedMaps) { Name = "from mapIds" };
        foreach (string beatmapId in beatmapIds)
        {
            if (int.TryParse(beatmapId.Trim(), out int id))
            {
                collection.AddBeatmapByMapId(id);
            }
        }

        osuFileIo.CollectionLoader.SaveOsdbCollection([collection], $"{saveLocation}.osdb");
    }
}
