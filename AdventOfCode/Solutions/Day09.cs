using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day09 : BaseDay
{
    private Point _currentHead;
    private Point _currentTail;
    private readonly HashSet<Point> _tailVisited;

    public Day09(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _currentHead = new Point(0, 0);
        _currentTail = new Point(0, 0);
        _tailVisited = new HashSet<Point>
        {
            _currentTail
        };
    }

    public override void Part1()
    {
        foreach (var move in Input.ToLines().Select(item => item.Split(' ')))
        {
            var (direction, distance) = (move[0], int.Parse(move[1]));
            for (var i = 0; i < distance; i++)
            {
                _currentHead.MoveHead(direction);
                _currentTail.Follow(_currentHead);
                _tailVisited.TryAdd(_currentTail);
            }
        }

        TestOutputHelper.WriteLine("Total squares Tail Visited {0}", _tailVisited.Count);
    }


    public override void Part2()
    {
        //Lets make 9 knots
        var knots = Enumerable.Range(0, 9).Select(_ => new Point()).ToArray();
        foreach (var move in Input.ToLines().Select(item => item.Split(' ')))
        {
            var (direction, distance) = (move[0], int.Parse(move[1]));
            for (var i = 0; i < distance; i++)
            {
                _currentHead.MoveHead(direction);
                knots[0].Follow(_currentHead);
                // Other knots follow prev knot
                for (var x = 1; x < knots.Length; x++)
                {
                    knots[x].Follow(knots[x - 1]);
                }
                // Visit tail pos
                _tailVisited.TryAdd(knots[^1]);
            }
        }
        TestOutputHelper.WriteLine("Total squares Tail Visited {0}", _tailVisited.Count);
    }
}