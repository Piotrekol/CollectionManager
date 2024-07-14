using CollectionManager.Enums;
using CollectionManager.Interfaces;

namespace CollectionManager.DataTypes;

public class LazerReplay
    : Replay, ILazerReplay
{
    public string LazerVersion { get; set; }
    public int UserId { get; set; }
    public OsuGrade Grade { get; set; }
}