namespace CollectionManager.Extensions.Modules.CollectionApiGenerator;

using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class CollectionsApiGenerator : IDisposable
{
    public event EventHandler CollectionsUpdated;
    public event EventHandler StatusUpdated;
    private string _status = "";
    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    public double ProcessingCompletionPercentage { get; set; }

    private Task<OsuCollections> _processingTask = Task.FromResult<OsuCollections>([]);
    private CancellationTokenSource _cancellationTokenSource;
    private OsuCollections _collections;
    private readonly MapCacher _loadedBeatmaps;
    private UserTopGenerator _userTopGenerator;

    public OsuCollections Collections
    {
        get => _collections;
        set
        {
            _collections = value;
            CollectionsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    public CollectionsApiGenerator(MapCacher loadedBeatmaps)
    {
        _loadedBeatmaps = loadedBeatmaps;
    }

    public static string CreateCollectionName(ApiScore score, string username, string collectionNameFormat) => UserTopGenerator.CreateCollectionName(score, username, collectionNameFormat);

    public void GenerateCollection(CollectionGeneratorConfiguration configuration)
    {
        _userTopGenerator ??= new UserTopGenerator(configuration.ApiKey, _loadedBeatmaps);

        if (_cancellationTokenSource?.TryCancel() ?? false)
        {
            _cancellationTokenSource?.Dispose();
        }

        _cancellationTokenSource = new CancellationTokenSource();

        if (_processingTask is null || _processingTask.IsCompleted)
        {
            _processingTask = Task.Run(async () => Collections = await _userTopGenerator.GetPlayersCollectionsAsync(configuration, Log, _cancellationTokenSource.Token));
        }
    }

    private void Log(string message, double percentage)
    {
        ProcessingCompletionPercentage = percentage;
        Status = message;
    }

    public async Task AbortAsync()
    {
        _ = _cancellationTokenSource?.TryCancel();

        try
        {
            _ = await _processingTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            Collections = [];
        }
    }

    public void Dispose() => _cancellationTokenSource?.Dispose();
}