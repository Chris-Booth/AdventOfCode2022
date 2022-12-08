using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day08 : BaseDay
{
    public Day08(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    private List<(int x, int y)> _directions = new()
    { 
        (0,-1),
        (1,0),
        (0,1),
        (-1,0),
    };
    
    public override void Part1()
    {
        var map = Input.ToLines().Select(x => x.ToCharArray().ToList()).ToList();
            int rows = map.Count, cols = map[0].Count;
            int biggest, val, visibleCount = 0;
            var visibleMap = new Dictionary<int, HashSet<int>>();

            for(var x = 0; x < cols; ++x)
            {
                biggest = -1;
                for (var y = 0; y < rows; ++y)
                {
                    val = map[y][x];
                    if (val > biggest)
                    {
                        if (MarkVisible(x, y, visibleMap))
                        {
                            ++visibleCount;
                        }
                    }
                    biggest = Math.Max(val, biggest);
                }
                biggest = -1;
                for (var y = rows - 1; y >= 0; --y)
                {
                    val = map[y][x];
                    if (val > biggest)
                    {
                        if (MarkVisible(x, y, visibleMap))
                        {
                            ++visibleCount;
                        }
                    }
                    biggest = Math.Max(val, biggest);
                }
            }
            for (var y = 0; y < rows; ++y)
            {
                biggest = -1;
                for (var x = 0; x < cols; ++x)
                {
                    val = map[y][x];
                    if (val > biggest)
                    {
                        if (MarkVisible(x, y, visibleMap))
                        {
                            ++visibleCount;
                        }
                    }
                    biggest = Math.Max(val, biggest);
                }
                biggest = -1;
                for(var x = cols -1; x >= 0; --x)
                {
                    val = map[y][x];
                    if (val > biggest)
                    {
                        if (MarkVisible(x, y, visibleMap))
                        {
                            ++visibleCount;
                        }
                    }
                    biggest = Math.Max(val, biggest);
                }
            }
            TestOutputHelper.WriteLine($"Visible: {visibleCount}");
    }

    public override void Part2()
    {
        var map = Input.ToLines().Select(x => x.ToCharArray().ToList()).ToList();
        int rows = map.Count, cols = map[0].Count;

        int mapScore, biggest = 0;
        for(int y = 0; y < rows; ++y)
        for(int x = 0; x < cols; ++x)
        {
            mapScore = GetScenicScore((y, x), cols, rows, map);
            if (mapScore > biggest) biggest = mapScore;
        }
        TestOutputHelper.WriteLine($"Biggest: {biggest}");
    }

    private static bool MarkVisible(int x, int y, Dictionary<int, HashSet<int>> visibleMap)
    {
        bool alreadyMapped;
        if (!visibleMap.TryGetValue(y, out var visibleColumns))
        {
            visibleColumns = new HashSet<int>();
            visibleMap.Add(y, visibleColumns);
        }

        if (!(alreadyMapped = visibleColumns.Contains(x)))
        {
            visibleColumns.Add(x);
        }

        return !alreadyMapped;
    }
    
    private int GetScenicScore((int y, int x) loc, int cols, int rows, List<List<char>>? map)
    {
        if (loc.x == 0 || loc.y == 0 || loc.x == cols - 1 || loc.y == rows - 1) return 0;
        List<int> scores = new List<int>();
        int score = 0, maxVal = map[loc.y][loc.x], currentMax = maxVal, localScore, localX, localY, val;
        foreach (var dir in _directions)
        {
            val = -1;
            localScore = 0;
            currentMax = -1;
            localX = loc.x + dir.x;
            localY = loc.y + dir.y;
            while (val < maxVal && localX >= 0 && localX < cols && localY >= 0 && localY < rows)
            {
                val = map[localY][localX];
                ++localScore;
                currentMax = val;
                localX += dir.x;
                localY += dir.y;
            }

            if (localScore > 0) scores.Add(localScore);
        }

        if (scores.Count > 0)
        {
            score = scores.First();
            for (int i = 1; i < scores.Count; ++i)
            {
                score *= scores[i];
            }
        }

        return score;
    }
}