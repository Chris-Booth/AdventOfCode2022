using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day04 : BaseDay
{
    public Day04(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var overlaps = 0;
        foreach (var line in Input.ToLines())
        {
            var pair = line.Split(',');
            var (left, right) = (pair[0].Split('-'), pair[1].Split('-'));

            var leftStart = int.Parse(left[0]);
            var leftEnd = int.Parse(left[1]) + 1;

            var rightStart = int.Parse(right[0]);
            var rightEnd = int.Parse(right[1]) + 1;

            var leftRange = Enumerable.Range(leftStart, leftEnd - leftStart).ToArray();
            var rightRange = Enumerable.Range(rightStart, rightEnd - rightStart).ToArray();

            if (leftRange.All(item => rightRange.Contains(item)) ||
                rightRange.All(item => leftRange.Contains(item)))
                overlaps++;
        }

        TestOutputHelper.WriteLine("The number of overlaps are {0}", overlaps);
    }

    public override void Part2()
    {
        var overlaps = 0;
        foreach (var line in Input.ToLines())
        {
            var pair = line.Split(',');
            var (left, right) = (pair[0].Split('-'), pair[1].Split('-'));

            var leftStart = int.Parse(left[0]);
            var leftEnd = int.Parse(left[1]) + 1;

            var rightStart = int.Parse(right[0]);
            var rightEnd = int.Parse(right[1]) + 1;

            var leftRange = Enumerable.Range(leftStart, leftEnd - leftStart).ToArray();
            var rightRange = Enumerable.Range(rightStart, rightEnd - rightStart).ToArray();

            if (leftRange.Any(item => rightRange.Contains(item)) ||
                rightRange.Any(item => leftRange.Contains(item)))
                overlaps++;
        }

        TestOutputHelper.WriteLine("The number of overlaps are {0}", overlaps);
    }
}