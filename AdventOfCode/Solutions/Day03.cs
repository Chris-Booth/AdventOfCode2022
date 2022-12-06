using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day03 : BaseDay
{
    public Day03(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var lines = Input.ToLines();
        var appearsInBoth = new List<int>();
        foreach (var line in lines)
        {
            var halfPoint = line.Length / 2;
            var (left, right) = (line[..halfPoint], line[halfPoint..]);
            foreach (var current in left.Distinct())
            {
                if (right.Contains(current))
                {
                    appearsInBoth.Add(char.IsUpper(current) ? current - 64 + 26 : current - 96);
                }
            }
        }

        TestOutputHelper.WriteLine("The total sum is {0}", appearsInBoth.Sum());
    }

    public override void Part2()
    {
        var lines = Input.ToLines();
        var (line1, line2) = (string.Empty, string.Empty);
        var appearsInBoth = new List<int>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line1))
            {
                line1 = line.DistinctString();
                continue;
            }

            if (string.IsNullOrWhiteSpace(line2))
            {
                line2 = line.DistinctString();
                continue;
            }

            var line3 = line.DistinctString();

            var current = line1.Join(line2, c => c, c => c, (c1, c2) => c1)
                .Join(line3, c => c, c => c, (c1, c2) => c1).Single();
            appearsInBoth.Add(char.IsUpper(current) ? current - 64 + 26 : current - 96);
            (line1, line2, _) = (string.Empty, string.Empty, string.Empty);
        }

        TestOutputHelper.WriteLine("The total sum is {0}", appearsInBoth.Sum());
    }
}