using System;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day02 : BaseDay
{
    public Day02(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var score = Input.ToLines().Sum(round => ScoreHands(GetHand(round.First()), GetHand(round.Last())));
        TestOutputHelper.WriteLine("The final score is {0}", score);
    }

    public override void Part2()
    {
        var score = Input.ToLines().Sum(round =>
            ScoreHands(GetHand(round.First()), GetFindHand(GetHand(round.First()), round.Last())));
        TestOutputHelper.WriteLine("The final score is {0}", score);
    }

    private static Hand GetFindHand(Hand elf, char last)
    {
        return last switch
        {
            'X' when elf == Hand.Rock => Hand.Scissors,
            'X' when elf == Hand.Paper => Hand.Rock,
            'X' when elf == Hand.Scissors => Hand.Paper,
            'Y' => elf,
            'Z' when elf == Hand.Rock => Hand.Paper,
            'Z' when elf == Hand.Paper => Hand.Scissors,
            'Z' when elf == Hand.Scissors => Hand.Rock,
            _ => throw new NotImplementedException()
        };
    }

    private static int ScoreHands(Hand elf, Hand you)
    {
        return you switch
        {
            Hand.Rock => 1,
            Hand.Paper => 2,
            Hand.Scissors => 3,
            _ => throw new NotImplementedException()
        } + Winner(elf, you);
    }

    private static int Winner(Hand elf, Hand you)
    {
        if (elf == you)
        {
            return 3;
        }

        if ((elf == Hand.Rock && you == Hand.Paper) ||
            (elf == Hand.Paper && you == Hand.Scissors) ||
            (elf == Hand.Scissors && you == Hand.Rock))
        {
            return 6;
        }

        return 0;
    }

    private static Hand GetHand(char hand)
    {
        return hand switch
        {
            'A' => Hand.Rock,
            'X' => Hand.Rock,
            'B' => Hand.Paper,
            'Y' => Hand.Paper,
            'C' => Hand.Scissors,
            'Z' => Hand.Scissors,
            _ => throw new NotImplementedException()
        };
    }

    private enum Hand
    {
        Rock,
        Paper,
        Scissors
    }
}