using System.Collections.Generic;
using CollectionManager.DataTypes;

namespace CollectionManager.Interfaces
{
    public interface IMapDataManager
    {
        void StartMassStoring();
        void EndMassStoring();
        void StoreBeatmap(Beatmap beatmap);
        Beatmap GetByHash(string hash);
        Beatmap GetByMapId(int mapId);
        List<Beatmap> GetByMapString(string artist, string title, string difficulty);
    }
}