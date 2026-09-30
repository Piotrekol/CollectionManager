namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("pipeline", HelpText = "Show help for pipeline mode (chaining commands with --then)")]
internal sealed partial class PipelineHelpCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    public override Task<int> RunAsync(CollectionContext context)
    {
        LogPipelineHelp(@"
Chain multiple commands using --then.

Usage:
  CollectionManager.App.Cli.exe <command1> [options] --then <command2> [options] [--then ...]

Examples:

  # Create, rename, and save
  create -i ""1 2 3"" --then ls --then rename -i 0 -n ""another name"" --then save -o fromIds.osdb

  # Generate and save
  generate -u ""player"" -k ""API_KEY"" --then save -o output.osdb
  # -o verb works on any command, so you can also do:
  generate -u ""player"" -k ""API_KEY"" -o output.osdb

  # Load collections and database from osu! stable installation, then save as osdb, as transferrable backup.
  load --stable --then load-maps --stable --then convert -o C:\some\cloud\folder\backup.osdb");
        return Task.FromResult(0);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{HelpText}")]
    private partial void LogPipelineHelp(string helpText);
}
