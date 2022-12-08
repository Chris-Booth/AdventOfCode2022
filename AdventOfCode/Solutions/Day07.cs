using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AdventOfCode.Solutions;

[PublicAPI]
public class Day07 : BaseDay
{
    public Day07(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    public override void Part1()
    {
        var storage = Storage.ParseTerminalOutput(Input.ToLines());

        TestOutputHelper.WriteLine("The total size is {0}", storage.AllDirectories.Where(x => x is {Size: < 100_000})
            .Sum(x => x?.Size ?? 0)
            .ToString());
    }

    public override void Part2()
    {
        var storage = Storage.ParseTerminalOutput(Input.ToLines());

        var needSpace = 30_000_000 - (70_000_000 - storage.Size);
        TestOutputHelper.WriteLine("The total size is {0}", storage.AllDirectories
            .Where(x => x != null && x.Size >= needSpace)
            .Min(x => x?.Size ?? 0)
            .ToString());
    }
}

public class Storage
{
    private readonly Dictionary<string, Directory?> _allDirectories;
    public Directory? Root { get; }

    public Storage()
    {
        _allDirectories = new Dictionary<string, Directory?>();
        Root = new Directory("root");
        _allDirectories.Add(Root.Fullname, Root);
    }

    public IEnumerable<Directory?> AllDirectories => _allDirectories.Values;
    public int Size => Root?.Size ?? 0;

    public void AddDirectory(Directory parent, string name)
    {
        var directory = parent.AddDirectory(name);
        if (directory?.Fullname != null)
            _allDirectories.Add(directory.Fullname, directory);
    }

    public static void AddFile(Directory directory, string name, int size)
    {
        directory.AddFile(name, size);
    }

    internal static Storage ParseTerminalOutput(in string[] inputLines)
        => TerminalOutputParser.Parse(in inputLines);
}

public class Directory
{
    public readonly Directory? Parent;
    private readonly string _name;
    private readonly Dictionary<string, Directory?> _directories;
    private readonly Dictionary<string, File?> _files;

    public Directory(string name) : this(null, name)
    {
    }

    private Directory(Directory? parent, string name)
    {
        _directories = new Dictionary<string, Directory?>();
        _files = new Dictionary<string, File?>();
        Parent = parent;
        _name = name;
        Size = 0;
    }

    public int Size { get; private set; }
    public string Fullname => Parent != null ? $"{Parent.Fullname}\\{_name}" : _name;

    private void IncreaseSize(int size)
    {
        Size += size;
        Parent?.IncreaseSize(size);
    }

    internal Directory? AddDirectory(string name)
    {
        if (_directories.ContainsKey(name)) return null;

        var directory = new Directory(this, name);
        _directories.Add(name, directory);
        return directory;
    }

    internal void AddFile(string name, int size)
    {
        if (_files.ContainsKey(name)) return;
        var file = new File(name, size);
        _files.Add(name, file);

        IncreaseSize(size);
    }

    internal Directory? GetDirectory(string name) => _directories[name];

    public override string ToString() => $"{_name} {Size}";
}

public class File
{
    public File(string name, int size)
    {
        _name = name;
        _size = size;
    }

    private readonly string _name;
    private readonly int _size;

    public override string ToString() => $"{_name} {_size}";
}

public static class TerminalOutputParser
{
    private const string ListAll = "$ ls";
    private const string ChangeToRootDirectory = "$ cd /";
    private const string ChangeToUpDirectory = "$ cd ..";
    private const string ChangeToDirectoryPrefix = "$ cd";
    private static readonly int ChangeToDirectoryPrefixJumpCount = ChangeToDirectoryPrefix.Length + 1;
    private const string DirectoryPrefix = "dir";
    private static readonly int DirectoryPrefixJumpCount = DirectoryPrefix.Length + 1;

    public static Storage Parse(in string[] inputLines)
    {
        var storage = new Storage();

        var currentDirectory = storage.Root;
        foreach (var inputLineRaw in inputLines)
        {
            var inputLine = inputLineRaw.AsSpan();

            if (inputLine.StartsWith(ListAll))
            {
                continue;
            }

            if (inputLine.StartsWith(ChangeToRootDirectory))
            {
                currentDirectory = storage.Root;
                continue;
            }

            if (inputLine.StartsWith(ChangeToUpDirectory))
            {
                currentDirectory = currentDirectory?.Parent ?? storage.Root;
                continue;
            }

            if (inputLine.StartsWith(ChangeToDirectoryPrefix)) //$ cd a
            {
                var directoryName = inputLine[ChangeToDirectoryPrefixJumpCount..].ToString();
                currentDirectory = currentDirectory?.GetDirectory(directoryName) ?? currentDirectory;
                continue;
            }

            if (inputLine.StartsWith(DirectoryPrefix)) //dir e
            {
                var directoryName = inputLine[DirectoryPrefixJumpCount..].ToString();
                if (currentDirectory != null) storage.AddDirectory(currentDirectory, directoryName);
                continue;
            }

            var spaceIndex = inputLine.IndexOf(' ');
            var size = int.Parse(inputLine[..spaceIndex]);
            var fileName = inputLine[spaceIndex..].ToString();
            if (currentDirectory != null) Storage.AddFile(currentDirectory, fileName, size);
        }

        return storage;
    }
}