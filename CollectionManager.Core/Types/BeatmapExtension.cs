namespace CollectionManager.Core.Types;
public class BeatmapExtension : Beatmap
{
    #region ICeBeatmapProps
    public string Name => ToString();

    public bool DataDownloaded { get; set; }
    public bool LocalBeatmapMissing { get; set; }
    public bool LocalVersionDiffers { get; set; }
    public string UserComment { get; set; } = "";

    #endregion
}