namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;

public sealed class UploadCollectionChangesHandler : IMainSidePanelActionHandler
{
    private readonly IMainFormView _mainForm;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.UploadCollectionChanges;

    public UploadCollectionChangesHandler(IMainFormView mainForm, IUserDialogs userDialogs)
    {
        _mainForm = mainForm;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        IList<WebCollection> collections = (IList<WebCollection>)data;
        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        foreach (WebCollection webCollection in collections)
        {
            if (webCollection.Loaded && webCollection.Modified)
            {
                WebCollection newCollection = (await webCollection.Save(Initalizer.WebCollectionProvider)).First();

                webCollection.NumberOfBeatmaps = newCollection.OriginalNumberOfBeatmaps;
                sidePanel.WebCollections.CallReset();

                await _userDialogs.OkMessageBoxAsync("Collection was uploaded", "Info", MessageBoxType.Success);

            }
            else
            {
                // User doesn't care.
                // await _userDialogs.OkMessageBoxAsync("This collection is already up to date", "Info");
            }
        }
    }
}
