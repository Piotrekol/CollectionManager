namespace CollectionManager.WinForms.FastObjectListView;

using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class SortableFastListBeatmapGroupingStrategy : SortableFastListGroupingStrategy
{
    private OLVColumn _fixedColumn;
    private OLVColumn _nameColumn;
    private OLVColumn _setIdColumn;

    public SortableFastListBeatmapGroupingStrategy(OLVColumn fixedColumn, OLVColumn nameColumn, OLVColumn setIdColumn)
    {
        _fixedColumn = fixedColumn;
        _nameColumn = nameColumn;
        _setIdColumn = setIdColumn;
    }

    private string GetFallbackKey(object model)
    {
        var name = _nameColumn.GetStringValue(model);
        var setId = _setIdColumn.GetStringValue(model);

        return $"{setId} {name}";
    }

    protected override (NullableDictionary<object, List<object>> map, int totalObjectCount) CreateGroups(GroupingParameters parameters, FastObjectListView listView)
    {
        (NullableDictionary<object, List<object>> map, int totalObjectCount) = base.CreateGroups(parameters, listView);

        if (map.Keys.Count == 1 && totalObjectCount > 1)
        {
            map.Clear();

            foreach (object model in listView.FilteredObjects)
            {
                object key = GetFallbackKey(model);

                if (!map.TryGetValue(key, out List<object> list))
                {
                    map.Add(key, list = []);
                }

                list.Add(model);
            }

        }

        return (map, totalObjectCount);
    }
}
