namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Verb("remove", aliases: ["rm"], HelpText = "Remove collection(s) by Id or name")]
internal sealed partial class RemoveCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Value(0, Required = false, HelpText = "Space separated collection Ids to remove. Example: remove 1 2 3")]
    public IEnumerable<int>? PositionalIds { get; init; }

    [Option('i', "ids", Required = false, HelpText = "Space separated collection Ids to remove. Example: -i 1 2 3")]
    public required IEnumerable<int> Ids { get; init; }

    [Option('n', "names", Required = false, HelpText = "Space separated collection names to remove. Example: -n \"Collection 1\" \"Collection 2\"")]
    public required IEnumerable<string> CollectionNames { get; init; }

    public override async Task<int> RunAsync(CollectionContext context)
    {
        IEnumerable<int>? effectiveIds = PositionalIds != null && PositionalIds.Any()
            ? PositionalIds
            : Ids;

        bool hasIds = effectiveIds != null && effectiveIds.Any();
        bool hasNames = CollectionNames != null && CollectionNames.Any();

        if (!hasIds && !hasNames)
        {
            LogIdOrCollectionRequired();
            return 1;
        }

        if (hasIds && hasNames)
        {
            LogIdsAndNamesMutuallyExclusive();
            return 1;
        }

        List<IOsuCollection>? collections;

        if (hasIds)
        {
            collections = Process(
                context,
                effectiveIds!,
                c => c.Id,
                ids => LogCollectionIdsNotFound(string.Join(", ", ids)));
        }
        else
        {
            collections = Process(
                context,
                CollectionNames!,
                c => c.Name,
                names => LogCollectionNamesNotFound(string.Join(", ", names.Select(n => $"'{n}'"))));
        }

        if (collections is null)
        {
            return 1;
        }

        List<string> names = [.. collections.Select(c => c.Name)];
        CollectionEditArgs args = CollectionEditArgs.RemoveCollections(names);
        context.Manager.EditCollection(args);

        IEnumerable<string> formatted = collections.Select(CollectionLogger.FormatCollection);
        LogCollectionsRemoved(collections.Count, string.Join(", ", formatted));

        return 0;
    }

    private static List<IOsuCollection>? Process<T>(
        CollectionContext context,
        IEnumerable<T> identifiers,
        Func<IOsuCollection, T> idSelector,
        Action<List<T>> logMissing)
    {
        IEnumerable<IOsuCollection> found = context.GetCollections(identifiers, idSelector);
        HashSet<T> foundIds = [.. found.Select(idSelector)];
        List<T> missing = [.. identifiers.Where(id => !foundIds.Contains(id))];

        if (missing.Count > 0)
        {
            logMissing(missing);
            return null;
        }

        List<IOsuCollection>? collections = [];

        foreach (IOsuCollection collection in found)
        {
            if (!collections.Contains(collection))
            {
                collections.Add(collection);
            }
        }

        return collections;
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Either --id(s) or --names is required.")]
    private partial void LogIdOrCollectionRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "--id(s) and --names cannot be used together.")]
    private partial void LogIdsAndNamesMutuallyExclusive();

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection Id(s) not found: {MissingIds}")]
    private partial void LogCollectionIdsNotFound(string missingIds);

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection(s) not found: {MissingNames}")]
    private partial void LogCollectionNamesNotFound(string missingNames);

    [LoggerMessage(Level = LogLevel.Information, Message = "Removed {Count} collection(s): {Names}")]
    private partial void LogCollectionsRemoved(int count, string names);

}
