using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Models;
using App.Properties;
using CollectionManager.Modules.CollectionsManager;
using CollectionManagerExtensionsDll.Utils;
using Common;
using GuiComponents.Interfaces;
using Newtonsoft.Json;

namespace App.Presenters.Controls
{
    public class StartupPresenter
    {
        private readonly IStartupForm _form;
        private readonly IStartupView _view;
        private readonly SidePanelActionsHandler _sidePanelActionsHandler;
        private readonly IUserDialogs _userDialogs;
        private readonly CollectionsManagerWithCounts _collectionsManager;
        private StartupSettings _startupSettings;
        private CancellationTokenSource _cancellationTokenSource;
        private Progress<string> _databaseLoadProgressReporter;
        private Task _databaseLoadTask;
        private bool _formClosedManually;
        private SemaphoreSlim _formClosed = new(0);

        public StartupPresenter(IStartupForm view, SidePanelActionsHandler sidePanelActionsHandler, IUserDialogs userDialogs, CollectionsManagerWithCounts collectionsManager)
        {
            _form = view;
            _view = view.StartupView;
            _sidePanelActionsHandler = sidePanelActionsHandler;
            _userDialogs = userDialogs;
            _collectionsManager = collectionsManager;
            _startupSettings = JsonConvert.DeserializeObject<StartupSettings>(Settings.Default.StartupSettings) ?? new StartupSettings();
            _cancellationTokenSource = new CancellationTokenSource();
            _databaseLoadProgressReporter = new Progress<string>(report =>
            {
                if (string.IsNullOrEmpty(Initalizer.OsuDirectory))
                    _view.LoadDatabaseStatusText = report;
                else
                    _view.LoadDatabaseStatusText = $"osu! location: \"{Initalizer.OsuDirectory}\"{Environment.NewLine}{report}";
            });

            _view.UseSelectedOptionsOnStartup = _startupSettings.AutoLoadMode;
            _form.Closing += _view_Closing;
            _view.StartupCollectionOperation += _view_StartupCollectionOperation;
            _view.StartupDatabaseOperation += _view_StartupDatabaseOperation;
        }

        private bool ShouldSkipStartupForm => _startupSettings.AutoLoadMode
            && (_startupSettings.StartupDatabaseAction == StartupDatabaseAction.Unload && _startupSettings.StartupCollectionAction != StartupCollectionAction.LoadOsuCollection) //Can't load default collection with no osu database loaded
            ;

        public async Task Run()
        {
            if (ShouldSkipStartupForm)
            {
                DoCollectionAction();
                return;
            }

            var loadedCollectionFromFile = _collectionsManager.CollectionsCount != 0;
            Application.UseWaitCursor = loadedCollectionFromFile || _startupSettings.AutoLoadMode;
            _view.UseSelectedOptionsOnStartupEnabled = !loadedCollectionFromFile;
            _view.CollectionButtonsEnabled = !(loadedCollectionFromFile || _startupSettings.AutoLoadMode);
            _view.DatabaseButtonsEnabled = true;
            _view.CollectionStatusText = loadedCollectionFromFile
                ? $"Going to load {_collectionsManager.CollectionsCount} collections with {_collectionsManager.BeatmapsInCollectionsCount} beatmaps"
                : string.Empty;

            _databaseLoadTask = Task.Run(() =>
            {
                LoadDatabase(_cancellationTokenSource.Token);
                Application.UseWaitCursor = false;
                _view.CollectionButtonsEnabled = true;
            });
            _form.Show();
            await _databaseLoadTask;

            //Wait for user to close the form if not in auto mode
            if (!_startupSettings.AutoLoadMode)
                await _formClosed.WaitAsync();

            _startupSettings.AutoLoadMode = _view.UseSelectedOptionsOnStartup;
            if (!loadedCollectionFromFile)
                SaveSettings();

            DoCollectionAction();
            CloseForm();
        }

        private void DoCollectionAction()
        {
            switch (_startupSettings.StartupCollectionAction)
            {
                case StartupCollectionAction.None:
                    break;
                case StartupCollectionAction.LoadCollectionFromFile:
                    _sidePanelActionsHandler.LoadCollectionFile();
                    break;
                case StartupCollectionAction.LoadOsuCollection:
                    _sidePanelActionsHandler.LoadOsuCollection();
                    break;
            }
        }

        private async void _view_StartupDatabaseOperation(object sender, StartupDatabaseAction args)
        {
            _startupSettings.StartupDatabaseAction = args;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            await _databaseLoadTask;
            switch (args)
            {
                case StartupDatabaseAction.Unload:
                    Initalizer.OsuFileIo.OsuDatabase.LoadedMaps.UnloadBeatmaps();
                    Initalizer.OsuFileIo.ScoresDatabase.Clear();
                    Initalizer.OsuDirectory = null;
                    _view.LoadOsuCollectionButtonEnabled = false;
                    GC.Collect();
                    ((IProgress<string>)_databaseLoadProgressReporter).Report("osu! database unloaded");
                    break;
                case StartupDatabaseAction.LoadFromDifferentLocation:
                    var osuDirectory = Initalizer.OsuFileIo.OsuPathResolver.GetManualOsuDir(_userDialogs.SelectDirectory);
                    if (!string.IsNullOrEmpty(osuDirectory))
                    {
                        _view.LoadOsuCollectionButtonEnabled = true;
                        _startupSettings.OsuLocation = osuDirectory;
                        _databaseLoadTask = Task.Run(() => LoadDatabase(_cancellationTokenSource.Token));
                    }

                    break;
                case StartupDatabaseAction.None:
                    break;
            }
        }

        private async void _view_StartupCollectionOperation(object sender, StartupCollectionAction args)
        {
            _startupSettings.StartupCollectionAction = args;
            _view.CollectionButtonsEnabled = _view.DatabaseButtonsEnabled = false;
            Application.UseWaitCursor = true;
            await _databaseLoadTask;
            Application.UseWaitCursor = false;
            CloseForm();
        }

        private void CloseForm()
        {
            _formClosedManually = true;
            _form.Close();
        }

        private void _view_Closing(object sender, EventArgs eventArgs)
        {
            _formClosed.Release();
            if (eventArgs is not FormClosingEventArgs formEventArgs || formEventArgs.CloseReason != CloseReason.UserClosing || _formClosedManually)
                return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            Initalizer.Quit();
        }

        private void LoadDatabase(CancellationToken cancellationToken)
        {
            var osuFileIo = Initalizer.OsuFileIo;
            var osuDirectory = Initalizer.OsuDirectory = string.IsNullOrEmpty(_startupSettings.OsuLocation)
                ? osuFileIo.OsuPathResolver.GetOsuDir()
                : _startupSettings.OsuLocation;

            if (string.IsNullOrEmpty(osuDirectory) || !Directory.Exists(osuDirectory))
            {
                _startupSettings.AutoLoadMode = _view.LoadOsuCollectionButtonEnabled = _view.UseSelectedOptionsOnStartup = false;
                _view.LoadDatabaseStatusText = "osu! could not be found. Select osu! location manually";
                return;
            }

            osuFileIo.OsuDatabase.LoadedMaps.UnloadBeatmaps();
            osuFileIo.ScoresDatabase.Clear();
            osuFileIo.OsuDatabase.Load(Path.Combine(osuDirectory, @"osu!.db"), _databaseLoadProgressReporter, cancellationToken);
            osuFileIo.OsuSettings.Load(osuDirectory);
            try
            {
                osuFileIo.ScoresLoader.ReadDb(Path.Combine(osuDirectory, @"scores.db"), cancellationToken);
                _view.LoadDatabaseStatusText += $"{Environment.NewLine}Loaded {osuFileIo.ScoresDatabase.Scores.Count} scores";
            }
            catch (Exception)
            {
                _view.LoadDatabaseStatusText += $"{Environment.NewLine}Could not load scores";
            }

            _view.LoadOsuCollectionButtonEnabled = true;
            BeatmapUtils.OsuSongsDirectory = Initalizer.OsuFileIo.OsuSettings.CustomBeatmapDirectoryLocation;
        }

        private void SaveSettings()
        {

            Settings.Default.StartupSettings = JsonConvert.SerializeObject(_startupSettings);
            Settings.Default.Save();
        }
    }
}