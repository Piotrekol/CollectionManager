namespace CollectionManagerApp;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Extensions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.API;
using CollectionManager.Extensions.Modules.API.osu;
using CollectionManager.Extensions.Modules.API.osustats;
using CollectionManager.Extensions.Modules.CollectionApiGenerator;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using CollectionManagerApp.Interfaces;
using CollectionManagerApp.Misc;
using CollectionManagerApp.Models.Controls;
using CollectionManagerApp.Presenters.Controls;
using CollectionManagerApp.Presenters.Forms;
using CollectionManagerApp.Properties;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

public class SidePanelActionsHandler : IDisposable
{
    private readonly OsuFileIo _osuFileIo;
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;
    private readonly IMainFormView _mainForm;

    private IBeatmapListingForm _beatmapListingForm;
    private IUserTopGeneratorForm _userTopGeneratorForm;
    private IUsernameGeneratorForm _usernameGeneratorForm;
    private IDownloadManagerFormView _downloadManagerForm;
    private readonly IBeatmapListingBindingProvider _beatmapListingBindingProvider;
    private readonly MainFormPresenter _mainFormPresenter;
    private readonly ILoginFormView _loginForm;
    private readonly CollectionsApiGenerator _collectionGenerator;
    private readonly OsuSite _osuSite = new();
    private BeatmapData BeatmapData;

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
            {MainSidePanelActions.SaveDefaultCollection, SaveDefaultCollection},
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
            {MainSidePanelActions.ResetApplicationSettings, ResetApplicationSettings },
            {MainSidePanelActions.SyntaxHelp, SyntaxHelp },
            {MainSidePanelActions.Discord, DiscordLink },
            {MainSidePanelActions.Github, GithubLink }
        };

        _mainForm.SidePanelView.SidePanelOperation += SidePanelViewOnSidePanelOperation;
        _mainForm.OnLoadFile += OnLoadFile;
        _mainForm.CombinedListingView.CollectionListingView.OnLoadFile += OnLoadFile;
        _mainFormPresenter.InfoTextModel.UpdateTextClicked += FormUpdateTextClicked;
        _mainForm.Closing += FormOnClosing;
    }

    private void GithubLink(object sender, object data) => ProcessExtensions.OpenUrl("https://github.com/Piotrekol/CollectionManager");

    private void DiscordLink(object sender, object data) => ProcessExtensions.OpenUrl("https://osustats.ppy.sh/discord");

    private void SyntaxHelp(object sender, object data) => ResourceStrings.GeneralHelpDialog(_userDialogs);

    public void LoadOsuCollection() => LoadDefaultCollection(null, null);

    public void LoadCollectionFile() => LoadCollectionFile(null);

    private void ResetApplicationSettings(object arg1, object arg2)
    {
        if (_userDialogs.YesNoMessageBox(
            "Are you sure that you want to reset all Collection Manager settings?", "Settings reset",
            MessageBoxType.Question))
        {
            Settings.Default.Reset();
            Settings.Default.Save();
            _userDialogs.OkMessageBox("Settings were set to their defaults", "Settings reset");
        }
        else
        {
            _userDialogs.OkMessageBox("Settings were not reset", "Settings reset");
        }
    }

    private void OnLoadFile(object sender, string[] filePaths)
    {
        string[] files = filePaths.Select(f => f.ToLowerInvariant())
                    .Where(f => f.EndsWith(".osdb") || f.EndsWith(".db"))
                    .ToArray()
                    ;

        if (files.Length != 0)
        {
            LoadCollections(files);
        }
    }

    private async void RemoveWebCollection(object sender, object data = null)
    {
        IList<WebCollection> collectionList = (IList<WebCollection>)data;
        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        foreach (WebCollection collection in collectionList)
        {
            if (await Initalizer.WebCollectionProvider.RemoveCollection(collection.OnlineId))
            {
                _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(new[] { collection.Name }));
                _ = sidePanel.WebCollections.Remove(collection);
            }
            else
            {
                _userDialogs.OkMessageBox($"Couldn't remove collection {collection.Name}", "Error", MessageBoxType.Error);
            }
        }
    }

    private async void UploadNewCollections(object sender, object data = null)
    {
        if (!await Initalizer.WebCollectionProvider.IsCurrentKeyValid())
        {
            _userDialogs.OkMessageBox("You need to login before uploading collections", "Error", MessageBoxType.Error);
            return;
        }

        IList<IOsuCollection> collectionList = (IList<IOsuCollection>)data;

        foreach (IOsuCollection c in collectionList)
        {
            if (!c.AllBeatmaps().Any())
            {
                _userDialogs.OkMessageBox("Empty collection - upload aborted", "Error", MessageBoxType.Error);
                return;
            }
        }

        OsuCollections oldCollections = [.. collectionList];

        OsuCollections newCollections = [];
        foreach (IOsuCollection c in collectionList)
        {
            WebCollection webCollection = new(0, _osuFileIo.LoadedMaps, true)
            {
                Name = c.Name,
                LastEditorUsername = c.LastEditorUsername
            };

            foreach (BeatmapExtension collectionBeatmap in c.AllBeatmaps())
            {
                webCollection.AddBeatmap(collectionBeatmap);
            }

            newCollections.AddRange(await webCollection.Save(Initalizer.WebCollectionProvider));
        }

        _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(oldCollections.Names));
        _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(newCollections));

        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        sidePanel.WebCollections.AddRange(newCollections.OfType<WebCollection>());
        sidePanel.WebCollections.CallReset();

        if (newCollections.Count > 0)
        {
            _userDialogs.OkMessageBox($"Collections uploaded", "Info", MessageBoxType.Success);
        }

        if (newCollections.Count == 1)
        {
            _ = ProcessExtensions.OpenUrl($"https://osustats.ppy.sh/collection/{newCollections[0].OnlineId}");
        }
    }

    private void SidePanelViewOnSidePanelOperation(object sender, MainSidePanelActions args, object data = null) => _mainSidePanelOperationHandlers[args](sender, data);

    private async void UploadCollectionChanges(object sender, object data = null)
    {
        IList<WebCollection> collections = (IList<WebCollection>)data;
        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        foreach (WebCollection webCollection in collections)
        {
            if (webCollection.Loaded && webCollection.Modified)
            {
                WebCollection newCollection = (await webCollection.Save(Initalizer.WebCollectionProvider)).First();

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
        IList<WebCollection> webCollections = (IList<WebCollection>)data;

        OsuCollections collections = [];
        foreach (WebCollection webCollection in webCollections)
        {
            if (!Initalizer.CollectionsManager.LoadedCollections.Contains(webCollection))
            {
                collections.Add(webCollection);
            }
        }

        Initalizer.CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(collections));
    }
    public async void OsustatsLogin(object sender, object data = null)
    {
        sender ??= _mainForm.SidePanelView;

        IOnlineCollectionList onlineListDisplayer = (IOnlineCollectionList)sender;
        OsuStatsApi provider = Initalizer.WebCollectionProvider;

        if (data == null)
        {
            IOsustatsApiLoginFormView osustatsLoginForm = GuiComponentsProvider.Instance.GetClassImplementing<IOsustatsApiLoginFormView>();

            osustatsLoginForm.ShowAndBlock();

            provider.ApiKey = osustatsLoginForm.ApiKey;
        }
        else
        {
            provider.ApiKey = (string)data;
        }

        if (await provider.IsCurrentKeyValid() && provider.CanFetch())
        {
            Settings.Default.Osustats_apiKey = provider.ApiKey;
            onlineListDisplayer.UserInformation = provider.UserInformation;
            onlineListDisplayer.WebCollections.Clear();
            onlineListDisplayer.WebCollections.AddRange(await provider.GetMyCollectionList());
            onlineListDisplayer.WebCollections.CallReset();
        }
    }

    private void ListAllBeatmaps(object sender, object data = null)
    {
        string fileLocation = _userDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
        if (fileLocation == string.Empty)
        {
            return;
        }

        ListGenerator listGenerator = new();
        CollectionListSaveType CollectionListSaveType = Path.GetExtension(fileLocation).Equals(".txt"
, StringComparison.OrdinalIgnoreCase)
            ? CollectionListSaveType.Txt
            : CollectionListSaveType.Html;
        string contents = listGenerator.GetAllMapsList(Initalizer.LoadedCollections, CollectionListSaveType);
        File.WriteAllText(fileLocation, contents);
    }
    private void ListMissingMaps(object sender, object data = null)
    {
        string fileLocation = _userDialogs.SaveFile("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
        if (fileLocation == string.Empty)
        {
            return;
        }

        ListGenerator listGenerator = new();
        CollectionListSaveType CollectionListSaveType = Path.GetExtension(fileLocation).Equals(".txt"
, StringComparison.OrdinalIgnoreCase)
            ? CollectionListSaveType.Txt
            : CollectionListSaveType.Html;
        string contents = listGenerator.GetMissingMapsList(Initalizer.LoadedCollections, CollectionListSaveType);
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
            UserTopGeneratorModel model = new((a) =>
                CollectionsApiGenerator.CreateCollectionName(new ApiScore() { EnabledMods = (int)(Mods.Hr | Mods.Hd) },
                "Piotrekol", a));
            model.GenerateUsernames += GenerateUsernames;
            _ = new UserTopGeneratorFormPresenter(model, _userTopGeneratorForm);
            model.Start += (s, a) => _collectionGenerator.GenerateCollection(model.GeneratorConfiguration);
            model.SaveCollections +=
                (s, a) => _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(model.Collections));
            model.Abort += async (s, a) => await _collectionGenerator.AbortAsync();
            _collectionGenerator.StatusUpdated +=
                (s, a) =>
                {
                    model.GenerationStatus = _collectionGenerator.Status;
                    model.GenerationCompletionPrecentage = _collectionGenerator.ProcessingCompletionPercentage;
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
            UsernameGeneratorModel model = new();
            model.Start +=
                (s, a) => new Thread(async () =>
                    {
                        model.GeneratedUsernames = await _osuSite.GetUsernamesAsync(model.StartRank, model.EndRank,
                       (string logMessage, int completionPrecentage) =>
                       {
                           model.Status = logMessage;
                           model.CompletionPrecentage = completionPrecentage;
                       }, CancellationToken.None);
                        model.EmitComplete();
                    }).Start();
            model.Complete += (s, a) => Helpers.SetClipboardText(model.GeneratedUsernamesStr);
            _ = new UsernameGeneratorPresenter(model, _usernameGeneratorForm.view);
        }

        _usernameGeneratorForm.ShowAndBlock();
    }

    private void DownloadAllMissing(object sender, object data = null)
    {
        Beatmaps downloadableBeatmaps = [];
        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            foreach (Beatmap beatmap in collection.DownloadableBeatmaps)
            {
                downloadableBeatmaps.Add(beatmap);
            }
        }

        if (downloadableBeatmaps.Count == 0)
        {
            _userDialogs.OkMessageBox("You don't have any missing maps that CM is able to download", "Info", MessageBoxType.Info);
            return;
        }

        if (OsuDownloadManager.Instance.AskUserForSaveDirectoryAndLogin(_userDialogs, _loginForm))
        {
            OsuDownloadManager.Instance.DownloadBeatmaps(downloadableBeatmaps);
            ShowDownloadManager();
        }
    }

    private void FormUpdateTextClicked(object sender, EventArgs args)
    {
        IUpdateModel updater = _mainFormPresenter.InfoTextModel.GetUpdater();
        if (updater.UpdateIsAvailable)
        {
            if (!string.IsNullOrWhiteSpace(updater.NewVersionLink))
            {
                _ = ProcessExtensions.OpenUrl(updater.NewVersionLink);
            }
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
        => LoadCollections(data?.ToString() ?? _userDialogs.SelectFile("", "Collection database (*.db/*.osdb/*.realm)|*.db;*.osdb;*.realm", "collection.db"));

    private void LoadDefaultCollection(object sender, object data = null)
        => LoadCollections(Path.Combine(Initalizer.OsuDirectory, "collection.db"), Path.Combine(Initalizer.OsuDirectory, "client.realm"));

    private void LoadCollections(params string[] fileLocations)
    {
        if (fileLocations == null || fileLocations.Length == 0 || fileLocations.Any(string.IsNullOrWhiteSpace))
        {
            return;
        }

        OsuCollections collections = [];

        foreach (string fileLocation in fileLocations.Where(File.Exists))
        {
            try
            {
                collections.AddRange(_osuFileIo.CollectionLoader.LoadCollection(fileLocation));
            }
            catch (CorruptedFileException ex)
            {
                _userDialogs.OkMessageBox(ex.Message, "Error", MessageBoxType.Error);
            }
        }

        _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(collections));

        GC.Collect();
    }

    private async void SaveDefaultCollection(object sender, object data = null)
    {
        bool isLegacyCollectionFile = true;
        string fileLocation = Path.Combine(Initalizer.OsuDirectory, "collection.db");

        if (!File.Exists(fileLocation))
        {
            fileLocation = Path.Combine(Initalizer.OsuDirectory, "client.realm");
            isLegacyCollectionFile = false;

            if (!File.Exists(fileLocation))
            {
                _userDialogs.OkMessageBox("Could not find collection file to overwritte!", "Error", MessageBoxType.Error);
                return;
            }
        }

        try
        {
            if (OsuIsRunning(isLegacyCollectionFile))
            {
                _userDialogs.OkMessageBox("Close your osu! before saving collections!", "Error", MessageBoxType.Error);
                return;
            }
        }
        catch (Win32Exception ex)
        {
            // access denied
            if (ex.NativeErrorCode != 5)
            {
                throw;
            }

            _userDialogs.OkMessageBox("Could not determine if osu! is running due to a permissions error.", "Warning", MessageBoxType.Warning);
        }

        if (_userDialogs.YesNoMessageBox($"Are you sure that you want to overwrite your existing osu! collection at \"{fileLocation}\"?",
            "Are you sure?", MessageBoxType.Question))
        {
            await BeforeCollectionSave(Initalizer.LoadedCollections);
            string backupFolder = Path.Combine(Initalizer.OsuDirectory, "collectionBackups");

            if (!TryBackupOsuCollection(backupFolder))
            {
                _userDialogs.OkMessageBox("Could not create collection backup. Save aborted.", "Error", MessageBoxType.Error);
                return;
            }

            _osuFileIo.CollectionLoader.SaveCollection(Initalizer.LoadedCollections, fileLocation);
            _userDialogs.OkMessageBox($"Collections saved.{Environment.NewLine}Previous collection backup was saved in \"{backupFolder}\" and will be kept for 30 days.", "Info", MessageBoxType.Success);
        }
        else
        {
            _userDialogs.OkMessageBox("Save Aborted", "Info", MessageBoxType.Warning);
        }

        static bool OsuIsRunning(bool isLegacyOsu)
        {
            IEnumerable<Process> osuProcesses = Process.GetProcessesByName("osu!")
                .Where(process => process.MainModule is not null);

            return isLegacyOsu
                ? osuProcesses.Any(process => (Path.GetDirectoryName(process.MainModule.FileName)?.ToLowerInvariant()).Equals(Initalizer.OsuDirectory, StringComparison.OrdinalIgnoreCase))
                : osuProcesses.Any(process => process.MainModule.ModuleName == "osu!.exe");
        }
    }

    private static bool TryBackupOsuCollection(string backupFolder)
    {
        if (!Directory.Exists(backupFolder))
        {
            _ = Directory.CreateDirectory(backupFolder);
        }

        string sourceCollectionFile = Path.Combine(Initalizer.OsuDirectory, "collection.db");
        string destinationCollectionFile;
        if (File.Exists(sourceCollectionFile))
        {
            destinationCollectionFile = Path.Combine(backupFolder, $"collection_{CalculateMD5(sourceCollectionFile)}.db");
        }
        else
        {
            sourceCollectionFile = Path.Combine(Initalizer.OsuDirectory, "client.realm");

            if (!File.Exists(sourceCollectionFile))
            {
                return false;
            }

            destinationCollectionFile = Path.Combine(backupFolder, $"client_{CalculateMD5(sourceCollectionFile)}.realm");
        }

        if (File.Exists(destinationCollectionFile))
        {
            // Update file save date to indicate latest collection version
            File.SetLastWriteTime(destinationCollectionFile, DateTime.Now);
            return true;
        }

        CleanupBackups("*.db");
        CleanupBackups("*.realm");
        File.Copy(sourceCollectionFile, destinationCollectionFile);

        return true;

        string CalculateMD5(string filename)
        {
            using MD5 md5 = MD5.Create();
            using FileStream stream = File.OpenRead(filename);
            byte[] hash = md5.ComputeHash(stream);
            return Convert.ToHexStringLower(hash);
        }

        void CleanupBackups(string searchPattern)
        {
            DateTime deleteDateThreshold = DateTime.UtcNow.AddDays(-30);
            string[] collectionFilePaths = Directory.GetFiles(backupFolder, searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<FileInfo> collectionFiles = collectionFilePaths.Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTimeUtc < deleteDateThreshold);

            foreach (FileInfo collectionFile in collectionFiles)
            {
                collectionFile.Delete();
            }
        }
    }

    private void ClearCollections(object sender, object data = null) => _collectionEditor.EditCollection(CollectionEditArgs.ClearCollections());

    private static Task BeforeCollectionSave(IList<IOsuCollection> collections)
    {
        List<Task> tasks = [];

        foreach (IOsuCollection collection in collections)
        {
            if (collection is WebCollection wc)
            {
                tasks.Add(wc.Load(Initalizer.WebCollectionProvider));
            }
        }

        return Task.WhenAll(tasks);
    }
    private async void SaveCollections(object sender, object data = null)
    {
        string fileLocation = _userDialogs.SaveFile("Where collection file should be saved?", "osu! Collection database (.db)|*.db|CM database (.osdb)|*.osdb");
        if (fileLocation == string.Empty)
        {
            return;
        }

        await BeforeCollectionSave(Initalizer.LoadedCollections);
        _osuFileIo.CollectionLoader.SaveCollection(Initalizer.LoadedCollections, fileLocation);
    }

    private async void SaveInvidualCollections(object sender, object data = null)
    {
        string saveDirectory = _userDialogs.SelectDirectory("Where collection files should be saved?", true);
        if (saveDirectory == string.Empty)
        {
            return;
        }

        string fileFormat = _userDialogs.YesNoMessageBox("Save collections in osdb format?", "Collection save format", MessageBoxType.Question)
            ? "osdb"
            : "db";

        await BeforeCollectionSave(Initalizer.LoadedCollections);
        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            string filename = $"{collection.Name.StripInvalidFileNameCharacters("_")}.{fileFormat}";
            _osuFileIo.CollectionLoader.SaveCollection([collection], Path.Combine(saveDirectory, filename));
        }
    }

    private void ShowBeatmapListing(object sender, object data = null)
    {
        if (_beatmapListingForm == null || _beatmapListingForm.IsDisposed)
        {
            _beatmapListingForm = GuiComponentsProvider.Instance.GetClassImplementing<IBeatmapListingForm>();
            BeatmapListingFormPresenter presenter = new(_beatmapListingForm, _userDialogs);
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
            _ = new DownloadManagerFormPresenter(_downloadManagerForm, new DownloadManagerModel(OsuDownloadManager.Instance));
        }
    }
    private void ShowDownloadManager(object sender = null, object data = null)
    {
        CreateDownloadManagerForm();
        _downloadManagerForm.Show();
    }

    public void Dispose()
    {
        _collectionGenerator?.Dispose();
        GC.SuppressFinalize(this);
    }
}