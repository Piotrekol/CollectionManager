namespace CollectionManagerApp.Models.Forms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Interfaces;
using CollectionManagerApp.Interfaces.Forms;

public class MainFormModel : IMainFormModel
{
    public MainFormModel(ICollectionEditor collectionEditor, IUserDialogs userDialogs)
    {
        UserDialogs = userDialogs;
        CollectionEditor = collectionEditor;
    }

    private ICollectionEditor CollectionEditor { get; }
    public ICollectionEditor GetCollectionEditor() => CollectionEditor;

    private IUserDialogs UserDialogs { get; }
    public IUserDialogs GetUserDialogs() => UserDialogs;
}