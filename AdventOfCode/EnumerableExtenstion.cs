using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class EnumerableExtenstion
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stack<T> ToStack<T>(this IEnumerable<T> input)
    {
        var stack = new Stack<T>();
        foreach (var x1 in input.Reverse())
        {
            stack.Push(x1);
        }

        return stack;
    }
    

}