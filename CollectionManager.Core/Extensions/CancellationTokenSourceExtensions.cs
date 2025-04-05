namespace CollectionManager.Core.Extensions;
using System;
using System.Threading;

public static class CancellationTokenSourceExtensions
{
    public static bool TryCancel(this CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            cancellationTokenSource?.Cancel();

            return true;
        }
        catch (ObjectDisposedException)
        {
            return false;
        }
    }
}
