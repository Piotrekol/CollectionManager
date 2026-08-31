namespace CollectionManager.App.Cli;

using CollectionManager.App.Cli.Logging;
using CollectionManager.App.Cli.Pipeline;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Globalization;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

internal static class Program
{
    internal static ILogger Logger { get; private set; } = default!;

    private static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            args = ["--help"];
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.With<IndentationEnricher>()
            .WriteTo.Console(
                outputTemplate: $$"""{{{IndentationEnricher.IndentationProperty}}}{Message:lj}{NewLine}""",
                formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();

        using ILoggerFactory loggerFactory = LoggerFactory
            .Create(builder => builder.AddSerilog(Log.Logger, dispose: false));

        Logger = loggerFactory.CreateLogger("CollectionManager.App.Cli");

        try
        {
            List<string[]> commands = PipelineParser.GroupArgs(args);
            return await PipelineExecutor.ExecuteAsync(commands);
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
