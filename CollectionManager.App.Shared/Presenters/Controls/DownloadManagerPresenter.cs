namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Extensions.Modules.Downloader.Api;
using System.Timers;

public class DownloadManagerPresenter : IDisposable
{
    private readonly IDownloadManagerView _view;
    private readonly IDownloadManagerModel _model;
    private readonly Timer enableButtonTimer = new();

    private bool _downloadsActive = true;

    public DownloadManagerPresenter(IDownloadManagerView view, IDownloadManagerModel model)
    {
        _view = view;
        _view.DownloadToggleClick += ViewOnDownloadToggleClick;
        _view.Disposed += (s, a) =>
        {
            _model.DownloadItemsChanged -= ModelOnDownloadItemsChanged;
            _model.DownloadItemUpdated -= ModelOnDownloadItemUpdated;
        };

        enableButtonTimer.Elapsed += EnableButtonTimer_Elapsed;
        enableButtonTimer.Interval = 3000;

        _model = model;
        _model.DownloadItemsChanged += ModelOnDownloadItemsChanged;
        _model.DownloadItemUpdated += ModelOnDownloadItemUpdated;
        if (_model.DownloadItems.Count > 0)
        {
            PopulateView(_model.DownloadItems);
        }
    }

    private void ModelOnDownloadItemUpdated(object sender, DownloadItem downloadItem) => _view.UpdateDownloadItem(downloadItem);

    private void ModelOnDownloadItemsChanged(object sender, EventArgs eventArgs) => PopulateView(_model.DownloadItems);

    private void EnableButtonTimer_Elapsed(object sender, EventArgs e)
    {
        enableButtonTimer.Stop();
        _view.DownloadButtonIsEnabled = true;
    }

    private void ToggleDownloads()
    {
        _downloadsActive = !_downloadsActive;
        if (_downloadsActive)
        {
            _model.EmitStartDownloads();
        }
        else
        {
            _model.EmitStopDownloads();
        }
    }
    private void SetDownloadButton(bool enabled = false, bool downloadsActive = false)
    {
        _view.DownloadButtonIsEnabled = enabled;
        _view.DownloadButtonText = downloadsActive ? "Stop Downloads" : "Resume Downloads";
    }

    private void PopulateView(ICollection<IDownloadItem> downladItems) => _view.SetDownloadItems(downladItems);
    private void ViewOnDownloadToggleClick(object sender, EventArgs eventArgs)
    {
        ToggleDownloads();
        SetDownloadButton(false, _downloadsActive);
        enableButtonTimer.Start();
    }

    public void Dispose()
    {
        enableButtonTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}