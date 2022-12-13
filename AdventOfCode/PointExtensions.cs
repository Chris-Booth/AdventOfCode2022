using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class PointExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MoveHead(this ref Point point, string direction)
    {
        switch (direction)
        {
            case "U":
                point.Y++;
                break;
            case "D":
                point.Y--;
                break;
            case "L":
                point.X--;
                break;
            case "R":
                point.X++;
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Follow(this ref Point tail, Point head)
    {
        var stepX = head.X - tail.X;
        var stepY = head.Y - tail.Y;

        if (Math.Abs(stepX) <= 1 && Math.Abs(stepY) <= 1)
        {
            // touching target - no move
            return;
        }

        // Move max of one towards target
        tail.X += Math.Sign(stepX);
        tail.Y += Math.Sign(stepY);
    }
}