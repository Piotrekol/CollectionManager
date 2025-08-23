namespace CollectionManager.WinForms;

using System.Drawing;
using System.Windows.Forms;

internal static class WindowsFormsExtensions
{
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
                e.Graphics.DrawLine(Pens.Silver, left, CurrBottom, left + halfWidth, CurrBottom + 5);
            }
        }
    }

    public static void SplitterPaintHorizontal(object sender, PaintEventArgs e)
    {
        if (sender is SplitContainer s)
        {
            int left = 5;
            int right = s.Width - 5;
            int top = s.SplitterDistance;
            int bottom = top + s.SplitterWidth - 1;
            e.Graphics.DrawLine(Pens.Silver, left, top, right, top);
            e.Graphics.DrawLine(Pens.Silver, left, bottom, right, bottom);
            int halfHeight = s.SplitterWidth / 2;
            for (int CurrRight = right; CurrRight > left; CurrRight -= 10)
            {
                e.Graphics.DrawLine(Pens.Silver, CurrRight, top, CurrRight - 5, top + halfHeight);
            }
        }
    }
}