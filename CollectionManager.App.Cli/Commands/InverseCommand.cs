namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Verb("inverse", HelpText = "Inverse collection (loaded beatmaps not in the specified collections)")]
internal sealed partial class InverseCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "ids", Required = true, HelpText = "Collection Ids to inverse.")]
    public required IEnumerable<int> Ids { get; init; }

    [Option('n', "name", Required = true, HelpText = "Name for the inverted collection.")]
    public required string NewName { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        if (context.LoadedMaps.Beatmaps.Count == 0)
        {
            LogBeatmapsNotLoaded();

            return Task.FromResult(1);
        }

        List<int> collectionIds = [.. Ids];

        if (collectionIds.Count < 1)
        {
            LogAtLeastOneIdRequired();

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
        CollectionEditArgs args = CollectionEditArgs.InverseCollections(names, newCollectionName);
        context.Manager.EditCollection(args);

        LogCollectionsInversed(names.Count, CollectionLogger.FormatCollection(context.Manager.GetCollectionByName(newCollectionName)));
        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Beatmaps not loaded. Use '{LoadMapsCommandName}' command first.")]
    private partial void LogBeatmapsNotLoaded(string loadMapsCommandName = "load-maps");

    [LoggerMessage(Level = LogLevel.Error, Message = "At least 1 Id is required for inverse.")]
    private partial void LogAtLeastOneIdRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection Id(s) not found: {MissingIds}")]
    private partial void LogCollectionIdsNotFound(string missingIds);

    [LoggerMessage(Level = LogLevel.Information, Message = "Inversed {Count} collection(s) into {InverseCollection}")]
    private partial void LogCollectionsInversed(int count, string inverseCollection);
}
