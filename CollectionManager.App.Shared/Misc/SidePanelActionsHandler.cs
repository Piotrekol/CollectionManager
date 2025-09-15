namespace CollectionManager.App.Shared.Misc;

using CollectionManager.App.Shared.Interfaces;
using CollectionManager.App.Shared.Misc.SidePanelActions;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo;

public class SidePanelActionsHandler : IDisposable
{
    private readonly OsuFileIo _osuFileIo;
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;
    private readonly IMainFormView _mainForm;

    private readonly IBeatmapListingBindingProvider _beatmapListingBindingProvider;
    private readonly MainFormPresenter _mainFormPresenter;
    private readonly ILoginFormView _loginForm;

    private Dictionary<MainSidePanelActions, IMainSidePanelActionHandler> _mainSidePanelOperationHandlers;
    public SidePanelActionsHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainForm, IBeatmapListingBindingProvider beatmapListingBindingProvider, MainFormPresenter mainFormPresenter, ILoginFormView loginForm, Dictionary<MainSidePanelActions, IMainSidePanelActionHandler> handlers = default)
    {
        _osuFileIo = osuFileIo;
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
        _mainForm = mainForm;
        _beatmapListingBindingProvider = beatmapListingBindingProvider;
        _mainFormPresenter = mainFormPresenter;
        _loginForm = loginForm;

        BindMainFormActions();
    }

    private void BindMainFormActions(Dictionary<MainSidePanelActions, IMainSidePanelActionHandler> handlers = default)
    {
        _mainSidePanelOperationHandlers = CreateDefaultHandlers(_osuFileIo, _collectionEditor, _userDialogs, _mainForm, _beatmapListingBindingProvider, _loginForm);

        if (handlers is not null)
        {
            foreach (KeyValuePair<MainSidePanelActions, IMainSidePanelActionHandler> handler in handlers)
            {
                _mainSidePanelOperationHandlers[handler.Key] = handler.Value;
            }
        }

        _mainForm.SidePanelView.SidePanelOperation += SidePanelViewOnSidePanelOperationEventHandler;
        _mainForm.OnLoadFile += OnLoadFileEventHandler;
        _mainForm.CombinedListingView.CollectionListingView.OnLoadFile += OnLoadFileEventHandler;
        _mainFormPresenter.InfoTextModel.UpdateTextClicked += FormUpdateTextClicked;
    }

    private static Dictionary<MainSidePanelActions, IMainSidePanelActionHandler> CreateDefaultHandlers(
        OsuFileIo osuFileIo,
        ICollectionEditor collectionEditor,
        IUserDialogs userDialogs,
        IMainFormView mainForm,
        IBeatmapListingBindingProvider beatmapListingBindingProvider,
        ILoginFormView loginForm) => new()
    {
        { MainSidePanelActions.LoadCollection, new LoadCollectionHandler(osuFileIo, collectionEditor, userDialogs) },
        { MainSidePanelActions.LoadDefaultCollection, new LoadDefaultCollectionHandler(osuFileIo, collectionEditor, userDialogs) },
        { MainSidePanelActions.ClearCollections, new ClearCollectionsHandler(collectionEditor) },
        { MainSidePanelActions.SaveCollections, new SaveCollectionsHandler(osuFileIo, userDialogs) },
        { MainSidePanelActions.SaveDefaultCollection, new SaveDefaultCollectionHandler(osuFileIo, userDialogs) },
        { MainSidePanelActions.SaveIndividualCollections, new SaveIndividualCollectionsHandler(osuFileIo, userDialogs) },
        { MainSidePanelActions.ShowBeatmapListing, new ShowBeatmapListingHandler(beatmapListingBindingProvider, userDialogs) },
        { MainSidePanelActions.ShowDownloadManager, ShowDownloadManagerHandler.Instance },
        { MainSidePanelActions.DownloadAllMissing, new DownloadAllMissingHandler(userDialogs, loginForm) },
        { MainSidePanelActions.GenerateCollections, new GenerateCollectionsHandler(userDialogs, collectionEditor) },
        { MainSidePanelActions.GetMissingMapData, new GetMissingMapDataHandler() },
        { MainSidePanelActions.ListMissingMaps, new ListMissingMapsHandler(userDialogs) },
        { MainSidePanelActions.ListAllBeatmaps, new ListAllBeatmapsHandler(userDialogs) },
        { MainSidePanelActions.OsustatsLogin, new OsustatsLoginHandler(userDialogs) },
        { MainSidePanelActions.AddCollections, new AddCollectionsHandler() },
        { MainSidePanelActions.UploadCollectionChanges, new UploadCollectionChangesHandler(mainForm, userDialogs) },
        { MainSidePanelActions.UploadNewCollections, new UploadNewCollectionsHandler(osuFileIo, collectionEditor, userDialogs, mainForm) },
        { MainSidePanelActions.RemoveWebCollection, new RemoveWebCollectionHandler(collectionEditor, userDialogs, mainForm) },
        { MainSidePanelActions.ResetApplicationSettings, new ResetApplicationSettingsHandler(userDialogs) },
        { MainSidePanelActions.SyntaxHelp, new SyntaxHelpHandler(userDialogs) },
        { MainSidePanelActions.Discord, new DiscordLinkHandler() },
        { MainSidePanelActions.Github, new GithubLinkHandler() },
    };

    private async void SidePanelViewOnSidePanelOperationEventHandler(object sender, MainSidePanelActions args, object data = null)
    {
        if (_mainSidePanelOperationHandlers.TryGetValue(args, out IMainSidePanelActionHandler handler))
        {
            await handler.HandleAsync(sender, data);
        }
    }

    private async void OnLoadFileEventHandler(object sender, string[] filePaths)
    {
        string[] files = [.. filePaths
            .Select(f => f.ToLowerInvariant())
            .Where(f => f.EndsWith(".osdb") || f.EndsWith(".db") || f.EndsWith(".realm"))];

        if (files.Length is not 0)
        {
            await SidePanelActionHelpers.LoadCollectionsAsync(_osuFileIo, _collectionEditor, _userDialogs, files);
        }
    }

    private void FormUpdateTextClicked(object sender, EventArgs args)
    {
        IUpdateModel updater = _mainFormPresenter.InfoTextModel.GetUpdater();

        if (updater.UpdateIsAvailable && !string.IsNullOrWhiteSpace(updater.NewVersionLink))
        {
            _ = ProcessExtensions.OpenUrl(updater.NewVersionLink);
        }
    }

    public void Dispose()
    {
        foreach (IMainSidePanelActionHandler handler in _mainSidePanelOperationHandlers.Values)
        {
            if (handler is IDisposable disposableHandler)
            {
                disposableHandler.Dispose();
            }
        }

        GC.SuppressFinalize(this);
    }
}