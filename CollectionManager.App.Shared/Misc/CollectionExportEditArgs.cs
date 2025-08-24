namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

internal class CollectionExportEditArgs : CollectionEditArgs
{
    public CollectionExportEditArgs(CollectionEdit action) : base(action)
    {
    }

    public static CollectionEditArgs ExportBeatmaps(IReadOnlyList<string> collections) => new CollectionExportEditArgs(CollectionEdit.ExportBeatmaps)
    {
        CollectionNames = collections
    };

    public static CollectionEditArgs ExportBeatmaps(IEnumerable<Beatmap> beatmaps) => new CollectionExportEditArgs(CollectionEdit.ExportBeatmaps)
    {
        Beatmaps = beatmaps
    };
}
