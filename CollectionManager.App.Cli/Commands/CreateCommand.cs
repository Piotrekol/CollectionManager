namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli;
using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("create", HelpText = "Create collection from beatmap IDs or hashes")]
internal sealed partial class CreateCommand : PipelineOptions
{
    private readonly ILogger _logger = Program.Logger;

    [Option('i', "ids", Required = false, HelpText = "Comma or whitespace separated beatmap IDs. Can be path to file.")]
    public required string BeatmapIds { get; init; }

    [Option('h', "hashes", Required = false, HelpText = "Comma or whitespace separated beatmap hashes (MD5). Can be path to file.")]
    public required string Hashes { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        string? input = GetInput();

        if (input is null)
        {
            return Task.FromResult(1);
        }

        _ = context.EnsureOsuDatabaseLoaded(this);
        LogCreatingCollections();
        string[] idOrHashArray = input.Split(CliConstants.ValueSeparator, StringSplitOptions.RemoveEmptyEntries);
        OsuCollection collection = !string.IsNullOrWhiteSpace(BeatmapIds)
            ? ProcessBeatmapIds(context, idOrHashArray)
            : ProcessHashes(context, idOrHashArray);

        CollectionEditArgs args = CollectionEditArgs.AddCollections([collection]);
        context.Manager.EditCollection(args);

        string collectionIdentifier = CollectionLogger.FormatCollection(collection);
        LogCreatedFromEntries(collectionIdentifier, idOrHashArray.Length);

        return Task.FromResult(0);
    }

    private string? GetInput()
    {
        bool hasBeatmapIds = !string.IsNullOrWhiteSpace(BeatmapIds);
        bool hasHashes = !string.IsNullOrWhiteSpace(Hashes);

        if (!hasBeatmapIds && !hasHashes)
        {
            LogBeatmapIdsOrHashesRequired();
            return default;
        }

        if (hasBeatmapIds && hasHashes)
        {
            LogBeatmapIdsAndHashesMutuallyExclusive();
            return default;
        }

        if (hasBeatmapIds)
        {
            return BeatmapIds;
        }

        return Hashes;
    }

    private static OsuCollection ProcessBeatmapIds(CollectionContext context, string[] beatmapIdArray)
    {
        OsuCollection collection = new(context.LoadedMaps) { Name = "from mapIds" };

        foreach (string beatmapId in beatmapIdArray)
        {
            if (int.TryParse(beatmapId.Trim(), out int id))
            {
                collection.AddBeatmapByMapId(id);
            }
        }

        return collection;
    }

    private static OsuCollection ProcessHashes(CollectionContext context, string[] hashArray)
    {
        OsuCollection collection = new(context.LoadedMaps) { Name = "from hashes" };

        foreach (string hash in hashArray)
        {
            string trimmedHash = hash.Trim();

            if (!string.IsNullOrWhiteSpace(trimmedHash))
            {
                collection.AddBeatmapByHash(trimmedHash);
            }
        }

        return collection;
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Either --ids or --hashes must be provided.")]
    private partial void LogBeatmapIdsOrHashesRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "--ids and --hashes cannot be used together.")]
    private partial void LogBeatmapIdsAndHashesMutuallyExclusive();

    [LoggerMessage(Level = LogLevel.Information, Message = "Creating collections.")]
    private partial void LogCreatingCollections();

    [LoggerMessage(Level = LogLevel.Information, Message = "Created collection {Collection} from {Count} entries.")]
    private partial void LogCreatedFromEntries(string collection, int count);
}
