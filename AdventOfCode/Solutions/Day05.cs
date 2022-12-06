using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day05 : BaseDay
{
    private readonly IDictionary<int, Stack<string>> _rows;
    private readonly Regex _regex = new("move (?<number>[\\d]+) from (?<from>[\\d]+) to (?<to>[\\d]+)");

    /***************************************
     * [H]                 [Z]         [J] *
     * [L]     [W] [B]     [G]         [R] *
     * [R]     [G] [S]     [J] [H]     [Q] *
     * [F]     [N] [T] [J] [P] [R]     [F] *
     * [B]     [C] [M] [R] [Q] [F] [G] [P] *
     * [C] [D] [F] [D] [D] [D] [T] [M] [G] *
     * [J] [C] [J] [J] [C] [L] [Z] [V] [B] *
     * [M] [Z] [H] [P] [N] [W] [P] [L] [C] *
     *  1   2   3   4   5   6   7   8   9  *
     ***************************************/

    public Day05(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        var row1 = new[] {"H", "L", "R", "F", "B", "C", "J", "M", "1"}.ToStack();
        var row2 = new[] {"D", "C", "Z"}.ToStack();
        var row3 = new[] {"W", "G", "N", "C", "F", "J", "H"}.ToStack();
        var row4 = new[] {"B", "S", "T", "M", "D", "J", "P"}.ToStack();
        var row5 = new[] {"J", "R", "D", "C", "N"}.ToStack();
        var row6 = new[] {"Z", "G", "J", "P", "Q", "D", "L", "W"}.ToStack();
        var row7 = new[] {"H", "R", "F", "T", "Z", "P"}.ToStack();
        var row8 = new[] {"G", "M", "V", "L"}.ToStack();
        var row9 = new[] {"J", "R", "Q", "F", "P", "G", "B", "C"}.ToStack();
        _rows = new Dictionary<int, Stack<string>>
        {
            [1] = row1,
            [2] = row2,
            [3] = row3,
            [4] = row4,
            [5] = row5,
            [6] = row6,
            [7] = row7,
            [8] = row8,
            [9] = row9
        };
    }

    public override void Part1()
    {
        foreach (var line in Input.ToLines())
        {
            var move = ParseMove(line);
            var startStack = _rows[move.From];
            var endStackStack = _rows[move.To];

            for (var i = 0; i < move.Number; i++)
            {
                endStackStack.Push(startStack.Pop());
            }
        }

        var stackTops = string.Join("", _rows.Values.SelectMany(item => item.Pop()));
        TestOutputHelper.WriteLine("The top of the stack are {0}", stackTops);
    }

    public override void Part2()
    {
        foreach (var line in Input.ToLines())
        {
            var move = ParseMove(line);
            var startStack = _rows[move.From];
            var endStackStack = _rows[move.To];
            var temp = new Stack<string>();
            
            for (var i = 0; i < move.Number; i++)
            {
                temp.Push(startStack.Pop());
            }
            
            for (var i = 0; i < move.Number; i++)
            {
                endStackStack.Push(temp.Pop());
            }
        }

        var stackTops = string.Join("", _rows.Values.SelectMany(item => item.Pop()));
        TestOutputHelper.WriteLine("The top of the stack are {0}", stackTops);
    }

    private Move ParseMove(string line)
    {
        var match = _regex.Match(line);
        return new Move(match.Groups["number"].Value.ToInt(), match.Groups["from"].Value.ToInt(),
            match.Groups["to"].Value.ToInt());
    }

    private record Move(int Number, int From, int To)
    {
        public override string ToString()
        {
            return $"move {Number} from {From} to {To}";
        }
    }
}