namespace CollectionManager.Extensions.Modules.Exporter;
using CollectionManager.Core.Types;

public record LazerBeatmapSetExport(Beatmap Source, string TargetFileName, LazerFile[] SetFiles)
    : BeatmapSetExport(Source, TargetFileName);
