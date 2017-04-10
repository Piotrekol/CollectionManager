using CollectionManager.DataTypes;

namespace CollectionManager.Interfaces
{
    public interface IMapDataStorer
    {
        void StartMassStoring();
        void EndMassStoring();
        void StoreBeatmap(BeatmapExtension beatmap);
    }
}