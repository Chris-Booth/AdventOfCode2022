using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day13 : BaseDay
{
    public Day13(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var chunks = Input.ToLines().Chunk(3)
            .Select(chunk => (new PacketPart(chunk[0]), new PacketPart(chunk[1])))
            .Select((pair, i) => (p: pair, i))
            .Aggregate(0, (tot, v) => tot + (v.p.Item1.CompareTo(v.p.Item2) != 1 ? v.i + 1 : 0));

        TestOutputHelper.WriteLine("The count is {0}", chunks);
    }

    public override void Part2()
    {
        var chunks = Input.ToLines()
            .Where(l => !string.IsNullOrEmpty(l))
            .Append("[[2]]")
            .Append("[[6]]")
            .Select(l => new PacketPart(l))
            .OrderBy(p => p)
            .Select((p, i) => (p, i))
            .Where(v => v.p.ToString() is "[[2]]" or "[[6]]")
            .Aggregate(1, (tot, v) =>  tot * (v.i + 1));
        TestOutputHelper.WriteLine("The count is {0}", chunks);
    }

    private class PacketPart : IComparable<PacketPart>
    {
        private int? Integer { get; }
        private List<PacketPart>? SubParts { get; set; }

        private PacketPart()
        {
            SubParts = new List<PacketPart>();
        }

        private PacketPart(params PacketPart[] subParts)
        {
            SubParts = new List<PacketPart>(subParts);
        }

        private PacketPart(int integer)
        {
            Integer = integer;
        }

        public PacketPart(string packetLine)
        {
            // Skip first [ and last ]
            var charArray = packetLine.ToCharArray()[1..^1];

            Stack<PacketPart> subArrayParts = new();
            SubParts = new();

            for (int i = 0; i < charArray.Length; i++)
            {
                var c = charArray[i];
                switch (c)
                {
                    case '[':
                        subArrayParts.Push(new PacketPart());
                        break;
                    case ']':
                        if (subArrayParts.Count > 1)
                        {
                            var popped = subArrayParts.Pop();
                            subArrayParts.Peek().AddPart(popped);
                        }
                        else
                        {
                            SubParts.Add(subArrayParts.Pop());
                        }

                        break;
                    case ',':
                        break;
                    default:
                        var intStr = new string(charArray.Skip(i).TakeWhile(char.IsDigit).ToArray());
                        var intPart = new PacketPart(int.Parse(intStr));
                        if (subArrayParts.Count > 0)
                        {
                            subArrayParts.Peek().AddPart(intPart);
                        }
                        else
                        {
                            SubParts.Add(intPart);
                        }

                        i += intStr.Length - 1;
                        break;
                }
            }
        }

        public override string ToString()
        {
            return Integer.HasValue ? Integer.ToString()! : $"[{string.Join(",", SubParts!.Select(p => p.ToString()))}]";
        }

        public int CompareTo(PacketPart? r)
        {
            while (true)
            {
                if (r is null)
                {
                    return -1;
                }

                // 2 ints
                if (Integer.HasValue && r.Integer.HasValue)
                {
                    return Integer.Value.CompareTo(r.Integer.Value);
                }

                // 2 lists
                if (SubParts is not null && r.SubParts is not null)
                {
                    foreach (var (lsp, rsp) in SubParts.Zip(r.SubParts))
                    {
                        var comp = lsp.CompareTo(rsp);
                        if (comp != 0)
                        {
                            return comp;
                        }
                    }

                    if (SubParts.Count != r.SubParts.Count)
                    {
                        return SubParts.Count < r.SubParts.Count ? -1 : 1;
                    }

                    return 0;
                }

                // Integer and list
                if (Integer.HasValue) return new PacketPart(new PacketPart(Integer.Value)).CompareTo(r);
                if (r.Integer != null) r = new PacketPart(new PacketPart(r.Integer.Value));
            }
        }

        private void AddPart(PacketPart packetPart)
        {
            SubParts ??= new List<PacketPart>();
            SubParts.Add(packetPart);
        }
    }
}