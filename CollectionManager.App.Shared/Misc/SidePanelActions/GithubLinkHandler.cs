namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.Common;

public sealed class GithubLinkHandler : IMainSidePanelActionHandler
{
    public MainSidePanelActions Action { get; } = MainSidePanelActions.Github;

    public Task HandleAsync(object sender, object data)
    {
        _ = ProcessExtensions.OpenUrl("https://github.com/Piotrekol/CollectionManager");
        return Task.CompletedTask;
    }
}
