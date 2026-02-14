namespace CollectionManager.App.Cli.Convert;

using CollectionManager.App.Cli.Common;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CommandLine;

[Verb("convert", HelpText = "Convert collection files between formats (.db/.osdb)")]
internal sealed class ConvertCommand : CommonOptions
{
    [Option('i', "Input", Required = true, HelpText = "Input db/osdb collection file.")]
    public string InputFilePath { get; init; }

    public int Run()
    {
        using OsuFileIo osuFileIo = this.LoadOsuDatabase();
        Console.WriteLine("Converting collections.");
        OsuCollections collections = osuFileIo.CollectionLoader.LoadCollection(InputFilePath);
        osuFileIo.CollectionLoader.SaveCollection(collections, OutputFilePath);
        Console.WriteLine("Done.");

        return 0;
    }
}
