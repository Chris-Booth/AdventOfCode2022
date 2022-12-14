using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day12 : BaseDay
{
    private record WayPoint(int X, int Y, int MaxX, int MaxY)
    {
        public virtual bool Equals(WayPoint? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public WayPoint Up => this with {X = X - 1};
        public WayPoint Down => this with {X = X + 1};
        public WayPoint Left => this with {Y = Y - 1};
        public WayPoint Right => this with {Y = Y + 1};
        public bool IsValid => X >= 0 && X < MaxX && Y >= 0 && Y < MaxY;

        public override string ToString() => $"{{X: {X}, Y: {Y}}}";

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }

    public Day12(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var lines = Input.ToLines().Select(line => line.ToCharArray()).ToArray();
        var startLine = lines.Single(line => line.Contains('S'));
        var startPoint = new WayPoint(Array.IndexOf(lines, startLine), Array.IndexOf(startLine, 'S'), lines.Length,
            lines[0].Length);

        PriorityQueue<WayPoint, int> pathFinder = new();
        var visited = new HashSet<WayPoint>
        {
            startPoint
        };
        pathFinder.Enqueue(startPoint, 0);

        while (pathFinder.TryDequeue(out var currentPoint, out var currentDistance))
        {
            var current = GetMarker(lines.GetPoint(currentPoint.ToPoint()));
            if (CheckPoint(currentPoint.Up, lines, currentDistance, current, pathFinder, visited, 'E',
                    next => CanMoveForward(current, next, currentPoint.Up, visited))) break;
            if (CheckPoint(currentPoint.Down, lines, currentDistance, current, pathFinder, visited, 'E',
                    next => CanMoveForward(current, next, currentPoint.Down, visited))) break;
            if (CheckPoint(currentPoint.Left, lines, currentDistance, current, pathFinder, visited, 'E',
                    next => CanMoveForward(current, next, currentPoint.Left, visited))) break;
            if (CheckPoint(currentPoint.Right, lines, currentDistance, current, pathFinder, visited, 'E',
                    next => CanMoveForward(current, next, currentPoint.Right, visited))) break;
        }

        if (pathFinder.Count == 0)
        {
            TestOutputHelper.WriteLine("No path found");
        }
    }

    public override void Part2()
    {
        var lines = Input.ToLines().Select(line => line.ToCharArray()).ToArray();
        var startLine = lines.Single(line => line.Contains('E'));
        var endPoint = new WayPoint(Array.IndexOf(lines, startLine), Array.IndexOf(startLine, 'E'), lines.Length,
            lines[0].Length);

        PriorityQueue<WayPoint, int> pathFinder = new();
        var visited = new HashSet<WayPoint>
        {
            endPoint
        };
        pathFinder.Enqueue(endPoint, 0);

        while (pathFinder.TryDequeue(out var currentPoint, out var currentDistance))
        {
            var current = GetMarker(lines.GetPoint(currentPoint.ToPoint()));
            if (CheckPoint(currentPoint.Up, lines, currentDistance, current, pathFinder, visited, 'a',
                    next => CanMoveBackward(current, next, currentPoint.Up, visited))) break;
            if (CheckPoint(currentPoint.Down, lines, currentDistance, current, pathFinder, visited, 'a',
                    next => CanMoveBackward(current, next, currentPoint.Down, visited))) break;
            if (CheckPoint(currentPoint.Left, lines, currentDistance, current, pathFinder, visited, 'a',
                    next => CanMoveBackward(current, next, currentPoint.Left, visited))) break;
            if (CheckPoint(currentPoint.Right, lines, currentDistance, current, pathFinder, visited, 'a',
                    next => CanMoveBackward(current, next, currentPoint.Right, visited))) break;
        }

        if (pathFinder.Count == 0)
        {
            TestOutputHelper.WriteLine("No path found");
        }
    }

    private bool CheckPoint(WayPoint currentPoint,
        char[][] lines,
        int currentDistance,
        char current,
        PriorityQueue<WayPoint, int> pathFinder,
        HashSet<WayPoint> visited,
        char endPoint,
        Func<char, bool> canMoveForward)
    {
        if (!currentPoint.IsValid) return false;

        var next = GetMarker(lines.GetPoint(currentPoint.ToPoint()));
        switch (canMoveForward(next))
        {
            case true when IsEndPoint(lines.GetPoint(currentPoint.ToPoint()), endPoint, currentDistance):
                return true;
            case false:
                return false;
        }

        visited.Add(currentPoint);
        pathFinder.Enqueue(currentPoint, currentDistance + 1);

        return false;
    }

    private bool IsEndPoint(char current, char c, int currentDistance)
    {
        if (current == c)
        {
            TestOutputHelper.WriteLine("The total distance is {0}", currentDistance + 1);
            return true;
        }

        return false;
    }

    private static char GetMarker(char c) => c switch
    {
        'E' => 'z',
        'S' => 'a',
        _ => c
    };

    private static bool CanMoveForward(char current, char next, WayPoint nextPoint, HashSet<WayPoint> visited) =>
        !visited.Contains(nextPoint) && (next - current <= 1 || current > next);

    private static bool CanMoveBackward(char current, char next, WayPoint nextPoint, HashSet<WayPoint> visited) =>
        !visited.Contains(nextPoint) && (current - next <= 1 || next > current);
}