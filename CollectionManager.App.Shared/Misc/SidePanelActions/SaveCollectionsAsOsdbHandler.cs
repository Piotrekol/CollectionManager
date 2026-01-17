namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.FileIo;

public sealed class SaveCollectionsAsOsdbHandler : IMainSidePanelActionHandler
{
    private const string Filter = "CM database (.osdb)|*.osdb";

    private readonly OsuFileIo _osuFileIo;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.SaveCollectionsAsOsdb;

    public SaveCollectionsAsOsdbHandler(OsuFileIo osuFileIo, IUserDialogs userDialogs)
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
