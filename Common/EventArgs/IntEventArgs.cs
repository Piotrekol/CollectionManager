using System;

namespace Gui.Misc
{
    public class IntEventArgs : EventArgs
    {
        public IntEventArgs(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}