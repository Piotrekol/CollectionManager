namespace CollectionManagerApp;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using CollectionManagerApp.Misc;

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

    public void EditCollection(CollectionEditArgs args)
    {
        if (args.Action is CollectionEdit.Rename or CollectionEdit.Add)
        {
            bool isRenameform = args.Action == CollectionEdit.Rename;

            string newCollectionName = _collectionAddRenameForm
                .GetCollectionName(IsCollectionNameValid, args.OrginalName, isRenameform);

            if (newCollectionName == "")
            {
                return;
            }

            switch (args.Action)
            {
                case CollectionEdit.Rename:
                    args = CollectionEditArgs.RenameCollection(args.OrginalName, newCollectionName);
                    break;
                case CollectionEdit.Add:
                    args = CollectionEditArgs.AddCollections([new OsuCollection(_mapCacher) { Name = newCollectionName }]);
                    break;
            }
        }
        else if (args.Action is CollectionEdit.Intersect or CollectionEdit.Difference)
        {
            if (args.Collections.Count < 2)
            {
                return;
            }

            args.Collections.Add(new OsuCollection(_mapCacher) { Name = GetValidCollectionName(args.NewName) });
        }
        else if (args.Action == CollectionEdit.Inverse)
        {
            args.Collections.Add(new OsuCollection(_mapCacher) { Name = GetValidCollectionName(args.NewName) });
        }
        else if (args.Action == CollectionEdit.Duplicate)
        {
            OsuCollection newCollection = new(_mapCacher) { Name = GetValidCollectionName(args.OrginalName) };
            _collectionEditor.EditCollection(
                CollectionEditArgs.AddCollections([newCollection])
            );
            Beatmaps beatmaps = [.. args.Collections[0].AllBeatmaps()];

            args = CollectionEditArgs.AddBeatmaps(newCollection.Name, beatmaps);
        }

        _collectionEditor.EditCollection(args);
    }

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
        => _collectionEditor.GetCollectionsForBeatmaps(beatmaps);

    public bool IsCollectionNameValid(string name)
        => _collectionNameValidator.IsCollectionNameValid(name);

    public string GetValidCollectionName(string desiredName, List<string> aditionalNames = null)
        => _collectionNameValidator.GetValidCollectionName(desiredName, aditionalNames);
}