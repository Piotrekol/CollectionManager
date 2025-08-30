namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using System.IO;

public sealed class ListMissingMapsHandler : IMainSidePanelActionHandler
{
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.ListMissingMaps;

    public ListMissingMapsHandler(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        string fileLocation = await _userDialogs.SaveFileAsync("Where list of all maps should be saved?", "Txt(.txt)|*.txt|Html(.html)|*.html");
        if (fileLocation == string.Empty)
        {
            return;
        }

        ListGenerator listGenerator = new();
        CollectionListSaveType CollectionListSaveType = Path.GetExtension(fileLocation).Equals(".txt", StringComparison.OrdinalIgnoreCase)
            ? CollectionListSaveType.Txt
            : CollectionListSaveType.Html;
        string contents = listGenerator.GetMissingMapsList(Initalizer.LoadedCollections, CollectionListSaveType);
        File.WriteAllText(fileLocation, contents);
    }
}
