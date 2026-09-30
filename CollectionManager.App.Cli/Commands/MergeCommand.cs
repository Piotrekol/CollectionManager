namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Verb("merge", HelpText = "Merge collections into one")]
internal sealed partial class MergeCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "ids", Required = true, HelpText = "Collection Ids to merge (comma or space separated). Example: -i 1 2 3")]
    public required IEnumerable<int> Ids { get; init; }

    [Option('n', "name", Required = true, HelpText = "Name for the merged collection")]
    public required string NewName { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        List<int> collectionIds = [.. Ids];

        if (collectionIds.Count < 2)
        {
            LogAtLeastTwoIdsRequired();

            return Task.FromResult(1);
        }

        IEnumerable<IOsuCollection> collections = context.Manager.GetCollectionsById(collectionIds);
        HashSet<int> foundIds = [.. collections.Select(c => c.Id)];
        List<int> missingIds = [.. collectionIds.Where(id => !foundIds.Contains(id))];

        if (missingIds.Count > 0)
        {
            LogCollectionIdsNotFound(string.Join(", ", missingIds));

            return Task.FromResult(1);
        }

        List<string> names = [.. collections.Select(c => c.Name)];

        // Get valid name for merged collection
        string mergedName = context.Manager.GetValidCollectionName(NewName);

        // Execute merge
        CollectionEditArgs args = CollectionEditArgs.MergeCollections(names, mergedName);
        context.Manager.EditCollection(args);

        LogCollectionsMerged(names.Count, CollectionLogger.FormatCollection(context.Manager.GetCollectionByName(mergedName)));
        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "At least 2 Ids are required for merge.")]
    private partial void LogAtLeastTwoIdsRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection Id(s) not found: {MissingIds}")]
    private partial void LogCollectionIdsNotFound(string missingIds);

    [LoggerMessage(Level = LogLevel.Information, Message = "Merged {Count} collection(s) into {MergedCollection}")]
    private partial void LogCollectionsMerged(int count, string mergedCollection);
}
