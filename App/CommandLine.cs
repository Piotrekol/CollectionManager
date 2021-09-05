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
                if (File.Exists(parsedArgs.BeatmapIds))
                    parsedArgs.BeatmapIds = File.ReadAllText(parsedArgs.BeatmapIds);

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
            var beatmapIds = rawBeatmapIds?.Split(new[] { ' ', ',', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (rawBeatmapIds == null || beatmapIds.Length == 0)
            {
                Console.WriteLine("No beatmap ids provided.");
                return;
            }

            var collection = new Collection(OsuFileIo.LoadedMaps) { Name = "from mapIds" };
            foreach (var beatmapId in beatmapIds)
            {
                if (int.TryParse(beatmapId.Trim(), out var id))
                    collection.AddBeatmapByMapId(id);
            }

            OsuFileIo.CollectionLoader.SaveOsdbCollection(new Collections { collection }, $"{saveLocation}.osdb");
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
}
