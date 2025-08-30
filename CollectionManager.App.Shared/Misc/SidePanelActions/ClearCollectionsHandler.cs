namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.Common;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;

public sealed class ClearCollectionsHandler : IMainSidePanelActionHandler
{
    private readonly ICollectionEditor _collectionEditor;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.ClearCollections;

    public ClearCollectionsHandler(ICollectionEditor collectionEditor)
    {
        _collectionEditor = collectionEditor;
    }

    public Task HandleAsync(object sender, object data)
    {
        _collectionEditor.EditCollection(CollectionEditArgs.ClearCollections());
        return Task.CompletedTask;
    }
}
