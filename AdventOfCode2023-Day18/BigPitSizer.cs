using System.Collections.Immutable;

namespace AdventOfCode2023_Day18;

public static class BigPitSizer
{
    public static long GetSizeBy(IImmutableList<DigInstruction> instructions) => 
        CalculateVolume(instructions);

    private static long SizeByCorners(IImmutableList<Point2D> corners)
    {
        // https://en.wikipedia.org/wiki/Shoelace_formula
        var u = Enumerable.Range(0, corners.Count - 1).Select(i => (long)corners[i].X * corners[i+1].Y).Sum();
        var v = Enumerable.Range(0, corners.Count - 1).Select(i => (long)corners[i].Y * corners[i+1].X).Sum();

        return (u - v) / 2;
    }

    private static long CalculateVolume(IImmutableList<DigInstruction> instructions)
    {
        var corners = LavaPit.GetCornersBy(instructions);
        var cycledCorners = corners.Append(corners.First()).ToImmutableList();
        var edges = cycledCorners.SlidingWindow().ToImmutableList();

        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        var boundary = edges.Sum(e => e.Item1.ManhattanDistance(e.Item2));
        var area = SizeByCorners(cycledCorners);
        var sum = area + boundary / 2 + 1;
        return sum;
    }
}