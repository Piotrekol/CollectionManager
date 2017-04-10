using System;

namespace Gui.Misc
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}