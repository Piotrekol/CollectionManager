using App.Interfaces;
using App.Presenters.Controls;
using GuiComponents.Interfaces;

namespace App.Presenters.Forms
{
    public class DownloadManagerFormPresenter
    {
        private readonly IDownloadManagerFormView _formView;
        private readonly IDownloadManagerView _downloadManagerView;

        public DownloadManagerFormPresenter(IDownloadManagerFormView view, IDownloadManagerModel downloadManagerModel)
        {
            _formView = view;
            _downloadManagerView = _formView.DownloadManagerView;
            new DownloadManagerPresenter(_downloadManagerView, downloadManagerModel);
        }
    }
}