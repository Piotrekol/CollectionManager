namespace CollectionManager.Common.Interfaces.Controls;
public interface ICollectionRenameView : ICollectionAddView
{
    string OrginalCollectionName { get; set; }
}