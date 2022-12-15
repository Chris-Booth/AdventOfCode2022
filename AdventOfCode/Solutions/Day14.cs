using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day14 : BaseDay
{
    private readonly Point[] _possibleMoves = {Point.Down, Point.DownLeft, Point.DownRight};

    public Day14(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var walls = Input.ToLines()
            .Select(l => l.Split(" -> "))
            .Select(point => point.Select(xy =>
                {
                    var split = xy.Split(',')
                        .Select(int.Parse)
                        .ToArray();
                    return new Point(split[0], split[1]);
                })
                .ToArray())
            .ToArray();


        var count = GetSettledCount(GetWallPoints(walls), false);
        TestOutputHelper.WriteLine("The count is {0}", count);
    }

    public override void Part2()
    {
        var walls = Input.ToLines()
            .Select(l => l.Split(" -> "))
            .Select(point => point.Select(xy =>
                {
                    var split = xy.Split(',')
                        .Select(int.Parse)
                        .ToArray();
                    return new Point(split[0], split[1]);
                })
                .ToArray())
            .ToArray();


        var count = GetSettledCount(GetWallPoints(walls), true);
        TestOutputHelper.WriteLine("The count is {0}", count);
    }

    private int GetSettledCount(HashSet<Point> walls, bool hasFloor)
    {
        var maxY = walls.Max(p => p.Y);
        var floor = maxY + 2;
        var sands = new HashSet<Point>();

        var complete = false;

        Stack<Point> path = new();
        var current = new Point(500, 0);
        path.Push(current);

        while (!complete)
        {
            while (true)
            {
                var nextPoint = _possibleMoves.Select(delta => current + delta)
                    .FirstOrDefault(tryPoint => !sands.Contains(tryPoint) &&
                                                !walls.Contains(tryPoint) &&
                                                !(hasFloor && tryPoint.Y >= floor));

                if (nextPoint is null)
                {
                    sands.Add(current);
                    if (path.Count == 0)
                    {
                        complete = true;
                        break;
                    }

                    current = path.Pop();
                    break;
                }

                if (!hasFloor && nextPoint.Y >= maxY)
                {
                    complete = true;
                    break;
                }

                path.Push(current);
                current = nextPoint;
            }
        }

        return sands.Count;
    }

    private static HashSet<Point> GetWallPoints(Point[][] walls)
    {
        HashSet<Point> wallPoints = new();
        foreach (var wall in walls)
        {
            for (var i = 0; i < wall.Length - 1; i++)
            {
                var a = wall[i];
                var b = wall[i + 1];
                var dx = b.X - a.X;
                var dy = b.Y - a.Y;

                wallPoints.Add(a);
                // Go from a to b 1 point at a time until b is reached.
                while (a != b)
                {
                    a = dx == 0 ? a with {Y = a.Y + Math.Sign(dy)} : a with {X = a.X + Math.Sign(dx)};
                    wallPoints.Add(a);
                }
            }
        }

        return wallPoints;
    }

    private record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

        public static readonly Point Down = new(0, 1);
        public static readonly Point DownLeft = new(-1, 1);
        public static readonly Point DownRight = new(1, 1);
    };
}