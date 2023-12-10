using System.Collections.Immutable;

namespace AdventOfCode2023_Utils;

public static class StringExtensions
{
    public static IImmutableList<string> AsLines(this string text) => text
        .Split('\n')
        .Select(s => s.Replace("\r", ""))
        .ToImmutableList();

    public static IImmutableList<int> IntegersAsList(this string text, char separator = ' ') => text
        .Split(separator)
        .Select(s => s.Trim())
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Where(s => s.All(c => char.IsDigit(c) || c == '-'))
        .Select(int.Parse)
        .ToImmutableList();

    public static string CollectToString(this IEnumerable<string> strings, string separator = "") =>
        string.Join(separator, strings);
}