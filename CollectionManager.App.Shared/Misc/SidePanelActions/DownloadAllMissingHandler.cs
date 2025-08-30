namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Types;

public sealed class DownloadAllMissingHandler : IMainSidePanelActionHandler
{
    private readonly IUserDialogs _userDialogs;
    private readonly ILoginFormView _loginForm;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.DownloadAllMissing;

    public DownloadAllMissingHandler(IUserDialogs userDialogs, ILoginFormView loginForm)
    {
        _userDialogs = userDialogs;
        _loginForm = loginForm;
    }

    public async Task HandleAsync(object sender, object data)
    {
        Beatmaps downloadableBeatmaps = [];
        foreach (IOsuCollection collection in Initalizer.LoadedCollections)
        {
            foreach (Beatmap beatmap in collection.DownloadableBeatmaps)
            {
                downloadableBeatmaps.Add(beatmap);
            }
        }

        if (downloadableBeatmaps.Count == 0)
        {
            await _userDialogs.OkMessageBoxAsync("You don't have any missing maps that CM is able to download", "Info", MessageBoxType.Info);
            return;
        }

        if (await OsuDownloadManager.Instance.AskUserForSaveDirectoryAndLoginAsync(_userDialogs, _loginForm))
        {
            OsuDownloadManager.Instance.DownloadBeatmaps(downloadableBeatmaps);
            ShowDownloadManagerHandler.Instance.ShowDownloadManager();
        }
    }
}
