namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Types;

public interface IMapDataManager
{
    Beatmaps Beatmaps { get; }
    void StartMassStoring();
    void EndMassStoring();
    void StoreBeatmap(Beatmap beatmap);
    Beatmap GetByHash(string hash);
    Beatmap GetByMapId(int mapId);
}