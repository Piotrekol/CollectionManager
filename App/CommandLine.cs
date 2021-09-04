using System;
using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;
using CommandLine;

namespace App
{
    public class CommandLine
    {
        private OsuFileIo OsuFileIo => Initalizer.OsuFileIo;

        public bool Process(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<CommandLineOptions>(args).MapResult((o) => o, o => null);
            if (parsedArgs == null)
                return false;

            if (!parsedArgs.SkipOsuLocation)
            {
                parsedArgs.OsuLocation = parsedArgs.OsuLocation ?? GetOsuLocation();
                if (string.IsNullOrWhiteSpace(parsedArgs.OsuLocation))
                    Console.WriteLine("Could not find osu!");
                else
                    OsuFileIo.OsuDatabase.Load(Path.Combine(parsedArgs.OsuLocation, "osu!.db"));
            }

            if (!string.IsNullOrEmpty(parsedArgs.BeatmapIds))
            {
                Console.WriteLine("Creating collections from beatmapIds.");
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

        private string GetOsuLocation()
        {
            return OsuFileIo.OsuPathResolver.GetOsuDir(p =>
            {
                Console.WriteLine($"Using osu! database found at \"{p}\".");
                return true;
            }, _ => string.Empty);
        }

        private void ConvertCollection(string inputFilePath, string outputFilePath)
        {
            var collections = OsuFileIo.CollectionLoader.LoadCollection(inputFilePath);
            OsuFileIo.CollectionLoader.SaveCollection(collections, outputFilePath);
        }

        private void CreateCollectionFromBeatmapIds(string rawBeatmapIds, string saveLocation)
        {
            var beatmapIds = rawBeatmapIds?.Split(' ', ',');
            if (rawBeatmapIds == null || beatmapIds.Length == 0)
            {
                Console.WriteLine("No beatmap ids provided.");
                return;
            }

            var collection = new Collection(OsuFileIo.LoadedMaps) { Name = "from mapIds" };
            foreach (var beatmapId in beatmapIds)
            {
                collection.AddBeatmapByMapId(int.Parse(beatmapId.Trim()));
            }

            OsuFileIo.CollectionLoader.SaveOsdbCollection(new Collections { collection }, $"{saveLocation}.osdb");
        }
    }

    public class CommandLineOptions
    {
        [Option('b', "BeatmapIds", Required = false, HelpText = "Comma or space separated list of beatmap ids.")]
        public string BeatmapIds { get; set; }
        [Option('o', "Output", Required = true, HelpText = "Output filename with or without path.")]
        public string OutputFilePath { get; set; }
        [Option('i', "Input", Required = false, HelpText = "Input db/osdb collection file.")]
        public string InputFilePath { get; set; }
        [Option('l', "OsuLocation", Required = false, HelpText = "Location of your osu! or directory where valid osu!.db can be found. If not provided, will be found automatically.")]
        public string OsuLocation { get; set; }
        [Option('s', "SkipOsuLocation", Required = false, HelpText = "Do not try to load osu beatmap database.")]
        public bool SkipOsuLocation { get; set; } = false;
    }
}
