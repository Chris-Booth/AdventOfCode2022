using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day01 : BaseDay
{
    private readonly IEnumerable<int> _elfTotalCalories;

    public Day01(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _elfTotalCalories = ElfTotalCalories();
    }
    
    public override void Part1()
    {
        TestOutputHelper.WriteLine("This max calories of any elf is {0}", _elfTotalCalories.Max());
    }
    
    public override void Part2()
    {
        TestOutputHelper.WriteLine("This total calories of the top three elves is {0}",
            _elfTotalCalories.OrderByDescending(item => item).Take(3).Sum());
    }

    private IEnumerable<int> ElfTotalCalories()
    {
        var lines = Input.ToLines();

        var currentCalories = 0;
        List<int> elfTotalCalories = new();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                elfTotalCalories.Add(currentCalories);
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }

        return elfTotalCalories;
    }
}