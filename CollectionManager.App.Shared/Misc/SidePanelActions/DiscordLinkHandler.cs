namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.Common;

public sealed class DiscordLinkHandler : IMainSidePanelActionHandler
{
    public MainSidePanelActions Action { get; } = MainSidePanelActions.Discord;

    public Task HandleAsync(object sender, object data)
    {
        _ = ProcessExtensions.OpenUrl("https://osustats.ppy.sh/discord");
        return Task.CompletedTask;
    }
}
