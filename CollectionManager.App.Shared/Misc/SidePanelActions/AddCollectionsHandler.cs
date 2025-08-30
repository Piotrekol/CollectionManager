namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;

public sealed class AddCollectionsHandler : IMainSidePanelActionHandler
{
    public MainSidePanelActions Action { get; } = MainSidePanelActions.AddCollections;

    public Task HandleAsync(object sender, object data)
    {
        IList<WebCollection> webCollections = (IList<WebCollection>)data;

        OsuCollections collections = [];
        foreach (WebCollection webCollection in webCollections)
        {
            if (!Initalizer.CollectionsManager.LoadedCollections.Contains(webCollection))
            {
                collections.Add(webCollection);
            }
        }

        Initalizer.CollectionsManager.EditCollection(CollectionEditArgs.AddCollections(collections));
        return Task.CompletedTask;
    }
}
