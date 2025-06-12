namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class DownloadManagerFormView : BaseForm, IDownloadManagerFormView
{
    public DownloadManagerFormView()
    {
        InitializeComponent();
    }

    public IDownloadManagerView DownloadManagerView => downloadManagerView1;
}
