namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CommandLine;
using System.Threading.Tasks;

[Verb("save", HelpText = "Save pipeline collections to file")]
internal sealed class SaveCommand : PipelineOptions, IPipelineCommand
{
    [Value(0, MetaName = "output", Required = false, HelpText = "Output .db/.osdb/.realm file (positional).")]
    public string? OutputFilePositional { get; init; }

    [Option('o', "output", Required = false, HelpText = "Output .db/.osdb/.realm file")]
    public override string? OutputFile { get => OutputFilePositional ?? field; init; }

    public override Task<int> RunAsync(CollectionContext context) =>
        // No-op. Handled in pipeline executor for all commands.
        // This is here only so there's an save-only command.
        Task.FromResult(0);
}
