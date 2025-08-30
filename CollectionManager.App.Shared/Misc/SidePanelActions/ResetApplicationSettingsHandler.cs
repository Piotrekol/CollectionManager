namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;

public sealed class ResetApplicationSettingsHandler : IMainSidePanelActionHandler
{
    private const string Caption = "Settings reset";
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.ResetApplicationSettings;

    public ResetApplicationSettingsHandler(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        if (await _userDialogs.YesNoMessageBoxAsync("Are you sure that you want to reset all Collection Manager settings?", Caption, MessageBoxType.Question))
        {
            Initalizer.Settings.Reset();
            Initalizer.Settings.Save();
            await _userDialogs.OkMessageBoxAsync("Settings were set to their defaults. Restart CM to fully reload.", Caption);
        }
        else
        {
            await _userDialogs.OkMessageBoxAsync("Reset aborted.", Caption);
        }
    }
}
