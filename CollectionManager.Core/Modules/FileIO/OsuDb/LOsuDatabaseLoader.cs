namespace CollectionManager.Core.Modules.FileIo.OsuDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Types;
using System;
using System.Threading;

public class LOsuDatabaseLoader : OsuDatabaseLoader
{
    private readonly ILogger logger;
    public string status { get; private set; }
    public LOsuDatabaseLoader(IMapDataManager mapDataStorer, Beatmap tempBeatmap) : base(mapDataStorer, tempBeatmap)
    {
    }

    protected override bool FileExists(string fullPath)
    {
        bool result = base.FileExists(fullPath);
        if (!result)
        {
            logger?.Log($"File \"{fullPath}\" doesn't exist!");
        }

        return result;
    }

    public override void LoadDatabase(string fullOsuDbPath, IProgress<string> progress, CancellationToken cancellationToken)
    {
        status = "Loading database";
        base.LoadDatabase(fullOsuDbPath, progress, cancellationToken);
        status = $"Loaded {NumberOfLoadedBeatmaps} beatmaps";
    }

    protected override void ReadDatabaseEntries(IProgress<string> progress, CancellationToken cancellationToken)
    {
        try
        {
            base.ReadDatabaseEntries(progress, cancellationToken);
        }
        catch (Exception e)
        {
            logger?.Log("Something went wrong while processing beatmaps(database is corrupt or its format changed)");
            logger?.Log("Try restarting your osu! first before reporting this problem.");
            logger?.Log("Exception: {0},{1}", e.Message, e.StackTrace);
            status = "Failed with exception " + $"Exception: {e.Message},{e.StackTrace}";
        }
    }
}
