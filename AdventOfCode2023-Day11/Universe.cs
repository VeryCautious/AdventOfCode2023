using System.Collections.Immutable;

namespace AdventOfCode2023_Day11;

public sealed record Universe(IImmutableList<Point2D> Galaxies, IImmutableList<int> EmptyRowsByIndex,  IImmutableList<int> EmptyColumnsByIndex, int ExpansionFactor)
{
    public static Universe From(string text, int expansionFactor)
    {
        var galaxies = text
            .CharsBy2dPosition()
            .Where(t => t.Value == '#')
            .Select(t => t.Key)
            .ToImmutableList();

        var lines = text.AsLines();
        
        var emptyRowsByIndex = Enumerable.Range(1, lines.Count)
            .Select(i => lines.Take(i).Count(line => line.All(c => c == '.')))
            .ToImmutableList();

        var emptyColumnsByIndex = Enumerable.Range(1, lines[0].Length).Select(i =>
            lines
                .SelectMany(line => line.ZipWithIndex())
                .GroupBy(t => t.Index)
                .Where(t => t.Key < i)
                .Count(g => g.All(t => t.Value == '.'))
            ).ToImmutableList();

        return new Universe(galaxies, emptyRowsByIndex, emptyColumnsByIndex, expansionFactor);
    }

    public long DistanceBetween(Point2D p1, Point2D p2)
    {
        var emptyRows = EmptyRowsByIndex[Math.Max(p1.Y, p2.Y)] - EmptyRowsByIndex[Math.Min(p1.Y, p2.Y)];
        var emptyColumns = EmptyColumnsByIndex[Math.Max(p1.X, p2.X)] - EmptyColumnsByIndex[Math.Min(p1.X, p2.X)];
        return p1.ManhattanDistance(p2) + (emptyRows + emptyColumns) * (long)(ExpansionFactor - 1);
    }
}