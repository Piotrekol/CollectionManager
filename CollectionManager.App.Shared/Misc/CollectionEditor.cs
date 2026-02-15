namespace CollectionManager.App.Shared.Misc;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.Linq;

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

    public void EditCollection(CollectionEditArgs args) => EditCollection([args]);

    public void EditCollection(IReadOnlyList<CollectionEditArgs> args)
    {
        if (args is null || args.Count == 0)
        {
            return;
        }

        List<CollectionEditArgs> processedArgs = new(args.Count);

        foreach (CollectionEditArgs arg in args)
        {
            CollectionEditArgs processedArg = ProcessUIActionIfNeeded(arg);

            if (processedArg is not null)
            {
                processedArgs.Add(processedArg);
            }
        }

        if (processedArgs.Count > 0)
        {
            _collectionsManager.EditCollection(processedArgs);
        }
    }

    private CollectionEditArgs? ProcessUIActionIfNeeded(CollectionEditArgs args)
    {
        if (args.Action is not (CollectionEdit.Rename or CollectionEdit.Add))
        {
            return args;
        }

        string newCollectionName = _collectionAddRenameForm.GetCollectionName(IsCollectionNameValid, args.CollectionNames.FirstOrDefault(), isRenameForm: args.Action == CollectionEdit.Rename);

        if (string.IsNullOrEmpty(newCollectionName))
        {
            return null;
        }

        return args.Action switch
        {
            CollectionEdit.Rename => CollectionEditArgs.RenameCollection(args.CollectionNames[0], newCollectionName),
            CollectionEdit.Add => CollectionEditArgs.AddCollections([new OsuCollection(_mapCacher) { Name = newCollectionName }]),
            _ => args
        };
    }

    public OsuCollections GetCollectionsForBeatmaps(Beatmaps beatmaps)
        => _collectionsManager.GetCollectionsForBeatmaps(beatmaps);

    public bool IsCollectionNameValid(string name)
        => _collectionsManager.IsCollectionNameValid(name);

    public string GetValidCollectionName(string desiredName, IReadOnlyList<string> additionalReservedNames = null)
        => _collectionsManager.GetValidCollectionName(desiredName, additionalReservedNames);
}