namespace CollectionManager.App.Shared.Presenters.Forms;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;

public class DownloadManagerFormPresenter
{
    private readonly IDownloadManagerFormView _formView;
    private readonly IDownloadManagerView _downloadManagerView;

    public DownloadManagerFormPresenter(IDownloadManagerFormView view, IDownloadManagerModel downloadManagerModel)
    {
        _formView = view;
        _downloadManagerView = _formView.DownloadManagerView;
        _ = new DownloadManagerPresenter(_downloadManagerView, downloadManagerModel);
    }
}