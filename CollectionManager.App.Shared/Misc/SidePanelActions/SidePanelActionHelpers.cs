namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using System.IO;

internal static class SidePanelActionHelpers
{
    public static async Task LoadCollectionsAsync(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, params string[] fileLocations)
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
                collections.AddRange(osuFileIo.CollectionLoader.LoadCollection(fileLocation));
            }
            catch (CorruptedFileException ex)
            {
                await userDialogs.OkMessageBoxAsync(ex.Message, "Error", MessageBoxType.Error);
            }
        }

        collectionEditor.EditCollection(CollectionEditArgs.AddCollections(collections));

        GC.Collect();
    }

    public static Task BeforeCollectionSave(IList<IOsuCollection> collections)
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

    public static bool OsuIsRunning(bool isLegacyOsu)
    {
        IEnumerable<Process> osuProcesses = Process.GetProcessesByName("osu!")
            .Where(process => process.MainModule is not null);

        return isLegacyOsu
            ? osuProcesses.Any(process => (Path.GetDirectoryName(process.MainModule.FileName)?.ToLowerInvariant()).Equals(Initalizer.OsuDirectory, StringComparison.OrdinalIgnoreCase))
            : osuProcesses.Any(process => process.MainModule.ModuleName == "osu!.exe");
    }
}
