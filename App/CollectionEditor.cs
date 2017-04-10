using App.Interfaces;
using App.Misc;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO.OsuDb;
using GuiComponents.Interfaces;

namespace App
{
    public class CollectionEditor : ICollectionEditor, ICollectionNameValidator
    {
        private readonly ICollectionEditor _collectionEditor;
        private readonly ICollectionNameValidator _collectionNameValidator;
        private readonly ICollectionAddRenameForm _collectionAddRenameForm;
        private readonly MapCacher _mapCacher;

        public CollectionEditor(ICollectionEditor collectionEditor, ICollectionNameValidator collectionNameValidator,
            ICollectionAddRenameForm collectionAddRenameForm, MapCacher mapCacher)
        {
            _collectionEditor = collectionEditor;
            _collectionNameValidator = collectionNameValidator;
            _collectionAddRenameForm = collectionAddRenameForm;
            _mapCacher = mapCacher;
        }


        public void EditCollection(CollectionEditArgs e)
        {
            if (e.Action == CollectionEdit.Rename || e.Action == CollectionEdit.Add)
            {
                bool isRenameform = e.Action == CollectionEdit.Rename;

                var newCollectionName = _collectionAddRenameForm
                    .GetCollectionName(IsCollectionNameValid, e.OrginalName, isRenameform);

                if (newCollectionName == "")
                    return;
                if (e.Action == CollectionEdit.Rename)
                    e = CollectionEditArgs.RenameCollection(e.OrginalName, newCollectionName);
                else
                    e = CollectionEditArgs.AddCollections(new Collections() { new Collection(_mapCacher) { Name = newCollectionName } });

            }

            _collectionEditor.EditCollection(e);
        }

        public bool IsCollectionNameValid(string name)
        {
            return _collectionNameValidator.IsCollectionNameValid(name);
        }
    }
}