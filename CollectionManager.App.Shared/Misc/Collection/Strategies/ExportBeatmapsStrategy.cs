namespace CollectionManager.App.Shared.Misc.Collection.Strategies;

using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.Exporter;
using CollectionManager.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ExportBeatmapsStrategy : ICollectionEditStrategy
{
    private readonly IUserDialogs _userDialogs;

    public ExportBeatmapsStrategy(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
    }
    public void Execute(CollectionsManager manager, CollectionEditArgs args)
    {
        if (args.Beatmaps.Any())
        {
            _ = ExportBeatmapSetsAsync(args.Beatmaps);
            return;
        }

        IEnumerable<Beatmap> beatmaps = args.CollectionNames
            .Select(manager.GetCollectionByName)
            .SelectMany(c => c.AllBeatmaps());

        _ = ExportBeatmapSetsAsync(beatmaps);
    }

    private async Task ExportBeatmapSetsAsync(IEnumerable<Beatmap> beatmaps)
    {
        if (!beatmaps.Any())
        {
            _userDialogs.OkMessageBox("No beatmaps selected.", "Info");

            return;
        }

        string saveDirectory = _userDialogs.SelectDirectory("Select directory for exported maps", true);

        if (!Directory.Exists(saveDirectory))
        {
            return;
        }

        BeatmapExporter beatmapExporter = new(BeatmapUtils.OsuSongsDirectory, saveDirectory);
        BeatmapExportFormPresenter exportPresenter = new(_userDialogs, beatmapExporter);

        try
        {
            await exportPresenter.ExportAsync([.. beatmaps], saveDirectory);
        }
        catch (Exception ex)
        {
            _userDialogs.OkMessageBox($"Error occurred during beatmap export: {ex}", "Error", MessageBoxType.Error);
        }
    }
}
