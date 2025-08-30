namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using System;
using System.Threading.Tasks;

public sealed class LoadCollectionHandler : IMainSidePanelActionHandler
{
    private readonly OsuFileIo _osuFileIo;
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.LoadCollection;

    public LoadCollectionHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs)
    {
        _osuFileIo = osuFileIo;
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        OsuCollections collections;

        try
        {
            collections = data is string fileLocation
                ? _osuFileIo.CollectionLoader.LoadCollection(fileLocation)
                : await _osuFileIo.CollectionLoader.LoadCollectionFileAsync(_userDialogs);
        }
        catch (CorruptedFileException ex)
        {
            await _userDialogs.OkMessageBoxAsync(ex.Message, "Error", MessageBoxType.Error);

            return;
        }

        _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(collections));

        GC.Collect();
    }
}
