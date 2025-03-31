using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CollectionManager.Extensions;
public static class EnumerableExtensions
{
#if NET48 || NETSTANDARD
    [DebuggerStepThrough]
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> keys = [];

        foreach (TSource element in source)
        {
            if (keys.Contains(keySelector(element)))
            {
                continue;
            }    

            _ = keys.Add(keySelector(element));

            yield return element;
        }
    }
#endif
}
