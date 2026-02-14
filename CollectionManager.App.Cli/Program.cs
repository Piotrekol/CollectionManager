namespace CollectionManager.App.Cli;

using CollectionManager.App.Cli.Convert;
using CollectionManager.App.Cli.Create;
using CommandLine;

internal static class Program
{
    private static int Main(string[] args)
        => Parser.Default.ParseArguments<ConvertCommand, CreateCommand>(args)
            .MapResult(
                (ConvertCommand cmd) => cmd.Run(),
                (CreateCommand cmd) => cmd.Run(),
                _ => 1
            );
}
