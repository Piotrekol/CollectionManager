namespace CollectionManager.App.Cli;

internal static class CliConstants
{
    /// <summary>
    /// Standard separators for splitting CLI input values.
    /// Used for parsing comma or whitespace separated lists.
    /// </summary>
    public static readonly char[] ValueSeparator = [' ', ',', '\n', '\r', '\t'];

    /// <summary>
    /// Separators for simple value lists (spaces and commas only).
    /// </summary>
    public static readonly char[] SimpleValueSeparator = [' ', ','];
}
