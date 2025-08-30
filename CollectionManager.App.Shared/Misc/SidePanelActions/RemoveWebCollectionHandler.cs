namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public sealed class RemoveWebCollectionHandler : IMainSidePanelActionHandler
{
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;
    private readonly IMainFormView _mainForm;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.RemoveWebCollection;

    public RemoveWebCollectionHandler(ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainForm)
    {
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
        _mainForm = mainForm;
    }

    public async Task HandleAsync(object sender, object data)
    {
        IList<WebCollection> collectionList = (IList<WebCollection>)data;
        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        foreach (WebCollection collection in collectionList)
        {
            if (await Initalizer.WebCollectionProvider.RemoveCollection(collection.OnlineId))
            {
                _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(new[] { collection.Name }));
                _ = sidePanel.WebCollections.Remove(collection);
            }
            else
            {
                await _userDialogs.OkMessageBoxAsync($"Couldn't remove collection {collection.Name}", "Error", MessageBoxType.Error);
            }
        }
    }
}
