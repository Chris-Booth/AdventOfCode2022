using System;
using System.Linq;

namespace AdventOfCode;

public static class StringExtensions
{
    public static string[] ToLines(this string input) => input.Split(Environment.NewLine);
    public static string DistinctString(this string input) => new (input.Distinct().ToArray());
    public static int ToInt(this string input) => int.Parse(input);
}