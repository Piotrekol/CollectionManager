namespace CollectionManager.App.Cli.Generate;

using CollectionManager.App.Cli.Common;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManager.Extensions.Modules.CollectionApiGenerator;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Verb("generate", HelpText = "Generate collections from user top scores using osu! API")]
internal sealed class GenerateCommand : CommonOptions
{
    private static readonly char[] Separator = [' ', ',', '\n', '\r', '\t'];

    [Option('u', "Usernames", Required = true, HelpText = "Comma or whitespace separated list of usernames. Can be also path to the file.")]
    public string Usernames { get; init; }

    [Option('k', "ApiKey", Required = true, HelpText = "osu! API key for accessing user data.")]
    public string ApiKey { get; init; }

    [Option('p', "CollectionNamePattern", Required = false, HelpText = "Collection name format pattern. {0}=username, {1}=mods. Default: \"{0} - {1}\"")]
    public string CollectionNamePattern { get; init; } = "{0} - {1}";

    [Option('g', "Gamemode", Required = false, HelpText = "Game mode: 0=Osu, 1=Taiko, 2=Catch, 3=Mania. Default: 0")]
    public int Gamemode { get; init; } = 0;

    [Option("MinPp", Required = false, HelpText = "Minimum PP required for a score. Default: 0")]
    public double MinimumPp { get; init; }

    [Option("MaxPp", Required = false, HelpText = "Maximum PP allowed for a score. Default: 5000")]
    public double MaximumPp { get; init; } = 5000;

    [Option("MinAcc", Required = false, HelpText = "Minimum accuracy required for a score (0-100). Default: 0")]
    public double MinimumAcc { get; init; }

    [Option("MaxAcc", Required = false, HelpText = "Maximum accuracy allowed for a score (0-100). Default: 100")]
    public double MaximumAcc { get; init; } = 100;

    [Option('r', "Ranks", Required = false, HelpText = "Rank filter: 0=S and better, 1=A and worse, 2=All. Default: 2")]
    public int RankFilter { get; init; } = 2;

    [Option('m', "Mods", Required = false, HelpText = "Comma separated list of required mods (e.g., 'Hd,Hr'). If empty, all mods are included.")]
    public string Mods { get; init; }

    public Task<int> RunAsync()
    {
        List<string> usernames = ParseUsernames();

        if (usernames.Count == 0)
        {
            Console.WriteLine("Error: No valid usernames provided.");
            return Task.FromResult(1);
        }

        using OsuFileIo osuFileIo = this.LoadOsuDatabase();
        Console.WriteLine($"Generating collections for {usernames.Count} user(s).");

        try
        {
            CollectionGeneratorConfiguration configuration = CreateConfiguration(usernames);
            return GenerateCollections(osuFileIo, configuration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Task.FromResult(1);
        }
    }

    private List<string> ParseUsernames()
    {
        string rawUsernames = Usernames;

        if (File.Exists(rawUsernames))
        {
            rawUsernames = File.ReadAllText(rawUsernames);
        }

        return [.. rawUsernames.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(username => username.Trim())
            .Where(username => !string.IsNullOrWhiteSpace(username))];
    }

    private CollectionGeneratorConfiguration CreateConfiguration(List<string> usernames)
    {
        List<Mods> modList = [];

        if (!string.IsNullOrWhiteSpace(Mods))
        {
            string[] modNames = Mods.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);

            foreach (string modName in modNames)
            {
                if (Enum.TryParse<Mods>(modName, ignoreCase: true, out Mods mod))
                {
                    modList.Add(mod);
                }
                else
                {
                    Console.WriteLine($"Warning: Invalid mod '{modName}' will be ignored.");
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

    private async Task<int> GenerateCollections(OsuFileIo osuFileIo, CollectionGeneratorConfiguration configuration)
    {
        using CollectionsApiGenerator generator = new(osuFileIo.LoadedMaps);
        using CancellationTokenSource cts = new();

        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            Console.WriteLine("\nAborting...");
            cts.Cancel();
        };

        // Subscribe to status updates
        generator.StatusUpdated += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(generator.Status))
            {
                Console.WriteLine(generator.Status);
            }
        };

        // Start generation
        generator.GenerateCollection(configuration);

        // Wait for completion
        await Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                await Task.Delay(100);

                // Check if task completed
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
            Console.WriteLine("Generation was aborted.");
            return 1;
        }

        // Save collections
        string outputPath = GetOutputPath();
        osuFileIo.CollectionLoader.SaveCollection(generator.Collections, outputPath);
        Console.WriteLine($"Done. Generated {generator.Collections.Count} collection(s).");

        return 0;
    }

    private string GetOutputPath()
        => Path.HasExtension(OutputFilePath)
            ? OutputFilePath
            : $"{OutputFilePath}.osdb";
}
