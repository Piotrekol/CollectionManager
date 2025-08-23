namespace CollectionManager.WinForms.FastObjectListView;

using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
/// <summary>
/// <see cref="FastListGroupingStrategy"> modified to also sort the groups by user selected column.
/// </summary>
internal abstract class SortableFastListGroupingStrategy : AbstractVirtualGroups
{
    private List<int> _indexToGroupMap;


    /// <summary>
    /// Create groups for FastListView
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override IList<OLVGroup> GetGroups(GroupingParameters parameters)
    {
        if (parameters.ListView is not FastObjectListView listView)
        {
            throw new InvalidOperationException("The SortableFastListGroupingStrategy can only be used with FastObjectListView");
        }

        // Separate the list view items into groups, using the group key as the descrimanent
        (NullableDictionary<object, List<object>> map, int totalObjectCount) = CreateGroups(parameters, listView);

        // Sort items within each group
        OLVColumn primarySortColumn = parameters.SortItemsByPrimaryColumn ? parameters.ListView.GetColumn(0) : parameters.PrimarySort;
        ModelObjectComparer sorter = new(primarySortColumn, parameters.PrimarySortOrder, parameters.SecondarySort, parameters.SecondarySortOrder);

        foreach (object key in map.Keys)
        {
            map[key].Sort(sorter);
        }

        // Make a list of the required groups
        List<OLVGroup> groups = [];

        foreach ((object key, List<object> values) in map)
        {
            string title = parameters.GroupByColumn.ConvertGroupKeyToTitle(key);

            if (!string.IsNullOrEmpty(parameters.TitleFormat))
            {
                int count = map[key].Count;
                string format = count == 1 ? parameters.TitleSingularFormat : parameters.TitleFormat;

                try
                {
                    title = string.Format(CultureInfo.InvariantCulture, format, title, count);
                }
                catch (FormatException)
                {
                    title = "Invalid group format: " + format;
                }
            }

            OLVGroup lvg = new(title)
            {
                Collapsible = listView.HasCollapsibleGroups,
                Key = key,
                SortValue = primarySortColumn.GetValue(map[key][0]) as IComparable,
                Contents = map[key].ConvertAll(listView.IndexOf),
                VirtualItemCount = map[key].Count,
                State = GroupState.LVGS_COLLAPSIBLE
            };

            if (parameters.GroupByColumn.GroupFormatter != null)
            {
                parameters.GroupByColumn.GroupFormatter(lvg, parameters);
            }

            groups.Add(lvg);
        }

        // Sort the groups
        if (parameters.GroupByOrder != System.Windows.Forms.SortOrder.None)
            groups.Sort(parameters.GroupComparer ?? new OLVGroupComparer(parameters.GroupByOrder));

        // Build an array that remembers which group each item belongs to.
        _indexToGroupMap = new List<int>(totalObjectCount);
        _indexToGroupMap.AddRange(new int[totalObjectCount]);

        for (int i = 0; i < groups.Count; i++)
        {
            OLVGroup group = groups[i];
            List<int> members = (List<int>)group.Contents;
            foreach (int j in members)
            {
                _indexToGroupMap[j] = i;
            }
        }

        return groups;
    }

    protected virtual (NullableDictionary<object, List<object>> map, int totalObjectCount) CreateGroups(GroupingParameters parameters, FastObjectListView listView)
    {
        int objectCount = 0;
        NullableDictionary<object, List<object>> map = [];

        foreach (object model in listView.FilteredObjects)
        {
            object key = parameters.GroupByColumn.GetGroupKey(model);

            if (!map.TryGetValue(key, out List<object> list))
            {
                map.Add(key, list = []);
            }

            list.Add(model);
            objectCount++;
        }

        return (map, objectCount);
    }

    public override int GetGroupMember(OLVGroup group, int indexWithinGroup) => (int)group.Contents[indexWithinGroup];

    public override int GetGroup(int itemIndex) => _indexToGroupMap[itemIndex];

    public override int GetIndexWithinGroup(OLVGroup group, int itemIndex) => group.Contents.IndexOf(itemIndex);
}