namespace CollectionManagerApp;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules.API.osustats;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Misc;
using CollectionManagerApp.Misc.Collection.Strategies;
using CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Models.Forms;
using CollectionManagerApp.Presenters.Controls;
using CollectionManagerApp.Presenters.Forms;
using CollectionManagerApp.Properties;
using System.IO;
using System.Windows.Forms;

public class Initalizer : ApplicationContext
{
    public static OsuFileIo OsuFileIo = new(new BeatmapExtension());
    public static CollectionsManagerWithCounts CollectionsManager;
    public static Beatmaps LoadedBeatmaps => OsuFileIo.LoadedMaps.Beatmaps;
    public static OsuCollections LoadedCollections => CollectionsManager.LoadedCollections;
    public static string OsuDirectory { get; set; }
    public static CollectionEditor CollectionEditor { get; private set; }
    private IUserDialogs UserDialogs { get; set; }// = new GuiComponents.UserDialogs();
    public static OsuStatsApi WebCollectionProvider = new("", OsuFileIo.LoadedMaps);
    public static StartupPresenter StartupPresenter;

    public async Task Run(string[] args)
    {
        //IUserDialogs can be implemented in WinForm or WPF or Gtk or Console or...?
        UserDialogs = GuiComponentsProvider.Instance.GetClassImplementing<IUserDialogs>();

        //Init "main" classes
        CollectionsManager = new CollectionsManagerWithCounts(OsuFileIo.LoadedMaps, new() { { CollectionEdit.ExportBeatmaps, new ExportBeatmapsStrategy(UserDialogs) } });

        ICollectionAddRenameForm collectionAddRemoveForm = GuiComponentsProvider.Instance.GetClassImplementing<ICollectionAddRenameForm>();
        CollectionEditor = new CollectionEditor(CollectionsManager, collectionAddRemoveForm, OsuFileIo.LoadedMaps);

        UpdateChecker updateChecker = new();
        InfoTextModel infoTextModel = new(updateChecker);

        IMainFormView mainForm = GuiComponentsProvider.Instance.GetClassImplementing<IMainFormView>();
        MainFormPresenter mainPresenter = new(mainForm, new MainFormModel(CollectionEditor, UserDialogs), infoTextModel, WebCollectionProvider);

        ILoginFormView loginForm = GuiComponentsProvider.Instance.GetClassImplementing<ILoginFormView>();
        GuiActionsHandler guiActionsHandler = new(OsuFileIo, CollectionsManager, UserDialogs, mainForm, mainPresenter, loginForm);

        if (!string.IsNullOrWhiteSpace(Settings.Default.Osustats_apiKey))
        {
            guiActionsHandler.SidePanelActionsHandler.OsustatsLogin(null, Settings.Default.Osustats_apiKey);
        }

        if (args.Length > 0)
        {
            if (File.Exists(args[0]))
            {
                CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(OsuFileIo.CollectionLoader.LoadCollection(args[0])));
            }
        }

        StartupPresenter = new StartupPresenter(GuiComponentsProvider.Instance.GetClassImplementing<IStartupForm>(), guiActionsHandler.SidePanelActionsHandler, UserDialogs, CollectionsManager, infoTextModel);

        await StartupPresenter.Run();

        SetTextData(infoTextModel);
        mainForm.ShowAndBlock();

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

    public static void Quit()
    {
        Settings.Default.Save();

        if (Application.MessageLoop)
        {
            Application.Exit();
        }
        else
        {
            Environment.Exit(1);
        }
    }
}