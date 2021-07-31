using System.Collections.Generic;
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
                switch (e.Action)
                {
                    case CollectionEdit.Rename:
                        e = CollectionEditArgs.RenameCollection(e.OrginalName, newCollectionName);
                        break;
                    case CollectionEdit.Add:
                        e = CollectionEditArgs.AddCollections(new Collections() { new Collection(_mapCacher) { Name = newCollectionName } });
                        break;
                }
            }
            else if (e.Action == CollectionEdit.Intersect)
            {
                if (e.Collections.Count < 2)
                    return;

                e.Collections.Add(new Collection(_mapCacher) { Name = GetValidCollectionName(e.NewName) });
            }
            else if (e.Action == CollectionEdit.Inverse)
            {
                e.Collections.Add(new Collection(_mapCacher) { Name = GetValidCollectionName(e.NewName) });
            }
            else if (e.Action == CollectionEdit.Duplicate)
            {
                var newCollection = new Collection(_mapCacher) { Name = GetValidCollectionName(e.OrginalName) };
                _collectionEditor.EditCollection(
                    CollectionEditArgs.AddCollections(new Collections() { newCollection })
                );
                var beatmaps = new Beatmaps();
                beatmaps.AddRange(e.Collections[0].AllBeatmaps());

                e = CollectionEditArgs.AddBeatmaps(newCollection.Name, beatmaps);
            }

            _collectionEditor.EditCollection(e);
        }

        public bool IsCollectionNameValid(string name)
            => _collectionNameValidator.IsCollectionNameValid(name);

        public string GetValidCollectionName(string desiredName, List<string> aditionalNames = null)
            => _collectionNameValidator.GetValidCollectionName(desiredName, aditionalNames);
    }
}