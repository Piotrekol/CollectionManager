namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Types;

public interface IScoreDataManager
{
    Scores Scores { get; }
    Scores GetScores(Beatmap map);
    void StartMassStoring();
    void EndMassStoring();
    void Clear();
    void Store(IReplay replay);
    void UpdateBeatmapsScoreMetadata(IMapDataManager mapDataManager);
}