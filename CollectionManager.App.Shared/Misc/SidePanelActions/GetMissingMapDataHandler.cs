namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.API;

public sealed class GetMissingMapDataHandler : IMainSidePanelActionHandler
{
    private BeatmapData BeatmapData;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.GetMissingMapData;

    public Task HandleAsync(object sender, object data)
    {
        //var test = Helpers.GetClipboardText();
        //var p = new TextProcessor();
        //var output = p.ParseLines(test.Split('\n').ToList());
        //foreach (var o in output)
        //{
        //    var collection = new Collection(Initalizer.OsuFileIo.LoadedMaps) { Name = o.Key };
        //    foreach (var mapResult in o.Value)
        //    {
        //        if (mapResult.IdType == TextProcessor.MapIdType.Map)
        //            collection.AddBeatmapByMapId(mapResult.Id);
        //    }
        //    _collectionEditor.EditCollection(
        //        CollectionEditArgs.AddCollections(
        //            new Collections
        //            {
        //                collection
        //            }));
        //}
        //TODO: UI for text parser and map data getter

        BeatmapData ??= new BeatmapData("SNIP", Initalizer.OsuFileIo.LoadedMaps);
        Beatmaps mapsWithMissingData = [];

        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            foreach (Beatmap beatmap in collection.UnknownBeatmaps)
            {
                mapsWithMissingData.Add(beatmap);
            }
        }

        IEnumerable<Beatmap> maps = mapsWithMissingData.Where(m => !string.IsNullOrWhiteSpace(m.Md5)).Distinct();
        List<Beatmap> fetchedBeatmaps = [];
        foreach (Beatmap map in maps)
        {
            Beatmap downloadedBeatmap = null;
            if (map.MapId > 0)
            {
                downloadedBeatmap = BeatmapData.GetBeatmapFromId(map.MapId, PlayMode.Osu);
            }
            else
            if (!map.Md5.Contains('|'))
            {
                downloadedBeatmap = BeatmapData.GetBeatmapFromHash(map.Md5, null);
            }

            if (downloadedBeatmap != null)
            {
                fetchedBeatmaps.Add(downloadedBeatmap);
            }
        }

        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            foreach (Beatmap fetchedBeatmap in fetchedBeatmaps)
            {
                collection.ReplaceBeatmap(fetchedBeatmap.Md5, fetchedBeatmap);
                collection.ReplaceBeatmap(fetchedBeatmap.MapId, fetchedBeatmap);
            }
        }

        return Task.CompletedTask;
    }
}
