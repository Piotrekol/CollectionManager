using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Modules.CollectionsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Misc
{
    internal class CollectionEditArgsExtension : CollectionEditArgs
    {
        public CollectionEditArgsExtension(CollectionEdit action) : base(action)
        {
        }

        public static CollectionEditArgs ExportBeatmaps(Collections collections)
        {
            return new CollectionEditArgsExtension(CollectionEdit.ExportBeatmaps)
            {
                Collections = collections
            };
        }
    }
}
