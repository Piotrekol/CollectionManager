namespace CollectionManager.App.Cli.Commands;

using CollectionManager.App.Cli.Pipeline;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Verb("interactive", HelpText = "Enter interactive REPL mode. This can be used standalone or anywhere in a --then pipeline chain.")]
internal sealed partial class InteractiveCommand : PipelineOptions, IPipelineCommand
{
    private readonly ILogger _logger = Program.Logger;

    public override async Task<int> RunAsync(CollectionContext context)
    {
        LogWelcomeMessage();
        LogHelpHint();

        int commandCount = 0;

        while (true)
        {
            commandCount++;
            Console.Write($"[{commandCount}]> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            string trimmedInput = input.Trim();

            if (IsExitCommand(trimmedInput))
            {
                LogExiting();
                return 0;
            }

            await ProcessInput(context, trimmedInput);
        }
    }

    private async Task ProcessInput(CollectionContext context, string trimmedInput)
    {
        string[] args = PipelineParser.ParseLine(trimmedInput);

        if (args.Length == 0)
        {
            return;
        }

        int result = await PipelineExecutor.ExecuteSingleCommandAsync(args, context);

        if (result != 0)
        {
            LogCommandFailed(result);
        }

        return;
    }

    private static bool IsExitCommand(string input)
    {
        string lower = input.ToLowerInvariant();
        return lower is "exit" or "quit" or "resume";
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "")]
    private partial void LogWelcomeMessage();

    [LoggerMessage(Level = LogLevel.Information, Message = "Enter commands one at a time. Type 'exit', 'quit', or 'resume' to leave interactive mode.")]
    private partial void LogHelpHint();

    [LoggerMessage(Level = LogLevel.Information, Message = "Exiting interactive mode.")]
    private partial void LogExiting();

    [LoggerMessage(Level = LogLevel.Error, Message = "Command failed with exit code {ExitCode}")]
    private partial void LogCommandFailed(int exitCode);
}
