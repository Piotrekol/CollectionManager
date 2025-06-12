namespace CollectionManager.Extensions.Modules.Filter;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;

public class FilterGroupData
{
    public Mods Mods { get; set; } = Mods.Nm;
    public PlayMode PlayMode { get; set; } = PlayMode.Osu;
}
