namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("list", aliases: ["ls"], HelpText = "List loaded collections")]
internal sealed partial class ListCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    public override Task<int> RunAsync(CollectionContext context)
    {
        if (context.Collections.Count == 0)
        {
            LogNoCollectionsLoaded();
            return Task.FromResult(0);
        }

        int displayedCount = 0;

        foreach (IOsuCollection collection in context.Collections)
        {
            int total = collection.NumberOfBeatmaps;
            int missing = collection.NumberOfMissingBeatmaps;

            string formatted = CollectionLogger.FormatCollection(collection, includeCounts: true);
            LogCollectionEntry(formatted);

            displayedCount++;
        }

        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "No collections loaded.")]
    private partial void LogNoCollectionsLoaded();

    [LoggerMessage(Level = LogLevel.Information, Message = "{Collection}")]
    private partial void LogCollectionEntry(string collection);

}
