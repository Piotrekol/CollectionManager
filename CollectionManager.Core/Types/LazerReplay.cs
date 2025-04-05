namespace CollectionManager.Core.Types;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;

public class LazerReplay
    : Replay, ILazerReplay
{
    public string LazerVersion { get; set; }
    public int UserId { get; set; }
    public OsuGrade Grade { get; set; }
}