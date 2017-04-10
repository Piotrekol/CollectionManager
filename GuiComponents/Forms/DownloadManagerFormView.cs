using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class DownloadManagerFormView : BaseForm, IDownloadManagerFormView
    {
        public DownloadManagerFormView()
        {
            InitializeComponent();
        }

        public IDownloadManagerView DownloadManagerView => downloadManagerView1;
    }
}
