namespace CollectionManager.Common;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;

public static class GuiHelpers
{
    public delegate void BeatmapsEventArgs(object sender, Beatmaps args);
    public delegate void CollectionBeatmapsEventArgs(object sender, Beatmaps args, string collectionName);
    public delegate void CollectionReorderEventArgs(object sender, OsuCollections collections, OsuCollection targetCollection, bool placeBefore, string sortColumn, SortOrder sortOrder);
    public delegate void BeatmapListingActionArgs(object sender, BeatmapListingAction args);
    public delegate void SidePanelActionsHandlerArgs(object sender, MainSidePanelActions args, object data = null);
    public delegate void LoadFileArgs(object sender, string[] filePaths);
    public delegate void StartupCollectionEventArgs(object sender, StartupCollectionAction args);
    public delegate void StartupDatabaseEventArgs(object sender, StartupDatabaseAction args);
    public delegate void ColumnsToggledEventArgs(object sender, string[] visibleColumnAspectNames);
    public delegate void BeatmapGroupColumnChangedEventArgs(object sender, string groupColumnName);
    public delegate void BeatmapGroupCollapsedChangedEventArgs(object sender, bool collapsed);
}