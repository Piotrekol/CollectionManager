using CollectionManager.DataTypes;
using System.IO;

namespace CollectionManagerExtensionsDll.Modules.BeatmapExporter;

public abstract record BeatmapSetExport(Beatmap Source, string TargetFileName)
{
    public string TargetFilePath(string saveDirectory) => Path.Combine(saveDirectory, TargetFileName);
}
