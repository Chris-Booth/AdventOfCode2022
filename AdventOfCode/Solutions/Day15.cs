using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day15 : BaseDay
{
    private class Sensor
    {
        public int X { get; }
        public int BeaconX { get; }
        public int BeaconY { get; }
        public int DeltaX => Math.Abs(X - BeaconX) + Math.Abs(_y - BeaconY);
        
        private readonly int _y;

        public Sensor(int x, int y, int beaconX, int beaconY)
        {
            X = x;
            _y = y;
            BeaconX = beaconX;
            BeaconY = beaconY;
        }

        public int MinXAtY(int y) => X - DeltaX + Math.Abs(_y - y);
        public int MaxXAtY(int y) => X + DeltaX - Math.Abs(_y - y);
    }

    private static Regex _pointRegex =
        new(
            @"Sensor at x=(?<sensorX>[\-\d]+), y=(?<sensorY>[\-\d]+): closest beacon is at x=(?<beaconX>[\-\d]+), y=(?<beaconY>[\-\d]+)");

    private readonly Sensor[] _sensors;

    public Day15(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _sensors = Input.ToLines().Select(CreatePoint).ToArray();
    }

    public override void Part1()
    {
        const int row = 2000000;
        var minX = _sensors.Min(s => s.X - s.DeltaX);
        var maxX = _sensors.Max(s => s.X + s.DeltaX);

        var score = 0;

        for (var i = minX; i <= maxX; i++)
        {
            var isBeacon = _sensors.Any(s => s.BeaconX == i && s.BeaconY == row);

            if (isBeacon)
                continue;

            if (_sensors.Any(s => i >= s.MinXAtY(row) && i <= s.MaxXAtY(row)))
            {
                score++;
            }
        }

        TestOutputHelper.WriteLine("{0}", score);
    }

    public override void Part2()
    {
        const int maxBound = 4000000;
        for (var y = maxBound; y >= 0; y--)
        {
            var bounds = _sensors.Select(s => new [] {Math.Max(s.MinXAtY(y), 0), Math.Min(s.MaxXAtY(y), maxBound)})
                .Where(e => e[0] <= e[1]).ToList();

            bounds.Sort((a, b) => a[0].CompareTo(b[0]));

            var isMerged = true;

            while (isMerged && bounds.Count > 1)
            {
                isMerged = false;

                if (bounds[0][0] > bounds[1][0] || bounds[0][1] < bounds[1][0]) continue;
                bounds[0][1] = Math.Max(bounds[0][1], bounds[1][1]);
                bounds.RemoveAt(1);
                isMerged = true;
            }

            if (isMerged && bounds[0][0] == 0 && bounds[0][1] == maxBound) continue;
            
            TestOutputHelper.WriteLine("{0}", (BigInteger) (bounds[0][1] + 1) * maxBound + y);
            break;
        }
    }

    private Sensor CreatePoint(string line)
    {
        var match = _pointRegex.Match(line);
        return new Sensor(int.Parse(match.Groups["sensorX"].Value),
            int.Parse(match.Groups["sensorY"].Value),
            int.Parse(match.Groups["beaconX"].Value),
            int.Parse(match.Groups["beaconY"].Value));
    }
}