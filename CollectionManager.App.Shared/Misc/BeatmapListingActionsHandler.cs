namespace CollectionManager.App.Shared.Misc;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;
using CollectionManager.Extensions.Utils;
using System.Linq;
using System.Threading.Tasks;

public class BeatmapListingActionsHandler
{
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;
    private readonly ILoginFormView _loginForm;
    private readonly OsuFileIo _osuFileIo;
    private readonly ListGenerator _listGenerator = new();
    private readonly Dictionary<BeatmapListingAction, Action<object>> _beatmapOperationHandlers;
    private readonly UserListGenerator UserUrlListGenerator = new() { collectionBodyFormat = "{MapLink}" + UserListGenerator.NewLine };
    public BeatmapListingActionsHandler(ICollectionEditor collectionEditor, IUserDialogs userDialogs, ILoginFormView loginForm, OsuFileIo osuFileIo)
    {
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
        _loginForm = loginForm;
        _osuFileIo = osuFileIo;

        _beatmapOperationHandlers = new Dictionary<BeatmapListingAction, Action<object>>
        {
            {BeatmapListingAction.CopyBeatmapsAsText, CopyBeatmapsAsText },
            {BeatmapListingAction.CopyBeatmapsAsUrls, CopyBeatmapsAsUrls },
            {BeatmapListingAction.DeleteBeatmapsFromCollection, DeleteBeatmapsFromCollection },
            {BeatmapListingAction.DownloadBeatmapsManaged, DownloadBeatmapsManaged },
            {BeatmapListingAction.DownloadBeatmaps, DownloadBeatmapsAsync },
            {BeatmapListingAction.OpenBeatmapPages, OpenBeatmapPagesAsync },
            {BeatmapListingAction.OpenBeatmapFolder, OpenBeatmapFolder },
            {BeatmapListingAction.PullWholeMapSet, PullWholeMapsets },
            {BeatmapListingAction.ExportBeatmapSets, ExportBeatmapSets },
        };
    }
    public void Bind(IBeatmapListingModel beatmapListingModel) => beatmapListingModel.BeatmapOperation += BeatmapListingModel_BeatmapOperation;

    public void UnBind(IBeatmapListingModel beatmapListingModel) => beatmapListingModel.BeatmapOperation -= BeatmapListingModel_BeatmapOperation;

    private void BeatmapListingModel_BeatmapOperation(object sender, BeatmapListingAction args) => _beatmapOperationHandlers[args](sender);

    private void PullWholeMapsets(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;

        if (model.SelectedBeatmaps?.Count > 0)
        {
            HashSet<int> targetMapSetIds = [];
            HashSet<string> targetDirs = [];

            foreach (Beatmap selectedBeatmap in model.SelectedBeatmaps)
            {
                if (selectedBeatmap.MapSetId <= MapCacher.InvalidMapIdThreshold)
                {
                    _ = targetDirs.Add(selectedBeatmap.Dir);
                }
                else
                {
                    _ = targetMapSetIds.Add(selectedBeatmap.MapSetId);
                }
            }

            Dictionary<int, HashSet<Beatmap>> mapsetBeatmaps = [];
            Dictionary<string, HashSet<Beatmap>> unsubmittedBeatmaps = [];

            foreach (Beatmap beatmap in Initalizer.LoadedBeatmaps)
            {
                if (beatmap.MapSetId > 20 && targetMapSetIds.Contains(beatmap.MapSetId))
                {
                    if (!mapsetBeatmaps.TryGetValue(beatmap.MapSetId, out HashSet<Beatmap> value))
                    {
                        value = [];
                        mapsetBeatmaps[beatmap.MapSetId] = value;
                    }

                    _ = value.Add(beatmap);
                }
                else if (beatmap.MapSetId <= MapCacher.InvalidMapIdThreshold && targetDirs.Contains(beatmap.Dir))
                {
                    if (!unsubmittedBeatmaps.TryGetValue(beatmap.Dir, out HashSet<Beatmap> value))
                    {
                        value = [];
                        unsubmittedBeatmaps[beatmap.Dir] = value;
                    }

                    _ = value.Add(beatmap);
                }
            }

            List<CollectionEditArgs> bulkEditArgs = new(mapsetBeatmaps.Count + unsubmittedBeatmaps.Count);

            bulkEditArgs.AddRange(mapsetBeatmaps.Values
                .Select(beatmaps => CollectionEditArgs.AddBeatmaps(model.CurrentCollection.Name, beatmaps)));
            bulkEditArgs.AddRange(unsubmittedBeatmaps.Values
                .Select(beatmaps => CollectionEditArgs.AddBeatmaps(model.CurrentCollection.Name, beatmaps)));

            Initalizer.CollectionEditor.EditCollection(bulkEditArgs);
        }
    }
    private async void DownloadBeatmapsManaged(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;
        OsuDownloadManager manager = OsuDownloadManager.Instance;
        if (model.SelectedBeatmaps == null || !model.SelectedBeatmaps.Any())
        {
            await _userDialogs.OkMessageBoxAsync("Select beatmaps with should be downloaded first, or use Online->Download all missing beatmaps option at the top instead", "Info", MessageBoxType.Info);
            return;
        }

        if (await manager.AskUserForSaveDirectoryAndLoginAsync(_userDialogs, _loginForm))
        {
            OsuDownloadManager.Instance.DownloadBeatmaps(model.SelectedBeatmaps);
        }
    }

    private void ExportBeatmapSets(object sender)
    {
        if (sender is not IBeatmapListingModel beatmapListingModel)
        {
            return;
        }

        _collectionEditor.EditCollection(CollectionExportEditArgs.ExportBeatmaps(beatmapListingModel.SelectedBeatmaps));
    }

    private void OpenBeatmapFolder(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;
        if (model.SelectedBeatmap != null)
        {
            string location = ((BeatmapExtension)model.SelectedBeatmap).FullOsuFileLocation();
            _ = Process.Start("explorer.exe", $"/select, \"{location}\"");
        }
    }
    private void DeleteBeatmapsFromCollection(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;
        _collectionEditor.EditCollection(CollectionEditArgs.RemoveBeatmaps(model.CurrentCollection.Name, model.SelectedBeatmaps));
    }

    private void CopyBeatmapsAsUrls(object sender)
    {
        OsuCollection dummyCollection = ((IBeatmapListingModel)sender).AddSelectedBeatmapsToCollection(new OsuCollection(_osuFileIo.LoadedMaps));
        Helpers.SetClipboardText(_listGenerator.GetAllMapsList([dummyCollection], UserUrlListGenerator));
    }

    private void CopyBeatmapsAsText(object sender)
    {
        OsuCollection dummyCollection = ((IBeatmapListingModel)sender).AddSelectedBeatmapsToCollection(new OsuCollection(_osuFileIo.LoadedMaps));
        Helpers.SetClipboardText(_listGenerator.GetAllMapsList([dummyCollection], CollectionListSaveType.BeatmapList));
    }

    private async void DownloadBeatmapsAsync(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;

        HashSet<int> mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
        await MassOpenAsync(mapIds, @"https://osu.ppy.sh/d/{0}");
    }

    private async void OpenBeatmapPagesAsync(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;

        HashSet<int> mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
        await MassOpenAsync(mapIds, @"https://osu.ppy.sh/s/{0}");
    }

    private async Task MassOpenAsync(HashSet<int> dataSet, string urlFormat)
    {
        bool shouldContinue = true;
        if (dataSet.Count > 100)
        {
            shouldContinue = await _userDialogs.YesNoMessageBoxAsync("You are going to open " + dataSet.Count +
                                         " map links at the same time in your default browser", "Are you sure?", MessageBoxType.Question);
        }

        if (shouldContinue)
        {
            foreach (int d in dataSet)
            {
                _ = ProcessExtensions.OpenUrl(string.Format(urlFormat, d));
            }
        }
    }
}