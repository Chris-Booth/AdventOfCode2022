using System.Drawing;

namespace AdventOfCode;

public static class NestedArrayExtensions
{
    public static T GetPoint<T>(this T[][] input, Point point)
    {
        return input[point.X][point.Y];
    }
}