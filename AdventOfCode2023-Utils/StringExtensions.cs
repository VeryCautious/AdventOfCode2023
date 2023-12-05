using System.Collections.Immutable;

namespace AdventOfCode2023_Utils;

public static class StringExtensions
{
    public static IImmutableList<string> AsLines(this string text) => text
        .Split('\n')
        .Select(s => s.Replace("\r", ""))
        .ToImmutableList();
}