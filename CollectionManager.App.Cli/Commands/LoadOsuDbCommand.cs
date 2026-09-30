namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CommandLine;
using System.Threading.Tasks;

[Verb("load-maps", HelpText = "Load osu! database for beatmap lookups")]
internal sealed partial class LoadOsuDbCommand : PipelineOptions, IPipelineCommand
{
    public override Task<int> RunAsync(CollectionContext context)
        => Task.FromResult(context.EnsureOsuDatabaseLoaded(this) ? 0 : 1);
}
