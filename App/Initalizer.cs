using System;
using System.IO;
using System.Windows.Forms;
using App.Interfaces;
using App.Misc;
using App.Models;
using App.Models.Forms;
using App.Presenters.Forms;
using CollectionManager.DataTypes;
using CollectionManager.Modules;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.DataTypes.MessageBus;
using CollectionManagerExtensionsDll.Modules;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using CollectionManagerExtensionsDll.Utils;
using Common;
using GuiComponents.Interfaces;

namespace App
{
    public class Initalizer : ApplicationContext
    {
        public static OsuFileIo OsuFileIo = new OsuFileIo(new BeatmapExtension());
        public static CollectionsManagerMessageBus CollectionsManager;
        public static Beatmaps LoadedBeatmaps => OsuFileIo.LoadedMaps.Beatmaps;
        public static Collections LoadedCollections => CollectionsManager.LoadedCollections;
        public static string OsuDirectory;
        public static CollectionEditor CollectionEditor { get; private set; }
        private IUserDialogs UserDialogs { get; set; }// = new GuiComponents.UserDialogs();

        public void Run()
        {
            //IUserDialogs can be implemented in WinForm or WPF or Gtk or Console or...?
            UserDialogs = GuiComponentsProvider.Instance.GetClassImplementing<IUserDialogs>();

            //Get osu! directory, or end if it can't be found
            OsuDirectory = OsuFileIo.OsuPathResolver.GetOsuDir(UserDialogs.IsThisPathCorrect, UserDialogs.SelectDirectory);
            if (OsuDirectory == string.Empty)
            {
                UserDialogs.OkMessageBox("Valid osu! directory is required to run Collection Manager" + Environment.NewLine + "Exiting...", "Error", MessageBoxType.Error);
                Quit();
            }

            //Load osu database and setting files
            var osuDbFile = Path.Combine(OsuDirectory, @"osu!.db");
            OsuFileIo.OsuDatabase.Load(osuDbFile);
            OsuFileIo.OsuSettings.Load(OsuDirectory);
            BeatmapUtils.OsuSongsDirectory = OsuFileIo.OsuSettings.CustomBeatmapDirectoryLocation;

            //Init "main" classes
            CollectionsManager = new CollectionsManagerMessageBus(LoadedBeatmaps);

            var collectionAddRemoveForm = GuiComponentsProvider.Instance.GetClassImplementing<ICollectionAddRenameForm>();
            CollectionEditor = new CollectionEditor(CollectionsManager, CollectionsManager, collectionAddRemoveForm, OsuFileIo.LoadedMaps);

            var UpdateChecker = new UpdateChecker();
            UpdateChecker.currentVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();
            var infoTextModel = new InfoTextModel(UpdateChecker);

            var mainForm = GuiComponentsProvider.Instance.GetClassImplementing<IMainFormView>();
            var mainPresenter = new MainFormPresenter(mainForm, new MainFormModel(CollectionEditor, UserDialogs), infoTextModel);

            //set initial text info and update events
            SetTextData(infoTextModel);


            var loginForm = GuiComponentsProvider.Instance.GetClassImplementing<ILoginFormView>();
            new GuiActionsHandler(OsuFileIo, CollectionsManager, UserDialogs, mainForm, mainPresenter, loginForm);

            HandleMainWindowActions(mainForm);

            MessageBus.Register<CollectionEditArgs>(CollectionsManager.EditCollection);
            MessageBus.RegisterFunc<IsCollectionNameValid, IsCollectionNameValid>(CollectionsManager.IsCollectionNameValid);

            new EndlessPlayManager(OsuFileIo.LoadedMaps, OsuFileIo);

            mainForm.ShowAndBlock();
            Quit();
        }

        private void SetTextData(IInfoTextModel model)
        {
            model.SetBeatmapCount(LoadedBeatmaps.Count);
            CollectionsManager.LoadedCollections.CollectionChanged += (s, a) =>
            {
                model.SetCollectionCount(CollectionsManager.CollectionsCount, CollectionsManager.BeatmapsInCollectionsCount);
                model.SetMissingBeatmapCount(CollectionsManager.MissingBeatmapCount);
            };
            LoadedBeatmaps.CollectionChanged += (s, a) =>
            {
                model.SetBeatmapCount(LoadedBeatmaps.Count);
            };
        }
        private void HandleMainWindowActions(IMainFormView form)
        {

            //TODO: export Listing of maps to CollectionTextPresenter(and refactor it as needed)
            form.SidePanelView.ListAllMaps += delegate
            {
                var fileLocation = UserDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
                if (fileLocation == string.Empty) return;
                var listGenerator = new ListGenerator();
                var CollectionListSaveType = Path.GetExtension(fileLocation).ToLower() == ".txt"
                    ? CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Txt
                    : CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Html;
                var contents = listGenerator.GetAllMapsList(LoadedCollections, CollectionListSaveType);
                File.WriteAllText(fileLocation, contents);
            };
            form.SidePanelView.ListMissingMaps += delegate
            {
                var fileLocation = UserDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
                if (fileLocation == string.Empty) return;
                var listGenerator = new ListGenerator();
                var CollectionListSaveType = Path.GetExtension(fileLocation).ToLower() == ".txt"
                    ? CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Txt
                    : CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Html;
                var contents = listGenerator.GetMissingMapsList(LoadedCollections, CollectionListSaveType);
                File.WriteAllText(fileLocation, contents);
            };

        }


        private static void Quit()
        {
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