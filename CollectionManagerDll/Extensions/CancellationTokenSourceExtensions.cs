using System;
using System.Threading;

namespace CollectionManager.Extensions;

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
