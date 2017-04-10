namespace GuiComponents.Interfaces
{
    public interface ICollectionAddRenameForm :IForm
    {
        ICollectionRenameView CollectionRenameView { get; }
        bool IsRenameForm { get; set; }
    }
}