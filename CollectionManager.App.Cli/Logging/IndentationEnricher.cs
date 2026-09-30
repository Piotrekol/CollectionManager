namespace CollectionManager.App.Cli.Logging;

using Serilog.Core;
using Serilog.Events;
using System.Threading;

/// <summary>
/// Enricher that adds indentation to log events within command execution scopes.
/// </summary>
internal sealed class IndentationEnricher : ILogEventEnricher
{
    public const string IndentationProperty = "Indentation";
    private const string SingleIndent = "    ";

    private static readonly AsyncLocal<int> ScopeDepth = new();

    public static IDisposable BeginCommandScope()
    {
        ScopeDepth.Value++;
        return new CommandScopeDisposable();
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        int depth = ScopeDepth.Value;
        string indentation = depth > 0
            ? SingleIndent
            : string.Empty;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(IndentationProperty, indentation));
    }

    private sealed class CommandScopeDisposable : IDisposable
    {
        public void Dispose() => ScopeDepth.Value--;
    }
}
