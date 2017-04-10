using App.Interfaces;
using App.Interfaces.Forms;
using GuiComponents.Interfaces;

namespace App.Models.Forms
{
    public class MainFormModel : IMainFormModel
    {

        public MainFormModel(ICollectionEditor collectionEditor, IUserDialogs userDialogs)
        {
            UserDialogs = userDialogs;
            CollectionEditor = collectionEditor;
        }

        private ICollectionEditor CollectionEditor { get; }
        public ICollectionEditor GetCollectionEditor()
        {
            return CollectionEditor;
        }

        private IUserDialogs UserDialogs { get; }
        public IUserDialogs GetUserDialogs()
        {
            return UserDialogs;
        }
        
    }
}