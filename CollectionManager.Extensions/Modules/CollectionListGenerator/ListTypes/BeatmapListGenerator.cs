namespace CollectionManager.Extensions.Modules.CollectionListGenerator.ListTypes;

using System;

internal class BeatmapListGenerator : GenericGenerator
{
    protected override string MainHeader { get; } = "Name	Difficulty	★" + Environment.NewLine;
    protected override string MainFooter { get; } = "";
    protected override string CollectionBodyFormat { get; } = "{5}	[{3}]	{4}★" + Environment.NewLine;
    protected override string CollectionFooter { get; } = "";
    protected override string CollectionHeaderTemplate { get; } = "";
}