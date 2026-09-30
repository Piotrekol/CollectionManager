namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("rename", aliases: ["mv"], HelpText = "Rename a collection by Id or name")]
internal sealed partial class RenameCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Value(0, MetaName = "id", Required = false, HelpText = "Collection Id (positional).")]
    public int? IdPositional { get; init; }

    [Value(1, MetaName = "name", Required = false, HelpText = "New name for the collection (positional).")]
    public string? NewNamePositional { get; init; }

    [Option('i', "id", HelpText = "Existing collection Id. Takes precedence over --collection if both provided.")]
    public int? Id { get; init; }

    [Option('c', "collection", HelpText = "Existing collection name.")]
    public string? CollectionName { get; init; }

    [Option('n', "name", Required = false, HelpText = "New name for the collection.")]
    public string? NewName { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        int? effectiveId = IdPositional ?? Id;
        string? effectiveNewName = NewNamePositional ?? NewName;

        if (!effectiveId.HasValue && string.IsNullOrEmpty(CollectionName))
        {
            LogIdOrCollectionRequired();

            return Task.FromResult(1);
        }

        if (string.IsNullOrWhiteSpace(effectiveNewName))
        {
            LogNewNameEmpty();

            return Task.FromResult(1);
        }

        IOsuCollection? collection = context.GetCollection(effectiveId, CollectionName);

        if (collection == default)
        {
            string identifier = effectiveId.HasValue ? $"Id {effectiveId.Value}" : $"'{CollectionName}'";
            LogCollectionNotFound(identifier);

            return Task.FromResult(1);
        }

        IOsuCollection? existingWithNewName = context.GetCollection(default, effectiveNewName);

        if (existingWithNewName != default && existingWithNewName != collection)
        {
            LogCollectionNameExists(effectiveNewName);

            return Task.FromResult(1);
        }

        string oldName = collection.Name;
        CollectionEditArgs args = CollectionEditArgs.RenameCollection(collection, effectiveNewName);
        context.Manager.EditCollection(args);

        string collectionIdentifier = CollectionLogger.FormatCollection(collection.Id, oldName);
        LogRenamed(collectionIdentifier, effectiveNewName);

        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Either --id or --collection is required.")]
    private partial void LogIdOrCollectionRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "New name cannot be empty.")]
    private partial void LogNewNameEmpty();

    [LoggerMessage(Level = LogLevel.Error, Message = "Collection {Collection} not found.")]
    private partial void LogCollectionNotFound(string collection);

    [LoggerMessage(Level = LogLevel.Error, Message = "A collection named '{Collection}' already exists.")]
    private partial void LogCollectionNameExists(string collection);

    [LoggerMessage(Level = LogLevel.Information, Message = "Renamed '{Collection}' to '{NewName}'.")]
    private partial void LogRenamed(string collection, string newName);
}
