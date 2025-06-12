namespace CollectionManager.Core.Types;

using CollectionManager.Core.Modules.FileIo;

public class Score : Replay
{
    public static Score ReadScore(OsuBinaryReader reader, bool minimalLoad = true, int? version = null) => (Score)Read(reader, new Score(), minimalLoad, version);
}