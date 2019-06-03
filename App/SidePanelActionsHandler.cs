using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using App.Interfaces;
using App.Misc;
using App.Models;
using App.Presenters.Controls;
using App.Presenters.Forms;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Modules.API;
using CollectionManagerExtensionsDll.Modules.API.osu;
using CollectionManagerExtensionsDll.Modules.CollectionApiGenerator;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using Common;
using GuiComponents.Interfaces;

namespace App
{
    public class SidePanelActionsHandler
    {
        private readonly OsuFileIo _osuFileIo;
        private readonly ICollectionEditor _collectionEditor;
        private readonly IUserDialogs _userDialogs;
        private readonly IMainFormView _mainForm;

        IBeatmapListingForm _beatmapListingForm;
        private IUserTopGeneratorForm _userTopGeneratorForm;
        private IUsernameGeneratorForm _usernameGeneratorForm;
        private IDownloadManagerFormView _downloadManagerForm;
        private readonly IBeatmapListingBindingProvider _beatmapListingBindingProvider;
        private readonly MainFormPresenter _mainFormPresenter;
        private readonly ILoginFormView _loginForm;
        private CollectionsApiGenerator _collectionGenerator;
        private OsuSite _osuSite = new OsuSite();
        private BeatmapData BeatmapData = null;

        private Dictionary<MainSidePanelActions, Action<object, object>> _mainSidePanelOperationHandlers;
        public SidePanelActionsHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainForm, IBeatmapListingBindingProvider beatmapListingBindingProvider, MainFormPresenter mainFormPresenter, ILoginFormView loginForm)
        {
            _osuFileIo = osuFileIo;
            _collectionEditor = collectionEditor;
            _userDialogs = userDialogs;
            _mainForm = mainForm;
            _beatmapListingBindingProvider = beatmapListingBindingProvider;
            _mainFormPresenter = mainFormPresenter;
            _loginForm = loginForm;
            _collectionGenerator = new CollectionsApiGenerator(Initalizer.OsuFileIo.LoadedMaps);

            BindMainFormActions();
        }

        private void BindMainFormActions()
        {
            _mainSidePanelOperationHandlers = new Dictionary<MainSidePanelActions, Action<object, object>>
            {
                {MainSidePanelActions.LoadCollection, LoadCollectionFile},
                {MainSidePanelActions.LoadDefaultCollection, LoadDefaultCollection},
                {MainSidePanelActions.ClearCollections, ClearCollections},
                {MainSidePanelActions.SaveCollections, SaveCollections},
                {MainSidePanelActions.SaveInvidualCollections, SaveInvidualCollections},
                {MainSidePanelActions.ShowBeatmapListing, ShowBeatmapListing},
                {MainSidePanelActions.ShowDownloadManager, ShowDownloadManager},
                {MainSidePanelActions.DownloadAllMissing, DownloadAllMissing},
                {MainSidePanelActions.GenerateCollections, GenerateCollections},
                {MainSidePanelActions.GetMissingMapData, GetMissingMapData},
                {MainSidePanelActions.ListMissingMaps, ListMissingMaps },
                {MainSidePanelActions.ListAllBeatmaps, ListAllBeatmaps },
                {MainSidePanelActions.OsustatsLogin, OsustatsLogin },
                {MainSidePanelActions.AddCollections, AddCollections },
                {MainSidePanelActions.UploadCollectionChanges, UploadCollectionChanges },
                {MainSidePanelActions.UploadNewCollections, UploadNewCollections },
                {MainSidePanelActions.RemoveWebCollection ,RemoveWebCollection },
            };

            _mainForm.SidePanelView.SidePanelOperation += SidePanelViewOnSidePanelOperation;
            _mainForm.OnLoadFile += OnLoadFile;
            _mainForm.CombinedListingView.CollectionListingView.OnLoadFile += OnLoadFile;
            _mainFormPresenter.InfoTextModel.UpdateTextClicked += FormUpdateTextClicked;
            _mainForm.Closing += FormOnClosing;
        }

        private void OnLoadFile(object sender, string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var lowercaseFilepath = filePath.ToLowerInvariant();
                if (lowercaseFilepath.EndsWith(".osdb") || lowercaseFilepath.EndsWith(".db"))
                {
                    LoadCollectionFile(sender, filePath);
                }
            }
        }

        private async void RemoveWebCollection(object sender, object data = null)
        {
            var collectionList = (IList<WebCollection>)data;
            var sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

            foreach (var collection in collectionList)
            {
                if (await Initalizer.WebCollectionProvider.RemoveCollection(collection.OnlineId))
                {
                    _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(new[] { collection.Name }));
                    sidePanel.WebCollections.Remove(collection);
                }
                else
                {
                    _userDialogs.OkMessageBox($"Couldn't remove collection {collection.Name}", "Error", MessageBoxType.Error);
                }
            }
        }


        private async void UploadNewCollections(object sender, object data = null)
        {
            var collectionList = (IList<ICollection>)data;

            var oldCollections = new Collections();
            oldCollections.AddRange(collectionList);

            var newCollections = new Collections();
            foreach (var c in collectionList)
            {
                var webCollection = new WebCollection(0, _osuFileIo.LoadedMaps, true);
                webCollection.Name = c.Name;
                webCollection.LastEditorUsername = c.LastEditorUsername;

                foreach (var collectionBeatmap in c.AllBeatmaps())
                {
                    webCollection.AddBeatmap(collectionBeatmap);
                }

                newCollections.AddRange(await webCollection.Save(Initalizer.WebCollectionProvider));
            }

            _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(oldCollections));
            _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(newCollections));

            var sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

            sidePanel.WebCollections.AddRange(newCollections.OfType<WebCollection>());
            sidePanel.WebCollections.CallReset();

            if (newCollections.Count > 0)
            {
                _userDialogs.OkMessageBox($"Collections uploaded", "Info", MessageBoxType.Success);
            }
            if (newCollections.Count == 1)
            {
                Process.Start($"https://osustats.ppy.sh/collection/{newCollections[0].OnlineId}");
            }
        }

        private void SidePanelViewOnSidePanelOperation(object sender, MainSidePanelActions args, object data = null)
        {
            _mainSidePanelOperationHandlers[args](sender, data);
        }

        private async void UploadCollectionChanges(object sender, object data = null)
        {
            var collections = (IList<WebCollection>)data;
            var sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

            foreach (var webCollection in collections)
            {
                if (webCollection.Loaded && webCollection.Modified)
                {
                    var newCollection = (await webCollection.Save(Initalizer.WebCollectionProvider)).First();

                    webCollection.NumberOfBeatmaps = newCollection.OriginalNumberOfBeatmaps;
                    sidePanel.WebCollections.CallReset();

                    _userDialogs.OkMessageBox("Collection was uploaded", "Info", MessageBoxType.Success);

                }
                else
                {
                    _userDialogs.OkMessageBox("This collection is already up to date", "Info");
                }
            }
        }

        private void AddCollections(object sender, object data = null)
        {
            var webCollections = (IList<WebCollection>)data;

            var collections = new Collections();
            foreach (var webCollection in webCollections)
            {
                if (!Initalizer.CollectionsManager.LoadedCollections.Contains(webCollection))
                {
                    collections.Add(webCollection);
                }
            }

            Initalizer.CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(collections));
        }
        private async void OsustatsLogin(object sender, object data = null)
        {
            var onlineListDisplayer = (IOnlineCollectionList)sender;

            var osustatsLoginForm = GuiComponentsProvider.Instance.GetClassImplementing<IOsustatsApiLoginFormView>();

            osustatsLoginForm.ShowAndBlock();

            var provider = Initalizer.WebCollectionProvider;
            provider.ApiKey = osustatsLoginForm.ApiKey;
            if (await provider.IsCurrentKeyValid() && provider.CanFetch())
            {
                onlineListDisplayer.UserInformation = provider.UserInformation;
                onlineListDisplayer.WebCollections.Clear();
                onlineListDisplayer.WebCollections.AddRange(await provider.GetMyCollectionList());
                onlineListDisplayer.WebCollections.CallReset();
            }
        }

        private void ListAllBeatmaps(object sender, object data = null)
        {
            var fileLocation = _userDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
            if (fileLocation == string.Empty) return;
            var listGenerator = new ListGenerator();
            var CollectionListSaveType = Path.GetExtension(fileLocation).ToLower() == ".txt"
                ? CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Txt
                : CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Html;
            var contents = listGenerator.GetAllMapsList(Initalizer.LoadedCollections, CollectionListSaveType);
            File.WriteAllText(fileLocation, contents);
        }
        private void ListMissingMaps(object sender, object data = null)
        {
            var fileLocation = _userDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
            if (fileLocation == string.Empty) return;
            var listGenerator = new ListGenerator();
            var CollectionListSaveType = Path.GetExtension(fileLocation).ToLower() == ".txt"
                ? CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Txt
                : CollectionManagerExtensionsDll.Enums.CollectionListSaveType.Html;
            var contents = listGenerator.GetMissingMapsList(Initalizer.LoadedCollections, CollectionListSaveType);
            File.WriteAllText(fileLocation, contents);
        }

        private void GetMissingMapData(object sender, object data = null)
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

            if (BeatmapData == null)
                BeatmapData = new BeatmapData("SNIP", Initalizer.OsuFileIo.LoadedMaps);
            var mapsWithMissingData = new Beatmaps();


            foreach (var collection in Initalizer.LoadedCollections)
            {
                foreach (var beatmap in collection.UnknownBeatmaps)
                {
                    mapsWithMissingData.Add(beatmap);
                }
            }
            var maps = mapsWithMissingData.Where(m => !string.IsNullOrWhiteSpace(m.Md5)).Distinct();
            List<Beatmap> fetchedBeatmaps = new List<Beatmap>();
            foreach (var map in maps)
            {
                Beatmap downloadedBeatmap = null;
                if (map.MapId > 0)
                    downloadedBeatmap = BeatmapData.GetBeatmapFromId(map.MapId, PlayMode.Osu);
                else
                if (!map.Md5.Contains("|"))
                    downloadedBeatmap = BeatmapData.GetBeatmapFromHash(map.Md5, null);

                if (downloadedBeatmap != null)
                {
                    fetchedBeatmaps.Add(downloadedBeatmap);
                }
            }
            foreach (var collection in Initalizer.LoadedCollections)
            {
                foreach (var fetchedBeatmap in fetchedBeatmaps)
                {
                    //TODO: this is really inefficient
                    collection.ReplaceBeatmap(fetchedBeatmap.Md5, fetchedBeatmap);
                    collection.ReplaceBeatmap(fetchedBeatmap.MapId, fetchedBeatmap);
                }
            }
        }
        private void GenerateCollections(object sender, object data = null)
        {
            if (_userTopGeneratorForm == null || _userTopGeneratorForm.IsDisposed)
            {
                _userTopGeneratorForm = GuiComponentsProvider.Instance.GetClassImplementing<IUserTopGeneratorForm>();
                var model = new UserTopGeneratorModel((a) =>
                    _collectionGenerator.CreateCollectionName(new ApiScore() { EnabledMods = (int)(Mods.Hr | Mods.Hd) },
                    "Piotrekol", a));
                model.GenerateUsernames += GenerateUsernames;
                new UserTopGeneratorFormPresenter(model, _userTopGeneratorForm);
                model.Start += (s, a) => _collectionGenerator.GenerateCollection(model.GeneratorConfiguration);
                model.SaveCollections +=
                    (s, a) => _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(model.Collections));
                model.Abort += (s, a) => _collectionGenerator.Abort();
                _collectionGenerator.StatusUpdated +=
                    (s, a) =>
                    {
                        model.GenerationStatus = _collectionGenerator.Status;
                        model.GenerationCompletionPrecentage = _collectionGenerator.ProcessingCompletionPrecentage;
                    };

                _collectionGenerator.CollectionsUpdated +=
                    (s, a) => model.Collections = _collectionGenerator.Collections;
            }
            _userTopGeneratorForm.Show();
        }


        private void GenerateUsernames(object sender, EventArgs eventArgs)
        {
            if (_usernameGeneratorForm == null || _usernameGeneratorForm.IsDisposed)
            {
                _usernameGeneratorForm = GuiComponentsProvider.Instance.GetClassImplementing<IUsernameGeneratorForm>();
                var model = new UsernameGeneratorModel();
                model.Start +=
                    (s, a) =>
                    {
                        new Thread(() =>
                        {
                            model.GeneratedUsernames = _osuSite.GetUsernames(model.StartRank, model.EndRank,
                           (string logMessage, int completionPrecentage) =>
                           {
                               model.Status = logMessage;
                               model.CompletionPrecentage = completionPrecentage;
                           });
                            model.EmitComplete();
                        }).Start();
                    };
                model.Complete += (s, a) => Helpers.SetClipboardText(model.GeneratedUsernamesStr);
                new UsernameGeneratorPresenter(model, _usernameGeneratorForm.view);
            }
            _usernameGeneratorForm.ShowAndBlock();
        }

        private void DownloadAllMissing(object sender, object data = null)
        {
            var downloadableBeatmaps = new Beatmaps();
            foreach (var collection in Initalizer.LoadedCollections)
            {
                foreach (var beatmap in collection.DownloadableBeatmaps)
                {
                    downloadableBeatmaps.Add(beatmap);
                }
            }

            if (downloadableBeatmaps.Count > 0)
                if (OsuDownloadManager.Instance.AskUserForSaveDirectoryAndLogin(_userDialogs, _loginForm))
                {
                    OsuDownloadManager.Instance.DownloadBeatmaps(downloadableBeatmaps);
                    ShowDownloadManager();
                }
        }

        private void FormUpdateTextClicked(object sender, EventArgs args)
        {
            var updater = _mainFormPresenter.InfoTextModel.GetUpdater();
            if (updater.IsUpdateAvaliable())
            {
                if (!string.IsNullOrWhiteSpace(updater.newVersionLink))
                    Process.Start(updater.newVersionLink);
            }

        }

        private void FormOnClosing(object sender, EventArgs eventArgs)
        {
            if (_beatmapListingForm != null && !_beatmapListingForm.IsDisposed)
            {
                _beatmapListingForm.Close();
            }
            if (_downloadManagerForm != null && !_downloadManagerForm.IsDisposed)
            {
                _downloadManagerForm.Close();
            }
        }
        private void LoadCollectionFile(object sender, object data = null)
        {
            string fileLocation = data?.ToString() ?? _userDialogs.SelectFile("", "Collection database (*.db/*.osdb)|*.db;*.osdb",
                    "collection.db");
            if (string.IsNullOrEmpty(fileLocation)) return;
            var loadedCollections = _osuFileIo.CollectionLoader.LoadCollection(fileLocation);
            _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(loadedCollections));
        }

        private void LoadDefaultCollection(object sender, object data = null)
        {
            var fileLocation = Path.Combine(Initalizer.OsuDirectory, "collection.db");
            if (File.Exists(fileLocation))
            {
                var loadedCollections = _osuFileIo.CollectionLoader.LoadCollection(fileLocation);
                _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(loadedCollections));
            }
        }

        private void ClearCollections(object sender, object data = null)
        {
            _collectionEditor.EditCollection(CollectionEditArgs.ClearCollections());
        }

        private void SaveCollections(object sender, object data = null)
        {
            var fileLocation = _userDialogs.SaveFile("Where collection file should be saved?", "osu! Collection database (.db)|*.db|CM database (.osdb)|*.osdb");
            if (fileLocation == string.Empty) return;
            _osuFileIo.CollectionLoader.SaveCollection(Initalizer.LoadedCollections, fileLocation);
        }

        private void SaveInvidualCollections(object sender, object data = null)
        {
            var saveDirectory = _userDialogs.SelectDirectory("Where collection files should be saved?", true);
            if (saveDirectory == string.Empty) return;
            foreach (var collection in Initalizer.LoadedCollections)
            {
                var filename = Helpers.StripInvalidCharacters(collection.Name);
                _osuFileIo.CollectionLoader.SaveCollection(new Collections() { collection }, saveDirectory + filename);
            }
        }

        private void ShowBeatmapListing(object sender, object data = null)
        {
            if (_beatmapListingForm == null || _beatmapListingForm.IsDisposed)
            {
                _beatmapListingForm = GuiComponentsProvider.Instance.GetClassImplementing<IBeatmapListingForm>();
                var presenter = new BeatmapListingFormPresenter(_beatmapListingForm);
                _beatmapListingBindingProvider.Bind(presenter.BeatmapListingModel);
                _beatmapListingForm.Closing += (s, a) => _beatmapListingBindingProvider.UnBind(presenter.BeatmapListingModel);
            }
            _beatmapListingForm.Show();
        }

        private void CreateDownloadManagerForm()
        {
            if (_downloadManagerForm == null || _downloadManagerForm.IsDisposed)
            {
                _downloadManagerForm = GuiComponentsProvider.Instance.GetClassImplementing<IDownloadManagerFormView>();
                new DownloadManagerFormPresenter(_downloadManagerForm, new DownloadManagerModel(OsuDownloadManager.Instance));
            }
        }
        private void ShowDownloadManager(object sender = null, object data = null)
        {
            CreateDownloadManagerForm();
            _downloadManagerForm.Show();
        }
    }
}