namespace CollectionManagerApp;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;
using CollectionManager.Extensions.Utils;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Misc;

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
            {BeatmapListingAction.DownloadBeatmaps, DownloadBeatmaps },
            {BeatmapListingAction.OpenBeatmapPages, OpenBeatmapPages },
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
        if (Initalizer.CollectionEditor != null && model.SelectedBeatmaps?.Count > 0 && model.CurrentCollection != null)
        {
            Beatmaps setBeatmaps = [];

            foreach (Beatmap selectedBeatmap in model.SelectedBeatmaps)
            {
                IEnumerable<Beatmap> set = selectedBeatmap.MapSetId <= 20
                    ? Initalizer.LoadedBeatmaps.Where(b => b.Dir == selectedBeatmap.Dir)
                    : Initalizer.LoadedBeatmaps.Where(b => b.MapSetId == selectedBeatmap.MapSetId);
                setBeatmaps.AddRange(set);

            }

            Initalizer.CollectionEditor.EditCollection(
                CollectionEditArgs.AddBeatmaps(model.CurrentCollection.Name, setBeatmaps)
            );
        }
    }
    private void DownloadBeatmapsManaged(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;
        OsuDownloadManager manager = OsuDownloadManager.Instance;
        if (model.SelectedBeatmaps == null || !model.SelectedBeatmaps.Any())
        {
            _userDialogs.OkMessageBox("Select beatmaps with should be downloaded first, or use Online->Download all missing beatmaps option at the top instead", "Info", MessageBoxType.Info);
            return;
        }

        if (manager.AskUserForSaveDirectoryAndLogin(_userDialogs, _loginForm))
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

    private void DownloadBeatmaps(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;

        HashSet<int> mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
        MassOpen(mapIds, @"https://osu.ppy.sh/d/{0}");
    }

    private void OpenBeatmapPages(object sender)
    {
        IBeatmapListingModel model = (IBeatmapListingModel)sender;

        HashSet<int> mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
        MassOpen(mapIds, @"https://osu.ppy.sh/s/{0}");
    }

    private void MassOpen(HashSet<int> dataSet, string urlFormat)
    {
        bool shouldContinue = true;
        if (dataSet.Count > 100)
        {
            shouldContinue = _userDialogs.YesNoMessageBox("You are going to open " + dataSet.Count +
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