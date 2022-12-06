using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day06 : BaseDay
{
    public Day06(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var index = FindPacketStart(4);
        TestOutputHelper.WriteLine("The sub packet is {0}", index + 4);
    }

    public override void Part2()
    {
        var index = FindPacketStart(14);
        TestOutputHelper.WriteLine("The sub packet is {0}", index + 14);
    }

    private int FindPacketStart(int length)
    {
        var index = 0;
        while (index + length - 1 < Input.Length)
        {
            var substring = Input.Substring(index, length).DistinctString();
            if (substring.Length == length)
            {
                break;
            }

            index++;
        }

        return index;
    }
}