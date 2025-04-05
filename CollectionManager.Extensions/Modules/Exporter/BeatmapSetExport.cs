namespace CollectionManager.Extensions.Modules.Exporter;

using CollectionManager.Core.Types;
using System.IO;

public abstract record BeatmapSetExport(Beatmap Source, string TargetFileName)
{
    public string TargetFilePath(string saveDirectory) => Path.Combine(saveDirectory, TargetFileName);
}
