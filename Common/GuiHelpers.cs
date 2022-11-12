using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using Common;

namespace Gui.Misc
{
    public static class GuiHelpers
    {
        public delegate void BeatmapsEventArgs(object sender, Beatmaps args);
        public delegate void CollectionBeatmapsEventArgs(object sender, Beatmaps args, string collectionName);
        public delegate void CollectionReorderEventArgs(object sender, Collections collections, Collection targetCollection, bool placeBefore, string sortColumn, SortOrder sortOrder);
        public delegate void BeatmapListingActionArgs(object sender, BeatmapListingAction args);
        public delegate void SidePanelActionsHandlerArgs(object sender, MainSidePanelActions args, object data = null);
        public delegate void LoadFileArgs(object sender, string[] filePaths);
        public delegate void StartupCollectionEventArgs(object sender, StartupCollectionAction args);
        public delegate void StartupDatabaseEventArgs(object sender, StartupDatabaseAction args);
    }
}