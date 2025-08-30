namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Models.Controls;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Forms;

public sealed class ShowDownloadManagerHandler : IMainSidePanelActionHandler
{
    public static ShowDownloadManagerHandler Instance { get; private set; }

    private IDownloadManagerFormView _downloadManagerForm;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.ShowDownloadManager;

    private ShowDownloadManagerHandler() { }

    static ShowDownloadManagerHandler()
    {
        Instance = new ShowDownloadManagerHandler();
    }

    public Task HandleAsync(object sender, object data)
    {
        ShowDownloadManager();

        return Task.CompletedTask;
    }

    public void ShowDownloadManager()
    {
        if (_downloadManagerForm == null || _downloadManagerForm.IsDisposed)
        {
            _downloadManagerForm = Initalizer.GuiComponentsProvider.GetClassImplementing<IDownloadManagerFormView>();
            _ = new DownloadManagerFormPresenter(_downloadManagerForm, new DownloadManagerModel(OsuDownloadManager.Instance));
        }

        _downloadManagerForm.Show();
    }
}
