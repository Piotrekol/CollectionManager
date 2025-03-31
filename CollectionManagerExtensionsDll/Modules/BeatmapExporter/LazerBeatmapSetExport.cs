using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.BeatmapExporter;

public record LazerBeatmapSetExport(Beatmap Source, string TargetFileName, LazerFile[] SetFiles)
    : BeatmapSetExport(Source, TargetFileName);
