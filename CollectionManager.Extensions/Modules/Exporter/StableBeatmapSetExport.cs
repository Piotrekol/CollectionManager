namespace CollectionManager.Extensions.Modules.Exporter;

using CollectionManager.Core.Types;

public record StableBeatmapSetExport(Beatmap Source, string TargetFileName, string SourceDirectory)
    : BeatmapSetExport(Source, TargetFileName);
