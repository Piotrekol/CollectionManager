namespace CollectionManager.App.Cli.Pipeline;

using CommandLine;

/// <summary>
/// Common options for all pipeline commands.
/// </summary>
internal abstract class PipelineOptions : IPipelineCommand
{
    [Option('o', "output", HelpText = "Output file. If provided, collections are saved after this command.")]
    public virtual string? OutputFile { get; init; }

    [Option('l', "osu-location", HelpText = "Location of osu! directory or osu!.db/client.realm. Auto-detected if not provided.")]
    public string? OsuLocation { get; init; }

    [Option('s', "skip-osu", HelpText = "Skip loading osu! database.")]
    public bool SkipOsu { get; init; }

    [Option("stable", HelpText = "Prefer osu! Stable during install auto-detecting.")]
    public bool PreferStable { get; init; }

    [Option("lazer", HelpText = "Prefer osu! Lazer during install auto-detecting.")]
    public bool PreferLazer { get; init; }

    public abstract Task<int> RunAsync(CollectionContext context);
}
