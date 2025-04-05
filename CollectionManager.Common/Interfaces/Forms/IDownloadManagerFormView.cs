namespace CollectionManager.Common.Interfaces.Forms;

using CollectionManager.Common.Interfaces.Controls;

public interface IDownloadManagerFormView : IForm
{
    IDownloadManagerView DownloadManagerView { get; }
}