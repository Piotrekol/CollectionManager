using System;

namespace CollectionManagerExtensionsDll.Enums
{
    [Flags]
    public enum OsuState
    {
        Null = 0,
        Listening = 1,
        Playing = 2,
        FalsePlaying = 4,
        Watching = 8,
        Editing = 16,
        Passed = 32,
        Failed = 64,
    }
}