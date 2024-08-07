﻿using CollectionManager.DataTypes;

namespace CollectionManager.Interfaces
{
    public interface IScoreDataManager
    {
        Scores Scores { get; }
        void StartMassStoring();
        void EndMassStoring();
        void Clear();
        void Store(IReplay replay);
    }
}