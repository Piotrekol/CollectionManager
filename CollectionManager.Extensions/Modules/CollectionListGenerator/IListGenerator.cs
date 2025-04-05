namespace CollectionManager.Extensions.Modules.CollectionListGenerator;

using CollectionManager.Core.Types;
using System.Collections.Generic;

public interface IListGenerator
{
    void StartGenerating();
    void EndGenerating();
    string GetListHeader(OsuCollections collections);
    string GetCollectionBody(IOsuCollection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber);
    string GetListFooter(OsuCollections collections);
}
