namespace CollectionManager.App.Cli;

using CollectionManager.App.Cli.Convert;
using CollectionManager.App.Cli.Create;
using CollectionManager.App.Cli.Generate;
using CommandLine;
using System.Threading.Tasks;

internal static class Program
{
    private static async Task<int> Main(string[] args)
        => await Parser.Default.ParseArguments<ConvertCommand, CreateCommand, GenerateCommand>(args)
            .MapResult(
                (ConvertCommand cmd) => cmd.RunAsync(),
                (CreateCommand cmd) => cmd.RunAsync(),
                (GenerateCommand cmd) => cmd.RunAsync(),
                _ => Task.FromResult(1)
            );
}
