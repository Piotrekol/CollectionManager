namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Extensions.Utils;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Models;
using CollectionManagerApp.Properties;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Windows.Forms;

public class StartupPresenter : IDisposable
{
    private readonly IStartupForm _form;
    private readonly IStartupView _view;
    private readonly SidePanelActionsHandler _sidePanelActionsHandler;
    private readonly IUserDialogs _userDialogs;
    private readonly CollectionsManagerWithCounts _collectionsManager;
    private readonly StartupSettings _startupSettings;
    private CancellationTokenSource _cancellationTokenSource;
    private readonly Progress<string> _databaseLoadProgressReporter;
    private Task _databaseLoadTask;
    private bool _formClosedManually;
    private readonly SemaphoreSlim _formClosed = new(0);

    public StartupPresenter(IStartupForm view, SidePanelActionsHandler sidePanelActionsHandler, IUserDialogs userDialogs, CollectionsManagerWithCounts collectionsManager, IInfoTextModel infoTextModel)
    {
        _form = view;
        _view = view.StartupView;
        _sidePanelActionsHandler = sidePanelActionsHandler;
        _userDialogs = userDialogs;
        _collectionsManager = collectionsManager;
        _startupSettings = JsonConvert.DeserializeObject<StartupSettings>(Settings.Default.StartupSettings) ?? new StartupSettings();
        _cancellationTokenSource = new CancellationTokenSource();
        _databaseLoadProgressReporter = new Progress<string>(report => _view.LoadDatabaseStatusText = string.IsNullOrEmpty(Initalizer.OsuDirectory)
                ? report
                : $"osu! location: \"{Initalizer.OsuDirectory}\"{Environment.NewLine}{report}");
        _ = new InfoTextPresenter(_view.InfoTextView, infoTextModel);

        _view.UseSelectedOptionsOnStartup = _startupSettings.AutoLoadMode;
        _form.Closing += _view_Closing;
        _view.StartupCollectionOperation += _view_StartupCollectionOperation;
        _view.StartupDatabaseOperation += _view_StartupDatabaseOperation;
    }

    private bool ShouldSkipStartupForm => _startupSettings.AutoLoadMode
        && _startupSettings.StartupDatabaseAction == StartupDatabaseAction.Unload && _startupSettings.StartupCollectionAction != StartupCollectionAction.LoadOsuCollection //Can't load default collection with no osu database loaded
        ;

    public async Task Run()
    {
        if (ShouldSkipStartupForm)
        {
            DoCollectionAction();
            return;
        }

        bool loadedCollectionFromFile = _collectionsManager.CollectionsCount != 0;
        Application.UseWaitCursor = loadedCollectionFromFile || _startupSettings.AutoLoadMode;
        _view.UseSelectedOptionsOnStartupEnabled = !loadedCollectionFromFile;
        _view.CollectionButtonsEnabled = !(loadedCollectionFromFile || _startupSettings.AutoLoadMode);
        _view.DatabaseButtonsEnabled = true;
        _view.LoadLazerDatabaseButtonEnabled = OsuPathResolver.TryGetLazerDataPath(out _);
        _view.LoadStableDatabaseButtonEnabled = OsuPathResolver.TryGetStablePath(out _);

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
        {
            await _formClosed.WaitAsync();
        }

        _startupSettings.AutoLoadMode = _view.UseSelectedOptionsOnStartup;
        if (!loadedCollectionFromFile)
        {
            SaveSettings();
        }

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
                string osuDirectory = OsuPathResolver.GetManualOsuPath(_userDialogs.SelectDirectory);

                if (!string.IsNullOrEmpty(osuDirectory))
                {
                    _view.LoadOsuCollectionButtonEnabled = true;
                    _startupSettings.OsuLocation = osuDirectory;
                    _databaseLoadTask = Task.Run(() => LoadDatabase(_cancellationTokenSource.Token));
                }

                break;

            case StartupDatabaseAction.LoadLazer:
                if (OsuPathResolver.TryGetLazerDataPath(out string lazerLocation))
                {
                    _view.LoadOsuCollectionButtonEnabled = true;
                    _startupSettings.OsuLocation = lazerLocation;
                    _databaseLoadTask = Task.Run(() => LoadDatabase(_cancellationTokenSource.Token));
                }

                break;

            case StartupDatabaseAction.LoadStable:
                if (OsuPathResolver.TryGetStablePath(out string stableLocation))
                {
                    _view.LoadOsuCollectionButtonEnabled = true;
                    _startupSettings.OsuLocation = stableLocation;
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
        _ = _formClosed.Release();
        if (eventArgs is not FormClosingEventArgs formEventArgs || formEventArgs.CloseReason != CloseReason.UserClosing || _formClosedManually)
        {
            return;
        }

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        Initalizer.Quit();
    }

    private void LoadDatabase(CancellationToken cancellationToken)
    {
        OsuFileIo osuFileIo = Initalizer.OsuFileIo;
        string osuDirectory = Initalizer.OsuDirectory = string.IsNullOrEmpty(_startupSettings.OsuLocation)
            ? OsuPathResolver.GetOsuOrLazerPath()
            : _startupSettings.OsuLocation;

        string osuDbOrRealmPath = new[] { Path.Combine(osuDirectory, @"osu!.db"), Path.Combine(osuDirectory, @"client.realm") }
            .Where(File.Exists)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(osuDirectory) || !Directory.Exists(osuDirectory) || string.IsNullOrEmpty(osuDbOrRealmPath))
        {
            _startupSettings.AutoLoadMode = _view.LoadOsuCollectionButtonEnabled = _view.UseSelectedOptionsOnStartup = false;
            _view.LoadDatabaseStatusText = "osu! could not be found. Select osu! location manually";
            return;
        }

        osuFileIo.OsuDatabase.LoadedMaps.UnloadBeatmaps();
        osuFileIo.ScoresDatabase.Clear();

        try
        {
            _ = osuFileIo.OsuDatabase.Load(osuDbOrRealmPath, _databaseLoadProgressReporter, cancellationToken);
        }
        catch (Exception exception)
        {
            _view.LoadDatabaseStatusText = $"Error: {exception.Message}";
            return;
        }

        osuFileIo.OsuSettings.Load(osuDirectory);

        try
        {
            osuFileIo.ScoresLoader.ReadDb(Path.Combine(osuDirectory, @"scores.db"), cancellationToken);
            _view.LoadDatabaseStatusText += $"{Environment.NewLine}Loaded {osuFileIo.ScoresDatabase.Scores.Count} scores";
            osuFileIo.ScoresDatabase.UpdateBeatmapsScoreMetadata(osuFileIo.LoadedMaps);
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

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
        _formClosed?.Dispose();
        GC.SuppressFinalize(this);
    }
}