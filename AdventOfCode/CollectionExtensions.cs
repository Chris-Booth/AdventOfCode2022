using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class CollectionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TryAdd<T>(this ICollection<T> set, T item)
    {
        if (!set.IsReadOnly && !set.Contains(item))
        {
            set.Add(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalculateInterestPoint(this ICollection<int> collection, ref int interestPoint)
    {
        if (collection.Count != interestPoint) return 0;
        var sum = collection.Last() * interestPoint;
        interestPoint += 40;
        return sum;
    }
}