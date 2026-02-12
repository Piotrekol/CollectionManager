namespace CollectionManager.App.Cli;

using CommandLine;

internal sealed class CommandLineOptions
{
    [Option('o', "Output", Required = true, HelpText = "Output filename with or without path.")]
    public string OutputFilePath { get; set; }

    [Option('b', "BeatmapIds", Required = false, HelpText = "Comma or whitespace separated list of beatmap ids. Can be also path to the file. \nYou should have all beatmapIds mentioned available locally in order to generate ready-to-use collection file, otherwise after generating upload it to https://osustats.ppy.sh/collections to get remaining data.")]
    public string BeatmapIds { get; set; }

    [Option('i', "Input", Required = false, HelpText = "Input db/osdb collection file.")]
    public string InputFilePath { get; set; }

    [Option('l', "OsuLocation", Required = false, HelpText = "Location of your osu! or directory where valid osu!.db or client.realm can be found. If not provided, will be found automatically.")]
    public string OsuLocation { get; set; }

    [Option('s', "SkipOsuLocation", Required = false, HelpText = "Skip loading of osu! database.")]
    public bool SkipOsuLocation { get; set; }
}
