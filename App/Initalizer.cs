using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Interfaces;
using App.Misc;
using App.Models;
using App.Models.Forms;
using App.Presenters.Controls;
using App.Presenters.Forms;
using App.Properties;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.Modules.API.osustats;
using GuiComponents.Interfaces;

namespace App
{
    public class Initalizer : ApplicationContext
    {
        public static OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());
        public static CollectionsManagerWithCounts CollectionsManager;
        public static Beatmaps LoadedBeatmaps => OsuFileIo.LoadedMaps.Beatmaps;
        public static Collections LoadedCollections => CollectionsManager.LoadedCollections;
        public static string OsuDirectory;
        public static CollectionEditor CollectionEditor { get; private set; }
        private IUserDialogs UserDialogs { get; set; }// = new GuiComponents.UserDialogs();
        public static OsuStatsApi WebCollectionProvider = new OsuStatsApi("", OsuFileIo.LoadedMaps);
        public static StartupPresenter StartupPresenter;

        public async Task Run(string[] args)
        {
            //IUserDialogs can be implemented in WinForm or WPF or Gtk or Console or...?
            UserDialogs = GuiComponentsProvider.Instance.GetClassImplementing<IUserDialogs>();



            //Init "main" classes
            CollectionsManager = new CollectionsManagerWithCounts(LoadedBeatmaps);

            var collectionAddRemoveForm = GuiComponentsProvider.Instance.GetClassImplementing<ICollectionAddRenameForm>();
            CollectionEditor = new CollectionEditor(CollectionsManager, CollectionsManager, collectionAddRemoveForm, OsuFileIo.LoadedMaps);

            var updateChecker = new UpdateChecker();
            updateChecker.CheckForUpdates();
            var infoTextModel = new InfoTextModel(updateChecker);

            var mainForm = GuiComponentsProvider.Instance.GetClassImplementing<IMainFormView>();
            var mainPresenter = new MainFormPresenter(mainForm, new MainFormModel(CollectionEditor, UserDialogs), infoTextModel, WebCollectionProvider);

            //set initial text info and update events
            SetTextData(infoTextModel);
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(OsuFileIo.CollectionLoader.LoadCollection(args[0])));
                }
            }

            var loginForm = GuiComponentsProvider.Instance.GetClassImplementing<ILoginFormView>();
            var guiActionsHandler = new GuiActionsHandler(OsuFileIo, CollectionsManager, UserDialogs, mainForm, mainPresenter, loginForm);

            if (!string.IsNullOrWhiteSpace(Settings.Default.Osustats_apiKey))
                guiActionsHandler.SidePanelActionsHandler.OsustatsLogin(null, Settings.Default.Osustats_apiKey);

            StartupPresenter = new StartupPresenter(GuiComponentsProvider.Instance.GetClassImplementing<IStartupForm>(), guiActionsHandler.SidePanelActionsHandler, UserDialogs);
            if (await StartupPresenter.Run())
                mainForm.ShowAndBlock();

            Quit();
        }

        private void SetTextData(IInfoTextModel model)
        {
            model.BeatmapCount = LoadedBeatmaps.Count;
            CollectionsManager.LoadedCollections.CollectionChanged += (s, a) =>
            {
                model.CollectionsCount = CollectionsManager.CollectionsCount;
                model.BeatmapsInCollectionsCount = CollectionsManager.BeatmapsInCollectionsCount;
                model.MissingMapSetsCount = CollectionsManager.MissingMapSetsCount;
                model.UnknownMapCount = CollectionsManager.UnknownMapCount;
            };
            LoadedBeatmaps.CollectionChanged += (s, a) =>
            {
                model.BeatmapCount = LoadedBeatmaps.Count;
            };
        }


        private static void Quit()
        {
            Settings.Default.Save();

            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                System.Environment.Exit(1);
            }
        }
    }
}