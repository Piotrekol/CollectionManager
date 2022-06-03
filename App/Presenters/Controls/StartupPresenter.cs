using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Models;
using App.Properties;
using CollectionManagerExtensionsDll.Utils;
using Common;
using GuiComponents.Interfaces;
using Newtonsoft.Json;

namespace App.Presenters.Controls
{
    public class StartupPresenter
    {
        private readonly IStartupForm _view;
        private readonly SidePanelActionsHandler _sidePanelActionsHandler;
        private readonly IUserDialogs _userDialogs;
        private StartupSettings _startupSettings;
        private CancellationTokenSource _cancellationTokenSource;
        private Progress<string> _databaseLoadProgressReporter;
        private Task _databaseLoadTask;

        public StartupPresenter(IStartupForm view, SidePanelActionsHandler sidePanelActionsHandler, IUserDialogs userDialogs)
        {
            _view = view;
            _sidePanelActionsHandler = sidePanelActionsHandler;
            _userDialogs = userDialogs;
            _startupSettings = JsonConvert.DeserializeObject<StartupSettings>(Settings.Default.StartupSettings);
            _cancellationTokenSource = new CancellationTokenSource();
            _databaseLoadProgressReporter = new Progress<string>(report =>
            {
                if (string.IsNullOrEmpty(Initalizer.OsuDirectory))
                    _view.StartupView.LoadDatabaseStatusText = report;
                else
                    _view.StartupView.LoadDatabaseStatusText = $"osu! location: \"{Initalizer.OsuDirectory}\"{Environment.NewLine}{report}";
            });

            _view.StartupView.UseSelectedOptionsOnStartup = _startupSettings.UseSelectedOptionsOnStartup;
            _view.StartupView.StartupCollectionOperation += _view_StartupCollectionOperation;
            _view.StartupView.StartupDatabaseOperation += StartupView_StartupDatabaseOperation;
        }

        public async Task Run()
        {
            _databaseLoadTask = Task.Run(() => LoadDatabase(_cancellationTokenSource.Token));
            if (_startupSettings.UseSelectedOptionsOnStartup)
            {
                _view.StartupView.ButtonsEnabled = false;
                Application.UseWaitCursor = true;
                _view.Show();
            }
            else
                _view.ShowAndBlock();

            await _databaseLoadTask;
            if (_startupSettings.UseSelectedOptionsOnStartup)
            {
                _view.Close();
                Application.UseWaitCursor = false;
            }

            _startupSettings.UseSelectedOptionsOnStartup = _view.StartupView.UseSelectedOptionsOnStartup;
            SaveSettings();

            switch (_startupSettings.StartupCollectionAction)
            {
                case StartupCollectionAction.None:
                    break;
                case StartupCollectionAction.LoadCollection:
                    _sidePanelActionsHandler.LoadCollectionFile();
                    break;
                case StartupCollectionAction.LoadDefaultCollection:
                    _sidePanelActionsHandler.LoadDefaultCollection();
                    break;
            }
        }

        private async void StartupView_StartupDatabaseOperation(object sender, StartupDatabaseAction args)
        {
            _startupSettings.StartupDatabaseAction = args;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            await _databaseLoadTask;
            switch (args)
            {
                case StartupDatabaseAction.None:
                    Initalizer.OsuFileIo.OsuDatabase.LoadedMaps.UnloadBeatmaps();
                    Initalizer.OsuFileIo.ScoresDatabase.Clear();
                    Initalizer.OsuDirectory = null;
                    _view.StartupView.LoadDefaultCollectionButtonEnabled = false;
                    ((IProgress<string>)_databaseLoadProgressReporter).Report("osu! database unloaded");
                    break;
                case StartupDatabaseAction.LoadFromDifferentLocation:
                    var osuDirectory = Initalizer.OsuFileIo.OsuPathResolver.GetManualOsuDir(_userDialogs.SelectDirectory);
                    if (!string.IsNullOrEmpty(osuDirectory))
                    {
                        _startupSettings.OsuLocation = osuDirectory;
                        _databaseLoadTask = Task.Run(() => LoadDatabase(_cancellationTokenSource.Token));
                    }

                    break;
            }
        }

        private async void _view_StartupCollectionOperation(object sender, StartupCollectionAction args)
        {
            _startupSettings.StartupCollectionAction = args;
            _view.StartupView.ButtonsEnabled = false;
            Application.UseWaitCursor = true;
            await _databaseLoadTask;
            Application.UseWaitCursor = false;
            _view.Close();
        }

        private void LoadDatabase(CancellationToken cancellationToken)
        {
            var osuFileIo = Initalizer.OsuFileIo;
            var osuDirectory = Initalizer.OsuDirectory = string.IsNullOrEmpty(_startupSettings.OsuLocation)
                ? osuFileIo.OsuPathResolver.GetOsuDir()
                : _startupSettings.OsuLocation;

            if (string.IsNullOrEmpty(osuDirectory))
            {
                _view.StartupView.LoadDatabaseStatusText = "osu! could not be found. Select osu! location manually";
                return;
            }

            osuFileIo.OsuDatabase.LoadedMaps.UnloadBeatmaps();
            osuFileIo.ScoresDatabase.Clear();
            osuFileIo.OsuDatabase.Load(Path.Combine(osuDirectory, @"osu!.db"), _databaseLoadProgressReporter, cancellationToken);
            osuFileIo.OsuSettings.Load(osuDirectory);
            try
            {
                osuFileIo.ScoresLoader.ReadDb(Path.Combine(osuDirectory, @"scores.db"), cancellationToken);
                _view.StartupView.LoadDatabaseStatusText += $"{Environment.NewLine}Loaded {osuFileIo.ScoresDatabase.Scores.Count} scores";
            }
            catch (Exception)
            {
                _view.StartupView.LoadDatabaseStatusText += $"{Environment.NewLine}Could not load scores.";
            }

            _view.StartupView.LoadDefaultCollectionButtonEnabled = true;
            BeatmapUtils.OsuSongsDirectory = Initalizer.OsuFileIo.OsuSettings.CustomBeatmapDirectoryLocation;
        }

        private void SaveSettings()
        {
            Settings.Default.StartupSettings = JsonConvert.SerializeObject(_startupSettings);
            Settings.Default.Save();
        }
    }
}