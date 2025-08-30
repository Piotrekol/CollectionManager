namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;

public sealed class UploadNewCollectionsHandler : IMainSidePanelActionHandler
{
    private readonly OsuFileIo _osuFileIo;
    private readonly ICollectionEditor _collectionEditor;
    private readonly IUserDialogs _userDialogs;
    private readonly IMainFormView _mainForm;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.UploadNewCollections;

    public UploadNewCollectionsHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainForm)
    {
        _osuFileIo = osuFileIo;
        _collectionEditor = collectionEditor;
        _userDialogs = userDialogs;
        _mainForm = mainForm;
    }

    public async Task HandleAsync(object sender, object data)
    {
        if (!await Initalizer.WebCollectionProvider.IsCurrentKeyValid())
        {
            await _userDialogs.OkMessageBoxAsync("You need to login before uploading collections", "Error", MessageBoxType.Error);
            return;
        }

        IList<IOsuCollection> collectionList = (IList<IOsuCollection>)data;

        foreach (IOsuCollection c in collectionList)
        {
            if (!c.AllBeatmaps().Any())
            {
                await _userDialogs.OkMessageBoxAsync("Empty collection - upload aborted", "Error", MessageBoxType.Error);
                return;
            }
        }

        OsuCollections oldCollections = [.. collectionList];

        OsuCollections newCollections = [];
        foreach (IOsuCollection c in collectionList)
        {
            WebCollection webCollection = new(0, _osuFileIo.LoadedMaps, true)
            {
                Name = c.Name,
                LastEditorUsername = c.LastEditorUsername
            };

            foreach (BeatmapExtension collectionBeatmap in c.AllBeatmaps())
            {
                webCollection.AddBeatmap(collectionBeatmap);
            }

            newCollections.AddRange(await webCollection.Save(Initalizer.WebCollectionProvider));
        }

        _collectionEditor.EditCollection(CollectionEditArgs.RemoveCollections(oldCollections.Names));
        _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(newCollections));

        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainForm.SidePanelView;

        sidePanel.WebCollections.AddRange(newCollections.OfType<WebCollection>());
        sidePanel.WebCollections.CallReset();

        if (newCollections.Count > 0)
        {
            await _userDialogs.OkMessageBoxAsync($"Collections uploaded", "Info", MessageBoxType.Success);
        }

        if (newCollections.Count == 1)
        {
            _ = ProcessExtensions.OpenUrl($"https://osustats.ppy.sh/collection/{newCollections[0].OnlineId}");
        }
    }
}
