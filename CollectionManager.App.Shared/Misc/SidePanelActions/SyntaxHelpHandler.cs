namespace CollectionManager.App.Shared.Misc.SidePanelActions;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;

public sealed class SyntaxHelpHandler : IMainSidePanelActionHandler
{
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.SyntaxHelp;

    public SyntaxHelpHandler(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data) => await ResourceStrings.GeneralHelpDialogAsync(_userDialogs);
}
