using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode;

public static class EnumerableExtenstion
{
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