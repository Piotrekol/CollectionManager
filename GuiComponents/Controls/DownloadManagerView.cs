using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class DownloadManagerView : UserControl, IDownloadManagerView
    {
        public DownloadManagerView()
        {
            InitializeComponent();

            button_ToggleDownloads.Click += (s, a) => DownloadToggleClick?.Invoke(this, EventArgs.Empty);
            ListViewDownload.FullRowSelect = true;
        }

        public event EventHandler DownloadToggleClick;

        public bool DownloadButtonIsEnabled
        {
            get { return this.button_ToggleDownloads.Enabled; }
            set { button_ToggleDownloads.Enabled = value; }
        }

        public string DownloadButtonText
        {
            set { button_ToggleDownloads.Text = value; }
        }

        public void SetDownloadItems(ICollection<DownloadItem> downloadItems)
        {
            ListViewDownload.SetObjects(downloadItems);
        }

        public void UpdateDownloadItem(DownloadItem downloadItem)
        {
            try
            {
                ListViewDownload.RefreshObject(downloadItem);
            }
            catch
            {
                // ignored
            }
        }
    }
}
