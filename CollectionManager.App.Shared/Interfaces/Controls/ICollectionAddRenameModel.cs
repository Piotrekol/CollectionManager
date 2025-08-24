namespace CollectionManager.App.Shared.Interfaces.Controls;

public interface ICollectionAddRenameModel
{
    event EventHandler Submited;
    Func<string, bool> IsCollectionNameValid { get; }
    string OrginalCollectionName { get; }
    string NewCollectionName { get; set; }
    bool NewCollectionNameIsValid { get; set; }
    bool UserCanceled { get; set; }
    void EmitSubmited();
}