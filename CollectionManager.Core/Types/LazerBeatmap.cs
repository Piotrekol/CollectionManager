namespace CollectionManager.Core.Types;

using System.Linq;

public class LazerBeatmap
    : BeatmapExtension
{
    public string BackgroundFileName { get; set; }
    public string AudioRelativeFilePath => SetFiles.FirstOrDefault(f => f.FileName == Mp3Name)?.RelativeRealmFilePath;
    public string BackgroundRelativeFilePath => SetFiles.FirstOrDefault(f => f.FileName == BackgroundFileName)?.RelativeRealmFilePath;
    public string MapHash { get; set; }
    public LazerFile[] SetFiles { get; set; }
}
