namespace CollectionManager.Extensions.Modules.Exporter;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.Utils;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BeatmapExporter
{
    private readonly string _OsuSongsOrLazerFilesDirectory;
    private readonly string _SaveDirectory;

    public BeatmapExporter(string osuSongsOrLazerFilesDirectory, string saveDirectory)
    {
        _OsuSongsOrLazerFilesDirectory = osuSongsOrLazerFilesDirectory;
        _SaveDirectory = saveDirectory;
    }

    public void ExportBeatmap(BeatmapSetExport beatmapSetExport)
    {
        try
        {
            string targetFilePath = beatmapSetExport.TargetFilePath(_SaveDirectory);

            if (File.Exists(targetFilePath))
            {
                return;
            }

            using ZipArchive zipArchive = ZipArchive.Create();

            if (beatmapSetExport is StableBeatmapSetExport stableBeatmapSetExport)
            {
                zipArchive.AddAllFromDirectory(stableBeatmapSetExport.SourceDirectory);
            }
            else if (beatmapSetExport is LazerBeatmapSetExport lazerBeatmapSetExport)
            {
                foreach (LazerFile setFile in lazerBeatmapSetExport.SetFiles)
                {
                    string sourceFilePath = Path.Combine(_OsuSongsOrLazerFilesDirectory, setFile.RelativeRealmFilePath);
                    zipArchive.AddEntry(setFile.FileName, sourceFilePath);
                }
            }
            else
            {
                throw new NotImplementedException($"Unknown beatmapSetExport type.");
            }

            using FileStream fileStream = new(targetFilePath, FileMode.Create, FileAccess.ReadWrite);
            zipArchive.SaveTo(fileStream);
        }
        catch (Exception exception)
        {
            throw new BeatmapSetExportException("Problem encountered while creating osz archive.", exception);
        }
    }

    public IEnumerable<BeatmapSetExport> ToBeatmapExportSets(IEnumerable<Beatmap> beatmaps)
    {
        Dictionary<int, Beatmaps> mapSets = beatmaps.GetBeatmapSets();
        IEnumerable<Beatmap> setFirstBeatmaps = mapSets
            .Where(beatmapSet => beatmapSet.Key > 0)
            .Select(beatmapSet => beatmapSet.Value.First())
            .Concat(mapSets
                .Where(beatmapSet => beatmapSet.Key <= 0)
                .SelectMany(beatmapSet => beatmapSet.Value));

        IEnumerable<BeatmapSetExport> exportSets = setFirstBeatmaps
            .Where(beatmap => beatmap is LazerBeatmap lazerBeatmap && !lazerBeatmap.LocalBeatmapMissing)
            .Select(beatmap =>
                new LazerBeatmapSetExport(
                    beatmap,
                    beatmap.OszFileName(),
                    ((LazerBeatmap)beatmap).SetFiles));

        exportSets = exportSets
            .Concat(setFirstBeatmaps
                .Where(beatmap => beatmap is not LazerBeatmap && !string.IsNullOrWhiteSpace(beatmap.Dir))
                .Select(beatmap =>
                    new StableBeatmapSetExport(
                        beatmap,
                        beatmap.OszFileName(),
                        beatmap.BeatmapDirectory(_OsuSongsOrLazerFilesDirectory))));

        // Whenever unsubmitted sets are involved, above creates duplicates based on different beatmap within same set. Keep only one from each set.
        return exportSets.DistinctBy(exportSet => exportSet.TargetFileName);
    }
}
