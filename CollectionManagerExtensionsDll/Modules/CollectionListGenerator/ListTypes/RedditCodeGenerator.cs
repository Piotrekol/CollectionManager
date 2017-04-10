using System;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    internal class RedditCodeGenerator : GenericGenerator
    {
        protected override string MainHeader { get; } = "|Name|Difficulty|BeatmapId|★|" + Environment.NewLine + "|---|---|---|:-:|";
        protected override string MainFooter { get; } = "";
        protected override string CollectionBodyFormat { get; } = Environment.NewLine + "{5}|{3}|{0}|{4}★";
        protected override string CollectionFooter { get; } = "";
        protected override string CollectionHeaderTemplate { get; } = "";
    }
}