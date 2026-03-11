namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

[Verb("load", aliases: ["open"], HelpText = "Load collections from file")]
internal sealed partial class LoadCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Value(0, MetaName = "input", Required = false, HelpText = "Input .db/.osdb/.realm file (positional).")]
    public string? InputFilePositional { get; init; }

    [Option('i', "input", Required = false, HelpText = "Input .db/.osdb/.realm file")]
    public string? InputFile { get; init; }

    [Option('a', "auto", Required = false, HelpText = "Load collection from auto-detected osu! installation. This is assumed when --lazer or --stable is set.")]
    public bool Auto { get; init; }

    public override Task<int> RunAsync(CollectionContext context)
    {
        string? effectiveInputFile = InputFilePositional ?? InputFile;

        if (Auto || PreferLazer || PreferStable)
        {
            effectiveInputFile = ResolveAutoCollectionPath();

            if (effectiveInputFile == default)
            {
                return Task.FromResult(1);
            }
        }

        if (string.IsNullOrEmpty(effectiveInputFile))
        {
            LogInputFileRequired();

            return Task.FromResult(1);
        }

        if (!File.Exists(effectiveInputFile))
        {
            LogFileNotFound(effectiveInputFile);
            return Task.FromResult(1);
        }

        try
        {
            int loadedCount = context.LoadCollectionsFromFile(effectiveInputFile);
            LogLoadedCollections(loadedCount, effectiveInputFile);
            return Task.FromResult(0);
        }
        catch (Exception ex)
        {
            LogErrorLoadingCollections(ex.Message);
            return Task.FromResult(1);
        }
    }

    private string? ResolveAutoCollectionPath()
    {
        OsuPathResult osuPath = OsuPathResolver.GetOsuOrLazerPath();

        if (osuPath.Type is OsuType.None)
        {
            return default;
        }

        string path;
        OsuType type;

        if (PreferStable)
        {
            if (osuPath.StablePath == default)
            {
                LogPreferredInstallationNotFound("Stable");
                return default;
            }

            path = osuPath.StablePath;
            type = OsuType.Stable;
        }
        else if (PreferLazer)
        {
            if (osuPath.LazerPath == default)
            {
                LogPreferredInstallationNotFound("Lazer");
                return default;
            }

            path = osuPath.LazerPath;
            type = OsuType.Lazer;
        }
        else
        {
            path = osuPath.Path;
            type = osuPath.Type;
        }

        return type switch
        {
            OsuType.Stable => Path.Combine(path, "collection.db"),
            OsuType.Lazer => Path.Combine(path, "client.realm"),
            _ => default
        };
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Input file is required.")]
    private partial void LogInputFileRequired();

    [LoggerMessage(Level = LogLevel.Error, Message = "File not found: {Path}")]
    private partial void LogFileNotFound(string path);

    [LoggerMessage(Level = LogLevel.Error, Message = "Could not find osu! {InstallationName} installation.")]
    private partial void LogPreferredInstallationNotFound(string installationName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Loaded {Count} collection(s) from {Path}")]
    private partial void LogLoadedCollections(int count, string path);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error loading collections: {Message}")]
    private partial void LogErrorLoadingCollections(string message);
}
