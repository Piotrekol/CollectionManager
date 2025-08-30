namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.FileIo.FileCollections;
using CollectionManager.Core.Types;

public static class CollectionLoaderExtensions
{
    public static async Task<OsuCollections> LoadCollectionFileAsync(this CollectionLoader collectionLoader, IUserDialogs userDialogs)
        => collectionLoader.LoadCollections(await userDialogs.SelectFileAsync("", "Collection database (*.db/*.osdb/*.realm)|*.db;*.osdb;*.realm", "collection.db"));

}
