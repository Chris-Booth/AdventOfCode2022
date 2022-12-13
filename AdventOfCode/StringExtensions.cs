using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] ToLines(this string input) => input.Split(Environment.NewLine);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DistinctString(this string input) => new (input.Distinct().ToArray());
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this string input) => int.Parse(input);
}