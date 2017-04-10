using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator
{
    public interface IListGenerator
    {
        void StartGenerating();
        void EndGenerating();
        string GetListHeader(Collections collections);
        string GetCollectionBody(Collection collection, Dictionary<int, Beatmaps> mapSets, int collectionNumber);
        string GetListFooter(Collections collections);
    }
}
