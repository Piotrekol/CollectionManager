namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
        get => button_ToggleDownloads.Enabled; set => button_ToggleDownloads.Enabled = value;
    }

    public string DownloadButtonText
    {
        set => button_ToggleDownloads.Text = value;
    }

    public void SetDownloadItems(ICollection<IDownloadItem> downloadItems) => ListViewDownload.SetObjects(downloadItems);

    public void UpdateDownloadItem(IDownloadItem downloadItem)
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
