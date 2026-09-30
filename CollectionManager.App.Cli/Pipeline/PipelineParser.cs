namespace CollectionManager.App.Cli.Pipeline;

using System.Collections.Generic;

public static class PipelineParser
{
    private const string ThenDelimiter = "--then";

    /// <summary>
    /// Groups arguments into a list of command segments.
    /// Each segment is an array of arguments for a single command.
    /// </summary>
    public static List<string[]> GroupArgs(string[] args)
    {
        List<string[]> result = [];
        List<string> current = [];

        foreach (string arg in args)
        {
            if (arg == ThenDelimiter)
            {
                if (current.Count > 0)
                {
                    result.Add([.. current]);
                    current = [];
                }
            }
            else
            {
                current.Add(arg);
            }
        }

        if (current.Count > 0)
        {
            result.Add([.. current]);
        }

        return result;
    }

    public static string[] ParseLine(string line)
    {
        List<string> args = [];
        char? currentQuoteChar = null;
        string currentArg = "";

        foreach (char c in line)
        {
            if (c is '"' or '\'')
            {
                if (currentQuoteChar == null)
                {
                    // opening quote
                    currentQuoteChar = c;
                }
                else if (currentQuoteChar == c)
                {
                    // closing quote of the same type
                    currentQuoteChar = null;
                }
                else
                {
                    // ignore different quote chars
                    currentArg += c;
                }
            }
            else if (c == ' ' && currentQuoteChar == null)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg);
                    currentArg = "";
                }
            }
            else
            {
                currentArg += c;
            }
        }

        if (currentArg.Length > 0)
        {
            args.Add(currentArg);
        }

        return [.. args];
    }
}
