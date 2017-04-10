using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App.Interfaces;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using Gui.Misc;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class DownloadManagerPresenter
    {
        private readonly IDownloadManagerView _view;
        private readonly IDownloadManagerModel _model;
        private Timer enableButtonTimer = new Timer();

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

            enableButtonTimer.Tick += EnableButtonTimer_Tick;
            enableButtonTimer.Interval = 3000;

            _model = model;
            _model.DownloadItemsChanged += ModelOnDownloadItemsChanged;
            _model.DownloadItemUpdated += ModelOnDownloadItemUpdated;
            if (_model.DownloadItems.Count > 0)
                PopulateView(_model.DownloadItems);

        }

        private void ModelOnDownloadItemUpdated(object sender, EventArgs<DownloadItem> eventArgs)
        {
            _view.UpdateDownloadItem(eventArgs.Value);
        }

        private void ModelOnDownloadItemsChanged(object sender, EventArgs eventArgs)
        {
            PopulateView(_model.DownloadItems);
        }

        private void EnableButtonTimer_Tick(object sender, EventArgs e)
        {
            enableButtonTimer.Stop();
            _view.DownloadButtonIsEnabled = true;
            _model.EmitStopDownloads();
        }

        private void ToggleDownloads()
        {
            _downloadsActive = !_downloadsActive;
            if (_downloadsActive)
                _model.EmitStartDownloads();
            else
                _model.EmitStopDownloads();
        }
        private void SetDownloadButton(bool enabled = false, bool downloadsActive = false)
        {
            _view.DownloadButtonIsEnabled = enabled;
            _view.DownloadButtonText = downloadsActive ? "Stop Downloads" : "Resume Downloads";
        }


        private void PopulateView(ICollection<DownloadItem> downladItems)
        {
            _view.SetDownloadItems(downladItems);
        }
        private void ViewOnDownloadToggleClick(object sender, EventArgs eventArgs)
        {
            ToggleDownloads();
            SetDownloadButton(false, _downloadsActive);
            enableButtonTimer.Start();
        }
    }
}