namespace CollectionManager.WinForms.FastObjectListView;

using BrightIdeasSoftware;

internal static class FastObjectListViewExtensions
{
    public static void EnsureSelectionIsVisible(this FastObjectListView list)
    {
        if (list.InvokeRequired)
        {
            list.Invoke(list.EnsureSelectionIsVisible);

            return;
        }

        object obj = list.SelectedObject;

        if (obj is not null)
        {
            list.EnsureModelVisible(obj);
        }
    }
    public static void SelectNextOrFirst(this FastObjectListView list)
    {
        if (list.InvokeRequired)
        {
            list.Invoke(list.SelectNextOrFirst);

            return;
        }

        OLVListItem nextItem = list.GetNextItem(list.SelectedItem);
        nextItem ??= list.GetNextItem(null);
        list.SelectedItem = nextItem;
    }
}
