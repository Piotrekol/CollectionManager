namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Verb("duplicate", HelpText = "Duplicate a collection.")]
internal sealed partial class DuplicateCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "id", Required = true, HelpText = "Collection Id to duplicate.")]
    public required int Id { get; init; }

    [Option('n', "name", Required = true, HelpText = "Name for the duplicated collection.")]
    public required string NewName { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        List<int> collectionIds = [Id];
        IEnumerable<IOsuCollection> collections = context.Manager.GetCollectionsById(collectionIds);
        HashSet<int> foundIds = [.. collections.Select(c => c.Id)];

        if (!foundIds.Contains(Id))
        {
            LogCollectionIdNotFound(Id);

            return Task.FromResult(1);
        }

        IOsuCollection collection = collections.First();
        string newCollectionName = context.Manager.GetValidCollectionName(NewName);
        CollectionEditArgs args = CollectionEditArgs.DuplicateCollection(collection.Name, newCollectionName);
        context.Manager.EditCollection(args);

        LogCollectionDuplicated(CollectionLogger.FormatCollection(collection), CollectionLogger.FormatCollection(context.Manager.GetCollectionByName(newCollectionName)));
        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection Id {Id} not found.")]
    private partial void LogCollectionIdNotFound(int id);

    [LoggerMessage(Level = LogLevel.Information, Message = "Duplicated {OriginalCollection} to {NewCollection}")]
    private partial void LogCollectionDuplicated(string originalCollection, string newCollection);
}
