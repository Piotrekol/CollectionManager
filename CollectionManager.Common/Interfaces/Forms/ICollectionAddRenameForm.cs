namespace CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Common.Interfaces.Controls;
public interface ICollectionAddRenameForm : IForm
{
    ICollectionRenameView CollectionRenameView { get; }
    bool IsRenameForm { get; set; }
}