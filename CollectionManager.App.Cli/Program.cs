namespace CollectionManager.App.Cli;

internal static class Program
{
    private static int Main(string[] args)
    {
        return CommandLineRunner.Process(args) ? 0 : 1;
    }
}
