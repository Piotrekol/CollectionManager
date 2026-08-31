namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Verb("difference", HelpText = "Find difference between collections (beatmaps that are present in only one collection).")]
internal sealed partial class DifferenceCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "ids", Required = true, HelpText = "Collection Ids to compare (comma or space separated).")]
    public required IEnumerable<int> Ids { get; init; }

    [Option('n', "name", Required = true, HelpText = "Name for the created collection.")]
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
        string newCollectionName = context.Manager.GetValidCollectionName(NewName);
        CollectionEditArgs args = CollectionEditArgs.DifferenceCollections(names, newCollectionName);
        context.Manager.EditCollection(args);

        LogCollectionsDifferenced(names.Count, CollectionLogger.FormatCollection(context.Manager.GetCollectionByName(newCollectionName)));
        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "At least 2 Ids are required for difference.")]
    private partial void LogAtLeastTwoIdsRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection Id(s) not found: {MissingIds}")]
    private partial void LogCollectionIdsNotFound(string missingIds);

    [LoggerMessage(Level = LogLevel.Information, Message = "Found difference of {Count} collection(s) into {DifferenceCollection}")]
    private partial void LogCollectionsDifferenced(int count, string differenceCollection);
}
