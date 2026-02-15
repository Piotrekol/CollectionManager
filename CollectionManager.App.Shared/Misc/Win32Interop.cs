namespace CollectionManager.App.Shared.Misc;

using System;
using System.Runtime.InteropServices;

internal static class Win32Interop
{
    /// <summary>
    /// Command for <see cref="ShowWindow"/> that activates and displays a window.
    /// If the window is minimized or maximized, the system restores it to its original size and position.
    /// Equivalent to Win32 <c>SW_RESTORE</c>.
    /// </summary>
    internal const int SwRestore = 9;

    [DllImport("user32.dll")]
    internal static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
