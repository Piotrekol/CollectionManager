using System;
using System.Collections.Generic;
using System.Text;
using CollectionManager.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionListGenerator.ListTypes
{
    internal class OsuBbCodeGenerator : GenericGenerator
    {
        protected override string CollectionBodyFormat { get; } = Environment.NewLine + "[*][url={0}]{1} ({2})[/url]";

        protected override string CollectionFooter { get; } = "[/list]";

        protected override string CollectionHeaderTemplate { get; } = Environment.NewLine + Environment.NewLine + "[centre][size=150][b]{0}[/b][/size][/centre][list]";

        protected override string MainFooter { get; } = "[/notice]";

        protected override string MainHeader { get; } = "[notice]";
    }
}