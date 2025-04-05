namespace CollectionManagerApp.Interfaces.Forms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Interfaces;

public interface IMainFormModel
{
    ICollectionEditor GetCollectionEditor();
    IUserDialogs GetUserDialogs();
}