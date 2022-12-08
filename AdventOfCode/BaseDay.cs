using System;
using System.IO;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode;

[PublicAPI]
public abstract class BaseDay
{
    protected ITestOutputHelper TestOutputHelper { get; }
    protected string Input { get; set; }

    protected BaseDay(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
        using var file = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inputs", $"{GetType().Name}.txt"));
        using var reader = new StreamReader(file);
        Input = reader.ReadToEnd();
    }

    [Fact]
    public abstract void Part1();

    [Fact]
    public abstract void Part2();
}