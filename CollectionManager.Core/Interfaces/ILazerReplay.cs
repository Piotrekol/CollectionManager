namespace CollectionManager.Core.Interfaces;

using CollectionManager.Core.Enums;

public interface ILazerReplay
    : IReplay
{
    string LazerVersion { get; set; }
    int UserId { get; set; }
    OsuGrade Grade { get; set; }
}