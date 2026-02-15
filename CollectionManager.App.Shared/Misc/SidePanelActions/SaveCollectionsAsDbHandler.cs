namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.FileIo;

public sealed class SaveCollectionsAsDbHandler : IMainSidePanelActionHandler
{
    private const string Filter = "osu! Collection database (.db)|*.db";

    private readonly OsuFileIo _osuFileIo;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.SaveCollectionsAsDb;

    public SaveCollectionsAsDbHandler(OsuFileIo osuFileIo, IUserDialogs userDialogs)
    {
        _osuFileIo = osuFileIo;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        string fileLocation = await _userDialogs.SaveFileAsync("Where collection file should be saved?", Filter);
        if (string.IsNullOrWhiteSpace(fileLocation))
        {
            return;
        }

        await SidePanelActionHelpers.BeforeCollectionSave(Initalizer.LoadedCollections);
        _osuFileIo.CollectionLoader.SaveCollection(Initalizer.LoadedCollections, fileLocation);
    }
}
