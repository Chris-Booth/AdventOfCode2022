using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day10 : BaseDay
{
    public Day10(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var nextInterestingPoint = 20;
        var values = new List<int> {1};
        var sum = 0;
        foreach (var line in Input.ToLines())
        {
            values.Add(values.Last());
            sum += values.CalculateInterestPoint(ref nextInterestingPoint);
            if (string.Equals(line, "noop")) continue;

            var valueToAdd = int.Parse(line.Replace("addx ", ""));
            values.Add(values.Last() + valueToAdd);
            sum += values.CalculateInterestPoint(ref nextInterestingPoint);
        }

        TestOutputHelper.WriteLine("Total signal strength is {0}", sum);
    }


    public override void Part2()
    {
        StringBuilder output = new();

        var inputArray = Input.ToLines();
        int instructionIndex = 0, cycleCount = 0, register = 1;
        do
        {
            var pixelIndex = cycleCount % 40;
            output.Append(pixelIndex >= register - 1 && pixelIndex <= register + 1 ? '#' : '.');
            if (++cycleCount % 40 == 0)
            {
                output.AppendLine();
            }

            var line = inputArray[instructionIndex];
            if (line[0] == 'a')
            {
                pixelIndex = cycleCount % 40;
                output.Append(pixelIndex >= register - 1 && pixelIndex <= register + 1 ? '#' : '.');
                if (++cycleCount % 40 == 0)
                {
                    output.AppendLine();
                }

                register += int.Parse(line[4..]);
            }
        } while (++instructionIndex < inputArray.Length);

        TestOutputHelper.WriteLine("{0}", output);
    }
}