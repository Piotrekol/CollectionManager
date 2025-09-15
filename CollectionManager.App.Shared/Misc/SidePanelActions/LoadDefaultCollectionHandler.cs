namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo;
using System.IO;

public sealed class LoadDefaultCollectionHandler : IMainSidePanelActionHandler
{
    private readonly OsuFileIo _osuFileIo;
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.LoadDefaultCollection;

    public LoadDefaultCollectionHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs)
    {
        _osuFileIo = osuFileIo;
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
        => await SidePanelActionHelpers.LoadCollectionsAsync(_osuFileIo, _collectionEditor, _userDialogs,
            Path.Combine(Initalizer.OsuDirectory, "collection.db"), Path.Combine(Initalizer.OsuDirectory, "client.realm"));
}
