namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli;
using CollectionManager.App.Cli.Pipeline;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManager.Extensions.Modules.CollectionApiGenerator;
using CommandLine;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Verb("generate", HelpText = "Generate collections from user top scores using osu! API")]
internal sealed partial class GenerateCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    [Option('u', "usernames", Required = true, HelpText = "Comma or whitespace separated list of usernames. Can also be path to a file.")]
    public required string Usernames { get; init; }

    [Option('k', "api-key", Required = true, HelpText = "osu! API key for accessing user data.")]
    public required string ApiKey { get; init; }

    [Option('p', "pattern", Required = false, HelpText = "Collection name format: {0}=username, {1}=mods. Default: \"{0} - {1}\"")]
    public string CollectionNamePattern { get; init; } = "{0} - {1}";

    [Option('g', "gamemode", Required = false, HelpText = "Game mode: 0=Osu, 1=Taiko, 2=Catch, 3=Mania. Default: 0")]
    public int Gamemode { get; init; } = 0;

    [Option("min-pp", Required = false, HelpText = "Minimum PP required for a score. Default: 0")]
    public double MinimumPp { get; init; }

    [Option("max-pp", Required = false, HelpText = "Maximum PP allowed for a score. Default: 5000")]
    public double MaximumPp { get; init; } = 5000;

    [Option("min-acc", Required = false, HelpText = "Minimum accuracy required (0-100). Default: 0")]
    public double MinimumAcc { get; init; }

    [Option("max-acc", Required = false, HelpText = "Maximum accuracy allowed (0-100). Default: 100")]
    public double MaximumAcc { get; init; } = 100;

    [Option('r', "ranks", Required = false, HelpText = "Rank filter: 0=S and better, 1=A and worse, 2=All. Default: 2")]
    public int RankFilter { get; init; } = 2;

    [Option('m', "mods", Required = false, HelpText = "Comma separated required mods (e.g., 'HD,HR'). Empty = all mods.")]
    public required string Mods { get; init; }

    public override async Task<int> RunAsync(CollectionContext context)
    {
        List<string> usernames = ParseUsernames();

        if (usernames.Count == 0)
        {
            LogNoValidUsernames();
            return 1;
        }

        _ = context.EnsureOsuDatabaseLoaded(this);
        LogGeneratingCollections(usernames.Count);

        CollectionGeneratorConfiguration configuration = CreateConfiguration(usernames);
        bool success = await GenerateCollectionsAsync(context, configuration);

        return success ? 0 : 1;
    }

    private List<string> ParseUsernames()
    {
        string rawUsernames = Usernames;

        if (File.Exists(rawUsernames))
        {
            rawUsernames = File.ReadAllText(rawUsernames);
        }

        return [.. rawUsernames.Split(CliConstants.ValueSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(username => username.Trim())
            .Where(username => !string.IsNullOrWhiteSpace(username))];
    }

    private CollectionGeneratorConfiguration CreateConfiguration(List<string> usernames)
    {
        List<Mods> modList = [];

        if (!string.IsNullOrWhiteSpace(Mods))
        {
            string[] modNames = Mods.Split(CliConstants.SimpleValueSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string modName in modNames)
            {
                if (Enum.TryParse<Mods>(modName, ignoreCase: true, out Mods mod))
                {
                    modList.Add(mod);
                }
                else
                {
                    LogInvalidMod(modName);
                }
            }
        }

        return new CollectionGeneratorConfiguration
        {
            ApiKey = ApiKey,
            Usernames = usernames,
            CollectionNameSavePattern = CollectionNamePattern,
            Gamemode = Gamemode,
            ScoreSaveConditions = new ScoreSaveConditions
            {
                MinimumPp = MinimumPp,
                MaximumPp = MaximumPp,
                MinimumAcc = MinimumAcc,
                MaximumAcc = MaximumAcc,
                RanksToGet = (RankTypes)RankFilter,
                ModCombinations = modList
            }
        };
    }

    private async Task<bool> GenerateCollectionsAsync(CollectionContext context, CollectionGeneratorConfiguration configuration)
    {
        using CollectionsApiGenerator generator = new(context.LoadedMaps);
        using CancellationTokenSource cts = new();
        Console.CancelKeyPress += cancelEventHandler;

        try
        {
            generator.StatusUpdated += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(generator.Status))
                {
                    LogGeneratorStatus(generator.Status);
                }
            };

            generator.GenerateCollection(configuration);

            await Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(100);

                    if (generator.Collections != null && generator.Collections.Count > 0)
                    {
                        break;
                    }
                }

                if (cts.Token.IsCancellationRequested)
                {
                    await generator.AbortAsync();
                }
            });

            if (cts.Token.IsCancellationRequested || generator.Collections == null)
            {
                LogGenerationAborted();
                return false;
            }

            // Add generated collections via manager to get proper Id assignment
            CollectionEditArgs args = CollectionEditArgs.AddCollections(generator.Collections);
            context.Manager.EditCollection(args);

            LogGeneratedCollections(generator.Collections.Count);
            return true;
        }
        finally
        {
            Console.CancelKeyPress -= cancelEventHandler;
        }

        void cancelEventHandler(object? s, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            LogAborting();
            cts.Cancel();
        }
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "No valid usernames provided.")]
    private partial void LogNoValidUsernames();

    [LoggerMessage(Level = LogLevel.Information, Message = "Generating collections for {Count} user(s).")]
    private partial void LogGeneratingCollections(int count);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Invalid mod '{ModName}' will be ignored.")]
    private partial void LogInvalidMod(string modName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Aborting...")]
    private partial void LogAborting();

    [LoggerMessage(Level = LogLevel.Information, Message = "{Status}")]
    private partial void LogGeneratorStatus(string status);

    [LoggerMessage(Level = LogLevel.Information, Message = "Generation was aborted.")]
    private partial void LogGenerationAborted();

    [LoggerMessage(Level = LogLevel.Information, Message = "Generated {Count} collection(s).")]
    private partial void LogGeneratedCollections(int count);
}
