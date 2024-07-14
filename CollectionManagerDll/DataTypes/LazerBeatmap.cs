namespace CollectionManager.DataTypes;

public class LazerBeatmap
    : BeatmapExtension
{
    public string AudioRelativeFilePath { get; set; }
    public string BackgroundRelativeFilePath { get; set; }
    public string MapHash { get; set; }
}
