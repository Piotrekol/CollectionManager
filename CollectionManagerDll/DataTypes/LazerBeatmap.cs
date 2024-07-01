using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManager.DataTypes;
public class LazerBeatmap 
    : BeatmapExtension
{
    public string AudioRelativeFilePath { get; set; }
    public string BackgroundRelativeFilePath { get; set; }
}
