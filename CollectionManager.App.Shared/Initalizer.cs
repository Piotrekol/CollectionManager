namespace CollectionManager.App.Shared;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Misc;
using CollectionManager.App.Shared.Misc.Collection.Strategies;
using CollectionManager.App.Shared.Models.Controls;
using CollectionManager.App.Shared.Models.Forms;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.API.osustats;
using System.IO;

public abstract class Initalizer
{
    private static Initalizer _instance;
    public static OsuFileIo OsuFileIo { get; private set; } = new(new BeatmapExtension());
    public static CollectionsManagerWithCounts CollectionsManager { get; private set; }
    public static Beatmaps LoadedBeatmaps => OsuFileIo.LoadedMaps.Beatmaps;
    public static OsuCollections LoadedCollections => CollectionsManager.LoadedCollections;
    public static string OsuDirectory { get; set; }
    public static CollectionEditor CollectionEditor { get; private set; }
    private IUserDialogs UserDialogs { get; set; }
    public static OsuStatsApi WebCollectionProvider { get; private set; } = new("", OsuFileIo.LoadedMaps);
    public static StartupPresenter StartupPresenter { get; private set; }
    public static IAppSettingsProvider Settings { get; private set; }
    public static IClipboard Clipboard { get; private set; }
    public static IGuiComponentsProvider GuiComponentsProvider { get; private set; }
    public static IMainFormView MainForm { get; private set; }
    protected Initalizer(IAppSettingsProvider settings, IClipboard clipboard, IGuiComponentsProvider guiComponentsProvider)
    {
        Settings = settings;
        Clipboard = clipboard;
        GuiComponentsProvider = guiComponentsProvider;
        _instance = this;
    }

    public virtual async Task Run(string[] args)
    {

        //IUserDialogs can be implemented in WinForm or WPF or Gtk or Console or...?
        UserDialogs = GuiComponentsProvider.GetClassImplementing<IUserDialogs>();

        //Init "main" classes
        CollectionsManager = new CollectionsManagerWithCounts(OsuFileIo.LoadedMaps, new() { { CollectionEdit.ExportBeatmaps, new ExportBeatmapsStrategy(UserDialogs) } });

        ICollectionAddRenameForm collectionAddRemoveForm = GuiComponentsProvider.GetClassImplementing<ICollectionAddRenameForm>();
        CollectionEditor = new CollectionEditor(CollectionsManager, collectionAddRemoveForm, OsuFileIo.LoadedMaps);

        UpdateChecker updateChecker = new();
        InfoTextModel infoTextModel = new(updateChecker);

        MainForm = GuiComponentsProvider.GetClassImplementing<IMainFormView>();

        MainFormPresenter mainPresenter = new(MainForm, new MainFormModel(CollectionEditor, UserDialogs), infoTextModel, WebCollectionProvider);

        ILoginFormView loginForm = GuiComponentsProvider.GetClassImplementing<ILoginFormView>();
        GuiActionsHandler guiActionsHandler = new(OsuFileIo, CollectionsManager, UserDialogs, MainForm, mainPresenter, loginForm);

        if (!string.IsNullOrWhiteSpace(Settings.Osustats_apiKey))
        {
            await PopulateOnlineWebCollectionsAsync(apiKey: Settings.Osustats_apiKey);
        }

        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
            {
                CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(OsuFileIo.CollectionLoader.LoadCollection(args[0])));
            }
        }

        StartupPresenter = new StartupPresenter(GuiComponentsProvider.GetClassImplementing<IStartupForm>(), guiActionsHandler.SidePanelActionsHandler, UserDialogs, CollectionsManager, infoTextModel);

        await StartupPresenter.Run();

        SetTextData(infoTextModel);
        MainForm.ShowAndBlock();

        Quit();
    }

    private static void SetTextData(IInfoTextModel model)
    {
        CollectionsManager.LoadedCollections.CollectionChanged += (_, _) => syncModel();
        LoadedBeatmaps.CollectionChanged += (_, _) => syncLoadedBeatmaps();

        syncModel();
        syncLoadedBeatmaps();

        void syncLoadedBeatmaps() => model.BeatmapCount = LoadedBeatmaps.Count;

        void syncModel()
        {
            model.CollectionsCount = CollectionsManager.CollectionsCount;
            model.BeatmapsInCollectionsCount = CollectionsManager.BeatmapsInCollectionsCount;
            model.MissingMapSetsCount = CollectionsManager.MissingMapSetsCount;
            model.UnknownMapCount = CollectionsManager.UnknownMapCount;
        }
    }

    public static async Task PopulateOnlineWebCollectionsAsync(OsuStatsApi osuStatsApiProvider = default, string apiKey = default)
    {
        osuStatsApiProvider ??= WebCollectionProvider;

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            osuStatsApiProvider.ApiKey = apiKey;
        }

        if (!await osuStatsApiProvider.IsCurrentKeyValid() || !osuStatsApiProvider.CanFetch())
        {
            return;
        }

        IOnlineCollectionList onlineListDisplayer = (IOnlineCollectionList)MainForm.SidePanelView;
        Settings.Osustats_apiKey = osuStatsApiProvider.ApiKey;
        onlineListDisplayer.UserInformation = osuStatsApiProvider.UserInformation;
        onlineListDisplayer.WebCollections.Clear();
        onlineListDisplayer.WebCollections.AddRange(await osuStatsApiProvider.GetMyCollectionList());
        onlineListDisplayer.WebCollections.CallReset();
    }

    protected abstract void QuitApplication();

    public static void Quit()
    {
        Settings.Save();
        _instance.QuitApplication();
    }
}
