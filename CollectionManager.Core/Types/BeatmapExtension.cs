namespace CollectionManager.Core.Types;
public class BeatmapExtension : Beatmap
{
    public string Name => ToString();

    public bool DataDownloaded { get; set; }
    public bool LocalBeatmapMissing { get; set; }
    public bool LocalVersionDiffers { get; set; }
    public string UserComment { get; set; } = "";

    public DateTimeOffset LastScoreDate { get; set; } = DateTimeOffset.MinValue;
    public int ScoresCount { get; set; }
    public override string Hash { get => Md5; set => Md5 = value; }
}