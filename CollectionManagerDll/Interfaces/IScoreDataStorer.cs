using CollectionManager.DataTypes;

namespace CollectionManager.Interfaces
{
    public interface IScoreDataStorer
    {
        void StartMassStoring();
        void EndMassStoring();
        void Store(Score score);
    }
}