namespace CollectionManager.WinForms;

using BrightIdeasSoftware;
using System.Drawing;
using System.Windows.Forms;

internal static class Helpers
{

    public static void EnsureSelectionIsVisible(this FastObjectListView list)
    {
        if (list.InvokeRequired)
        {
            list.Invoke(list.EnsureSelectionIsVisible);
            return;
        }

        object obj = list.SelectedObject;
        if (obj != null)
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
    public static void SplitterPaint(object sender, PaintEventArgs e)
    {
        if (sender is SplitContainer s)
        {
            int top = 5;
            int bottom = s.Height - 5;
            int left = s.SplitterDistance;
            int right = left + s.SplitterWidth - 1;
            e.Graphics.DrawLine(Pens.Silver, left, top, left, bottom);
            e.Graphics.DrawLine(Pens.Silver, right, top, right, bottom);
            int halfWidth = s.SplitterWidth / 2;

            for (int CurrBottom = bottom; CurrBottom > top; CurrBottom -= 10)
            {
                //e.Graphics.DrawLine(Pens.Silver, left, top, left, bottom);
                e.Graphics.DrawLine(Pens.Silver, left, CurrBottom, left + halfWidth, CurrBottom + 5);
            }
        }
    }
}