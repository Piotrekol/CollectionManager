namespace CollectionManager.Common;
using System;

public sealed class FormClosingEventArgs : EventArgs
{
    public CloseReason CloseReason { get; }

    public FormClosingEventArgs(CloseReason closeReason)
    {
        CloseReason = closeReason;
    }
}
