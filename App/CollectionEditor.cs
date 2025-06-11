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
    private readonly CollectionsManager _collectionsManager;
    private readonly ICollectionAddRenameForm _collectionAddRenameForm;
    private readonly MapCacher _mapCacher;

    public CollectionEditor(CollectionsManager collectionsManager,
        ICollectionAddRenameForm collectionAddRenameForm, MapCacher mapCacher)
    {
        _collectionsManager = collectionsManager;
        _collectionAddRenameForm = collectionAddRenameForm;
        _mapCacher = mapCacher;
    }

    public void EditCollection(CollectionEditArgs args)
    {
        if (args.Action is CollectionEdit.Rename or CollectionEdit.Add)
        {
            bool isRenameform = args.Action == CollectionEdit.Rename;

            string newCollectionName = _collectionAddRenameForm
                .GetCollectionName(IsCollectionNameValid, args.CollectionNames.FirstOrDefault(), isRenameform);

            if (newCollectionName == "")
            {
                return;
            }

            switch (args.Action)
            {
                case CollectionEdit.Rename:
                    args = CollectionEditArgs.RenameCollection(args.CollectionNames[0], newCollectionName);
                    break;
                case CollectionEdit.Add:
                    args = CollectionEditArgs.AddCollections([new OsuCollection(_mapCacher) { Name = newCollectionName }]);
                    break;
            }
        }

        _collectionsManager.EditCollection(args);
    }

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
        => _collectionsManager.GetCollectionsForBeatmaps(beatmaps);

    public bool IsCollectionNameValid(string name)
        => _collectionsManager.IsCollectionNameValid(name);

    public string GetValidCollectionName(string desiredName, List<string> aditionalNames = null)
        => _collectionsManager.GetValidCollectionName(desiredName, aditionalNames);
}