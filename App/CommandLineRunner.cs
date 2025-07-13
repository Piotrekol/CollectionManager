namespace CollectionManagerApp;

using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CommandLine;
using System.IO;
using System.Threading;

public class CommandLineRunner
{
    private static OsuFileIo OsuFileIo => Initalizer.OsuFileIo;
    private static readonly char[] separator = new[] { ' ', ',', '\n', '\r', '\t' };

    public static bool Process(string[] args)
    {
        CommandLineOptions parsedArgs = Parser.Default.ParseArguments<CommandLineOptions>(args).MapResult((o) => o, o => null);
        if (parsedArgs == null)
        {
            return false;
        }

        if (!parsedArgs.SkipOsuLocation)
        {
            parsedArgs.OsuLocation ??= GetOsuLocation();
            if (string.IsNullOrWhiteSpace(parsedArgs.OsuLocation))
            {
                Console.WriteLine("Could not find osu!");
            }
            else
            {
                _ = OsuFileIo.OsuDatabase.Load(Path.Combine(parsedArgs.OsuLocation, "osu!.db"), null, CancellationToken.None);
            }
        }

        if (!string.IsNullOrEmpty(parsedArgs.BeatmapIds))
        {
            Console.WriteLine("Creating collections from beatmapIds.");
            if (File.Exists(parsedArgs.BeatmapIds))
            {
                parsedArgs.BeatmapIds = File.ReadAllText(parsedArgs.BeatmapIds);
            }

            CreateCollectionFromBeatmapIds(parsedArgs.BeatmapIds, parsedArgs.OutputFilePath);
        }
        else if (!string.IsNullOrEmpty(parsedArgs.InputFilePath))
        {
            Console.WriteLine("Converting collections.");
            ConvertCollection(parsedArgs.InputFilePath, parsedArgs.OutputFilePath);
        }
        else
        {
            Console.WriteLine("Nothing to do.");
            return false;
        }

        Console.WriteLine("Done.");
        return true;
    }

    private static string GetOsuLocation() => OsuPathResolver.GetOsuPath(p =>
                                                {
                                                    Console.WriteLine($"Using osu! database found at \"{p}\".");
                                                    return true;
                                                }, _ => string.Empty);

    private static void ConvertCollection(string inputFilePath, string outputFilePath)
    {
        OsuCollections collections = OsuFileIo.CollectionLoader.LoadCollection(inputFilePath);
        OsuFileIo.CollectionLoader.SaveCollection(collections, outputFilePath);
    }

    private static void CreateCollectionFromBeatmapIds(string rawBeatmapIds, string saveLocation)
    {
        string[] beatmapIds = rawBeatmapIds?.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (rawBeatmapIds == null || beatmapIds.Length == 0)
        {
            Console.WriteLine("No beatmap ids provided.");
            return;
        }

        OsuCollection collection = new(OsuFileIo.LoadedMaps) { Name = "from mapIds" };
        foreach (string beatmapId in beatmapIds)
        {
            if (int.TryParse(beatmapId.Trim(), out int id))
            {
                collection.AddBeatmapByMapId(id);
            }
        }

        OsuFileIo.CollectionLoader.SaveOsdbCollection([collection], $"{saveLocation}.osdb");
    }
}

public class CommandLineOptions
{
    [Option('o', "Output", Required = true, HelpText = "Output filename with or without path.")]
    public string OutputFilePath { get; set; }
    [Option('b', "BeatmapIds", Required = false, HelpText = "Comma or whitespace separated list of beatmap ids. Can be also path to the file. \nYou should have all beatmapIds mentioned available localy in order to generate ready-to-use collection file, otherwise after generating upload it to https://osustats.ppy.sh/collections to get remaining data.")]
    public string BeatmapIds { get; set; }
    [Option('i', "Input", Required = false, HelpText = "Input db/osdb collection file.")]
    public string InputFilePath { get; set; }
    [Option('l', "OsuLocation", Required = false, HelpText = "Location of your osu! or directory where valid osu!.db can be found. If not provided, will be found automatically.")]
    public string OsuLocation { get; set; }
    [Option('s', "SkipOsuLocation", Required = false, HelpText = "Skip loading of osu! database.")]
    public bool SkipOsuLocation { get; set; } = false;
}
