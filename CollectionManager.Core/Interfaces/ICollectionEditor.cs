namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public interface ICollectionEditor
{
    /// <summary>
    /// Performs a single edit on the collections.
    /// </summary>
    void EditCollection(CollectionEditArgs args);

    /// <summary>
    /// Performs multiple edits on the collections in bulk.
    /// </summary>
    /// <param name="args">List of arguments containing the actions to perform.</param>
    void EditCollection(IReadOnlyList<CollectionEditArgs> args);

    /// <summary>
    /// Gets the collections that contain any of the specified beatmaps.
    /// </summary>
    OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps);
}