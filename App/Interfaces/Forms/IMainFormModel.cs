using CollectionManager.Interfaces;
using GuiComponents.Interfaces;

namespace App.Interfaces.Forms
{
    public interface IMainFormModel
    {
        ICollectionEditor GetCollectionEditor();
        IUserDialogs GetUserDialogs();
    }
}