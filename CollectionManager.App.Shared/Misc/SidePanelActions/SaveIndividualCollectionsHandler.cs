namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Extensions;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using System.IO;

public sealed class SaveIndividualCollectionsHandler : IMainSidePanelActionHandler
{
    private readonly OsuFileIo _osuFileIo;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.SaveIndividualCollections;

    public SaveIndividualCollectionsHandler(OsuFileIo osuFileIo, IUserDialogs userDialogs)
    {
        _osuFileIo = osuFileIo;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        string saveDirectory = await _userDialogs.SelectDirectoryAsync("Where collection files should be saved?", true);
        if (saveDirectory == string.Empty)
        {
            return;
        }

        bool saveAsOsdb = await _userDialogs.YesNoMessageBoxAsync("Save collections in osdb format?", "Collection save format", MessageBoxType.Question);
        string fileFormat = saveAsOsdb
            ? "osdb"
            : "db";

        await SidePanelActionHelpers.BeforeCollectionSave(Initalizer.LoadedCollections);
        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            string filename = $"{collection.Name.StripInvalidFileNameCharacters("_")}.{fileFormat}";
            _osuFileIo.CollectionLoader.SaveCollection([collection], Path.Combine(saveDirectory, filename));
        }
    }
}
