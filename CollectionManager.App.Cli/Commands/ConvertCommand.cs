namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("convert", HelpText = "Convert collection files between formats (.db/.osdb/.realm)")]
internal sealed partial class ConvertCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "input", Required = true, HelpText = "Input collection file (.db/.osdb/.realm)")]
    public required string InputFile { get; init; }

    [Option('o', "output", Required = true, HelpText = "Output .db/.osdb/.realm file")]
    public override string? OutputFile { get; init; }
    public override Task<int> RunAsync(CollectionContext context)
    {
        _ = context.EnsureOsuDatabaseLoaded(this);
        LogConvertingCollections();
        CollectionLoadResult loaded = context.LoadCollectionsFromFile(InputFile);
        LogLoadedCollections(loaded.Collections.Count, InputFile);

        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Converting collections.")]
    private partial void LogConvertingCollections();

    [LoggerMessage(Level = LogLevel.Information, Message = "Loaded {Count} collection(s) from {Path}")]
    private partial void LogLoadedCollections(int count, string path);
}
