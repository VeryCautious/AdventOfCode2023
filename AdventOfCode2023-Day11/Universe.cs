using System.Collections.Immutable;

namespace AdventOfCode2023_Day11;

public sealed record Universe(int Width, int Height, IImmutableList<Point2D> Galaxies)
{
    public static Universe From(string text)
    {
        var universeText = DeGravitate(text);
        var universeLines = universeText.AsLines();
        var height = universeLines.Count;
        var width = universeLines[0].Length;

        var galaxies = universeText
            .CharsBy2dPosition()
            .Where(t => t.Value == '#')
            .Select(t => t.Key)
            .ToImmutableList();

        return new Universe(width, height, galaxies);
    }

    private static string DeGravitate(string text)
    {
        var lines = text.AsLines();

        var emptySpaceLines = lines
            .ZipWithIndex()
            .Where(t => t.Value.All(c => c == '.'))
            .Select(t => t.Index)
            .OrderDescending()
            .ToArray();

        var emptyLine = Enumerable.Repeat('.', lines[0].Length).CollectToString();

        var expandedLines = emptySpaceLines.Aggregate(lines, (ls, index) => ls.Insert(index, emptyLine));

        var emptySpaceColumns = lines
            .SelectMany(line => line.ZipWithIndex())
            .GroupBy(t => t.Index)
            .Where(g => g.All(t => t.Value == '.'))
            .Select(g => g.Key)
            .OrderDescending()
            .ToArray();

        return expandedLines
            .Select(originalLine => emptySpaceColumns.Aggregate(originalLine, (line, index) => line.Insert(index, ".")))
            .CollectToString("\n");
    }
}