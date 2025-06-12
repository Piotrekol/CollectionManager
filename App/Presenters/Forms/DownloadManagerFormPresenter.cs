namespace CollectionManagerApp.Presenters.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Presenters.Controls;

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