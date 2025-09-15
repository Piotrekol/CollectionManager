namespace CollectionManager.App.Shared.Presenters.Forms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Extensions;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Exporter;
using System.IO;
using System.Text;
using System.Threading;

public sealed class BeatmapExportFormPresenter
{
    private readonly IUserDialogs _UserDialogs;
    private readonly BeatmapExporter _BeatmapExporter;

    public BeatmapExportFormPresenter(IUserDialogs userDialogs, BeatmapExporter beatmapExporter)
    {
        _UserDialogs = userDialogs;
        _BeatmapExporter = beatmapExporter;
    }

    public async Task ExportAsync(List<Beatmap> beatmaps, string saveDirectory)
    {
        Progress<string> stringProgress = new();
        Progress<int> percentageProgress = new();
        IProgress<string> stringProgressReporter = stringProgress;
        IProgress<int> percentageProgressReporter = percentageProgress;

        using CancellationTokenSource cancelationTokenSource = new();
        IProgressForm progressForm = await _UserDialogs.CreateProgressFormAsync(stringProgress, percentageProgress);
        progressForm.AbortClicked += (_, __) =>
        {
            if (!cancelationTokenSource.TryCancel())
            {
                progressForm.Close();
            }
        };

        progressForm.Show();
        BeatmapSetExport[] exportSets = _BeatmapExporter
            .ToBeatmapExportSets(beatmaps)
            .ToArray();

        await StartExportAsync(saveDirectory, stringProgressReporter, percentageProgressReporter, exportSets, cancelationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task StartExportAsync(string saveDirectory, IProgress<string> stringProgressReporter, IProgress<int> percentageProgressReporter, BeatmapSetExport[] exportSets, CancellationToken cancelationToken)
    {
        try
        {
            Dictionary<BeatmapSetExport, Exception> failedExports = await ExportBeatmapsAsync(stringProgressReporter, percentageProgressReporter, exportSets, cancelationToken).ConfigureAwait(false);

            if (failedExports.Count != 0)
            {
                File.WriteAllText(Path.Combine(saveDirectory, "log.txt"), CreateErrorLogs(failedExports));
                stringProgressReporter.Report($"Processed {exportSets.Length - failedExports.Count} of {exportSets.Length} map sets.{Environment.NewLine}{failedExports.Count} map sets failed to save, see full log in log.txt file in export directory.");
            }
            else
            {
                stringProgressReporter.Report($"Processed {exportSets.Length} map sets without issues.");
            }
        }
        catch (OperationCanceledException)
        {
            stringProgressReporter.Report($"Processing cancelled.");
        }
    }

    private static string CreateErrorLogs(Dictionary<BeatmapSetExport, Exception> failedExports)
    {
        StringBuilder stringBuilder = new();

        foreach (KeyValuePair<BeatmapSetExport, Exception> kv in failedExports)
        {
            BeatmapSetExport beatmapSetExport = kv.Key;

            _ = beatmapSetExport is StableBeatmapSetExport stableSetExport
                ? stringBuilder.AppendFormat("Failed processing stable map set located at \"{0}\" ", stableSetExport.SourceDirectory)
                : stringBuilder.AppendFormat("Failed processing lazer map set named \"{0}\"", beatmapSetExport.TargetFileName);

            _ = stringBuilder.AppendFormat(" with exception: {0}{1}", kv.Value.InnerException, Environment.NewLine);
        }

        return stringBuilder.ToString();
    }

    private Task<Dictionary<BeatmapSetExport, Exception>> ExportBeatmapsAsync(IProgress<string> stringProgressReporter, IProgress<int> percentageProgressReporter, BeatmapSetExport[] exportSets, CancellationToken cancellationToken)
        => Task.Run(() => ExportBeatmaps(stringProgressReporter, percentageProgressReporter, exportSets, cancellationToken));

    private Dictionary<BeatmapSetExport, Exception> ExportBeatmaps(IProgress<string> stringProgressReporter, IProgress<int> percentageProgressReporter, BeatmapSetExport[] exportSets, CancellationToken cancellationToken)
    {
        Dictionary<BeatmapSetExport, Exception> failedExports = [];

        for (int index = 0; index < exportSets.Length; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            BeatmapSetExport exportSet = exportSets[index];

            stringProgressReporter.Report($"Processing map set {index + 1} of {exportSets.Length}.{Environment.NewLine}\"{exportSet.TargetFileName}\"");

            try
            {
                _BeatmapExporter.ExportBeatmap(exportSet);
            }
            catch (BeatmapSetExportException exception)
            {
                failedExports[exportSet] = exception;
            }

            percentageProgressReporter.Report(Convert.ToInt32((double)(index + 1) / exportSets.Length * 100));
        }

        return failedExports;
    }
}
