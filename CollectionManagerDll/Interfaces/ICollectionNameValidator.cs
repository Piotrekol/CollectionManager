namespace CollectionManager.Interfaces
{
    public interface ICollectionNameValidator
    {
        bool IsCollectionNameValid(string name);
        string GetValidCollectionName(string desiredName);

    }
}