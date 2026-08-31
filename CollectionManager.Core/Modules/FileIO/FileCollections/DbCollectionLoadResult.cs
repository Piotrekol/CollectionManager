namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Types;

public sealed record DbCollectionLoadResult(OsuCollections Collections, int FileVersion)
    : CollectionLoadResult(Collections);
