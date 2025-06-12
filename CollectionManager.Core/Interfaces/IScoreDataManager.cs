namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Types;

public interface IScoreDataManager
{
    Scores Scores { get; }
    void StartMassStoring();
    void EndMassStoring();
    void Clear();
    void Store(IReplay replay);
}