namespace CollectionManager.App.Cli.Common;

using CommandLine;

internal abstract class CommonOptions
{
    [Option('o', "Output", Required = true, HelpText = "Output filename with or without path.")]
    public string OutputFilePath { get; init; }

    [Option('l', "OsuLocation", Required = false, HelpText = "Location of your osu! or directory where valid osu!.db or client.realm can be found. If not provided, will be found automatically.")]
    public string OsuLocation { get; init; }

    [Option('s', "SkipOsuLocation", Required = false, HelpText = "Skip loading of osu! database.")]
    public bool SkipOsuLocation { get; init; }
}
