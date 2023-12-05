using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode2023_Utils;

public static class RegexExtensions
{
    public static IImmutableList<string> AsList(this Group group, char separator) => 
        group.Value.Split(separator).ToImmutableList();

    public static IImmutableList<int> AsIntList(this Group group, char separator) => 
        group
            .AsList(separator)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(int.Parse)
            .ToImmutableList();

}