using CollectionManager.DataTypes;

namespace CollectionManager.Interfaces
{
    public interface IScoreDataStorer
    {
        Scores Scores { get; }
        void StartMassStoring();
        void EndMassStoring();
        void Clear();
        void Store(Score score);
    }
}