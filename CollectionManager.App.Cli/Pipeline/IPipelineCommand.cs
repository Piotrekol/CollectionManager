namespace CollectionManager.App.Cli.Pipeline;

using System.Threading.Tasks;

internal interface IPipelineCommand
{
    Task<int> RunAsync(CollectionContext context);
}
