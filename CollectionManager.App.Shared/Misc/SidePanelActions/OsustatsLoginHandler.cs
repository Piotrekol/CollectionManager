namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;

public sealed class OsustatsLoginHandler : IMainSidePanelActionHandler
{
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.OsustatsLogin;

    public OsustatsLoginHandler(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        string apiKey;

        if (data is string dataApiKey)
        {
            apiKey = dataApiKey;
        }
        else
        {
            IOsustatsApiLoginFormView osustatsLoginForm = Initalizer.GuiComponentsProvider.GetClassImplementing<IOsustatsApiLoginFormView>();
            osustatsLoginForm.ShowAndBlock();
            apiKey = osustatsLoginForm.ApiKey;
        }

        await Initalizer.PopulateOnlineWebCollectionsAsync(apiKey: apiKey);
    }
}
