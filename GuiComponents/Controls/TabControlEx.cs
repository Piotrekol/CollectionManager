using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GuiComponents.Controls
{
    public class TabControlEx : TabControl
    {
        //tab border fix
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1300 + 40)
            {
                RECT rc = (RECT)m.GetLParam(typeof(RECT));
                rc.Left -= 4;
                rc.Right += 2;
                rc.Top -= 2;
                rc.Bottom += 3;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }

    }
    internal struct RECT { public int Left, Top, Right, Bottom; }
}