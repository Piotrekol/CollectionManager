namespace CollectionManager.App.Cli.Pipeline;

using CollectionManager.App.Cli.Commands;
using CollectionManager.App.Cli.Logging;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
/// <summary>
/// Executes pipeline command chains with shared context.
/// </summary>
internal static partial class PipelineExecutor
{
    private static readonly ILogger Logger = Program.Logger;

    /// <summary>
    /// Executes a single command using a shared context.
    /// </summary>
    public static async Task<int> ExecuteSingleAsync(string[] args)
    {
        using CollectionContext context = new();
        return await ExecuteCommandAsync(args, context);
    }

    /// <summary>
    /// Executes a single command using an existing context (for interactive mode).
    /// </summary>
    public static async Task<int> ExecuteSingleCommandAsync(string[] args, CollectionContext context)
        => await ExecuteCommandAsync(args, context);

    /// <summary>
    /// Executes a series of commands in sequence using a shared CollectionContext.
    /// </summary>
    public static async Task<int> ExecuteAsync(List<string[]> commandArgsList)
    {
        using CollectionContext context = new();

        for (int i = 0; i < commandArgsList.Count; i++)
        {
            string[] args = commandArgsList[i];
            Logger.LogPipelineStep(i + 1, commandArgsList.Count, string.Join(' ', args));

            int result = await ExecuteCommandAsync(args, context);

            if (result != 0)
            {
                Logger.LogPipelineStepFailed(i + 1, result);
                return result;
            }
        }

        Logger.LogPipelineCompleted();
        return 0;
    }

    private static async Task<int> ExecuteCommandAsync(string[] args, CollectionContext context)
        => await Parser.Default
            .ParseArguments<
                ConvertCommand,
                CreateCommand,
                GenerateCommand,
                InteractiveCommand,
                LoadCommand,
                LoadOsuDbCommand,
                SaveCommand,
                ListCommand,
                RenameCommand,
                MergeCommand,
                RemoveCommand,
                PipelineHelpCommand
            >(args)
            .MapResult(
                (ConvertCommand cmd) => ExecuteCommandAsync(cmd, context),
                (CreateCommand cmd) => ExecuteCommandAsync(cmd, context),
                (GenerateCommand cmd) => ExecuteCommandAsync(cmd, context),
                (InteractiveCommand cmd) => ExecuteCommandAsync(cmd, context),
                (LoadCommand cmd) => ExecuteCommandAsync(cmd, context),
                (LoadOsuDbCommand cmd) => ExecuteCommandAsync(cmd, context),
                (SaveCommand cmd) => ExecuteCommandAsync(cmd, context),
                (ListCommand cmd) => ExecuteCommandAsync(cmd, context),
                (RenameCommand cmd) => ExecuteCommandAsync(cmd, context),
                (MergeCommand cmd) => ExecuteCommandAsync(cmd, context),
                (RemoveCommand cmd) => ExecuteCommandAsync(cmd, context),
                (PipelineHelpCommand cmd) => ExecuteCommandAsync(cmd, context),
                errors =>
                    // commandline already logs errors, do nothing.
                    Task.FromResult(1));

    private static async Task<int> ExecuteCommandAsync(PipelineOptions cmd, CollectionContext context)
    {
        using IDisposable _ = IndentationEnricher.BeginCommandScope();
        int result = await cmd.RunAsync(context);

        if (result is 0 && !string.IsNullOrWhiteSpace(cmd.OutputFile))
        {
            if (context.Collections.Count is 0)
            {
                Logger.LogNoCollectionsToSave();

                return 1;
            }

            string path = GetOutputPath(cmd.OutputFile);
            context.SaveCollectionsToFile(path);
            Logger.LogSavedToFile(path);
        }

        return result;
    }

    private static string GetOutputPath(string path)
        => Path.HasExtension(path) ? path : $"{path}.osdb";

    [LoggerMessage(Level = LogLevel.Information, Message = "Executing step {StepIndex}/{TotalSteps}: {Command}")]
    public static partial void LogPipelineStep(this ILogger logger, int stepIndex, int totalSteps, string command);

    [LoggerMessage(Level = LogLevel.Error, Message = "Step {StepIndex} failed with exit code {ExitCode}")]
    public static partial void LogPipelineStepFailed(this ILogger logger, int stepIndex, int exitCode);

    [LoggerMessage(Level = LogLevel.Information, Message = "All steps completed successfully.")]
    public static partial void LogPipelineCompleted(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Saved to {Path}")]
    public static partial void LogSavedToFile(this ILogger logger, string path);

    [LoggerMessage(Level = LogLevel.Error, Message = "No collections to save. Load, create or generate collections first.")]
    private static partial void LogNoCollectionsToSave(this ILogger logger);
}
