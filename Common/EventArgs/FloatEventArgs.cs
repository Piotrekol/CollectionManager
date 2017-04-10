using System;

namespace Gui.Misc
{
    public class FloatEventArgs : EventArgs
    {
        public FloatEventArgs(float value)
        {
            Value = value;
        }

        public float Value { get; set; }
    }
}