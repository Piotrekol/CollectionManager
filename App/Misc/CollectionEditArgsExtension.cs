namespace CollectionManagerApp.Misc;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;

internal class CollectionEditArgsExtension : CollectionEditArgs
{
    public CollectionEditArgsExtension(CollectionEdit action) : base(action)
    {
    }

    public static CollectionEditArgs ExportBeatmaps(IReadOnlyList<string> collections) => new CollectionEditArgsExtension(CollectionEdit.ExportBeatmaps)
    {
        CollectionNames = collections
    };
}
