using System;
using CollectionManager.Enums;
using CollectionManager.Modules.FileIO.OsuLazerDb.RealmModels.Enums;

namespace CollectionManager.Interfaces;

public interface ILazerReplay
    : IReplay
{
    string LazerVersion { get; set; }
    int UserId { get; set; }
    OsuGrade Grade { get; set; }
}