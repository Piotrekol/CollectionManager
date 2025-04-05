namespace CollectionManagerApp.Misc;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

internal class CollectionEditArgsExtension : CollectionEditArgs
{
    public CollectionEditArgsExtension(CollectionEdit action) : base(action)
    {
    }

    public static CollectionEditArgs ExportBeatmaps(OsuCollections collections) => new CollectionEditArgsExtension(CollectionEdit.ExportBeatmaps)
    {
        Collections = collections
    };
}
