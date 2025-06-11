namespace CollectionManager.Core.Interfaces;

using System.Collections.Generic;

public interface ICollectionNameValidator
{
    bool IsCollectionNameValid(string name);
    string GetValidCollectionName(string desiredName, IReadOnlyList<string> additionalReservedNames = null);

}