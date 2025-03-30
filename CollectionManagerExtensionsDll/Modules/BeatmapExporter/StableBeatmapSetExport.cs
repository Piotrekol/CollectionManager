using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.BeatmapExporter;

public record StableBeatmapSetExport(Beatmap Source, string TargetFileName, string SourceDirectory)
    : BeatmapSetExport(Source, TargetFileName);
