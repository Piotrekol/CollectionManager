namespace CollectionManager.App.Cli.Create;

using CollectionManager.App.Cli.Common;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CommandLine;
using System.IO;

[Verb("create", HelpText = "Create collection from beatmap IDs or hashes")]
internal sealed class CreateCommand : CommonOptions
{
    private static readonly char[] Separator = [' ', ',', '\n', '\r', '\t'];

    [Option('b', "BeatmapIds", Required = false, HelpText = "Comma or whitespace separated list of beatmap ids. Can be also path to the file.\nYou should have all beatmapIds mentioned available locally in order to generate ready-to-use collection file, otherwise after generating upload it to https://osustats.ppy.sh/collections to get remaining data.")]
    public string BeatmapIds { get; init; }

    [Option('h', "Hashes", Required = false, HelpText = "Comma or whitespace separated list of beatmap hashes (MD5). Can be also path to the file.\nYou should have all beatmaps mentioned available locally in order to generate ready-to-use collection file.")]
    public string Hashes { get; init; }

    public int Run()
    {
        if (!Validate())
        {
            return 1;
        }

        using OsuFileIo osuFileIo = this.LoadOsuDatabase();
        Console.WriteLine("Creating collections.");

        return !string.IsNullOrWhiteSpace(BeatmapIds)
            ? ProcessBeatmapIds(osuFileIo)
            : ProcessHashes(osuFileIo);
    }

    private bool Validate()
    {
        bool hasBeatmapIds = !string.IsNullOrWhiteSpace(BeatmapIds);
        bool hasHashes = !string.IsNullOrWhiteSpace(Hashes);

        if (!hasBeatmapIds && !hasHashes)
        {
            Console.WriteLine("Error: Either --BeatmapIds or --Hashes must be provided.");
            return false;
        }

        if (hasBeatmapIds && hasHashes)
        {
            Console.WriteLine("Error: --BeatmapIds and --Hashes cannot be used together. Use one or the other.");
            return false;
        }

        return true;
    }

    private int ProcessBeatmapIds(OsuFileIo osuFileIo)
    {
        string rawBeatmapIds = BeatmapIds;

        if (File.Exists(rawBeatmapIds))
        {
            rawBeatmapIds = File.ReadAllText(rawBeatmapIds);
        }

        string[] beatmapIdArray = rawBeatmapIds.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        OsuCollection collection = new(osuFileIo.LoadedMaps) { Name = "from mapIds" };

        foreach (string beatmapId in beatmapIdArray)
        {
            if (int.TryParse(beatmapId.Trim(), out int id))
            {
                collection.AddBeatmapByMapId(id);
            }
        }

        string outputPath = GetOutputPath();
        osuFileIo.CollectionLoader.SaveCollection([collection], outputPath);
        Console.WriteLine($"Done. Created collection from {beatmapIdArray.Length} beatmap IDs.");

        return 0;
    }

    private int ProcessHashes(OsuFileIo osuFileIo)
    {
        string rawHashes = Hashes;

        if (File.Exists(rawHashes))
        {
            rawHashes = File.ReadAllText(rawHashes);
        }

        string[] hashArray = rawHashes.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        OsuCollection collection = new(osuFileIo.LoadedMaps) { Name = "from hashes" };

        foreach (string hash in hashArray)
        {
            string trimmedHash = hash.Trim();

            if (!string.IsNullOrWhiteSpace(trimmedHash))
            {
                collection.AddBeatmapByHash(trimmedHash);
            }
        }

        string outputPath = GetOutputPath();
        osuFileIo.CollectionLoader.SaveCollection([collection], outputPath);
        Console.WriteLine($"Done. Created collection from {hashArray.Length} hashes.");

        return 0;
    }

    private string GetOutputPath()
        => Path.HasExtension(OutputFilePath)
            ? OutputFilePath
            : $"{OutputFilePath}.osdb";
}
