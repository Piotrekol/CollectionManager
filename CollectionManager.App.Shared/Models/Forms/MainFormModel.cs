namespace CollectionManager.App.Shared.Models.Forms;

using CollectionManager.App.Shared.Interfaces.Forms;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Interfaces;

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