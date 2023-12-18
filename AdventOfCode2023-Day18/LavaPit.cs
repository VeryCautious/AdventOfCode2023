using System.Collections.Immutable;

namespace AdventOfCode2023_Day18;

public sealed record LavaPit(ISet<Point2D> Pit)
{
    public static LavaPit From(IImmutableList<DigInstruction> instructions)
    {
        var current = new Point2D(0, 0);
        var border = new HashSet<Point2D>{ current };

        foreach (var instruction in instructions)
        {
            var next = current.Add(instruction.Direction);
            border.UnionWith(GetNodesBetween(current, next));
            current = next;
        }

        return new LavaPit(Filled(border));
    }

    public static ISet<Point2D> GetCornersBy(IImmutableList<DigInstruction> instructions)
    {
        var current = new Point2D(0, 0);
        var cornerPoints = new HashSet<Point2D>{ current };

        foreach (var instruction in instructions)
        {
            var next = current.Add(instruction.Direction);
            cornerPoints.Add(next);
            current = next;
        }

        return cornerPoints;
    }

    private static ISet<Point2D> Filled(ISet<Point2D> border)
    {
        var fills = new HashSet<Point2D>(border);
        var startPoint = new Point2D(1, 1);
        var fringe = new Queue<Point2D>();
        fringe.Enqueue(startPoint);

        while (fringe.Count > 0)
        {
            var current = fringe.Dequeue();
            foreach (var neighbour in current.GetDirectNeighbours)
            {
                if (fills.Add(neighbour))
                {
                    fringe.Enqueue(neighbour);
                }
            }
        }

        return fills;
    }

    private static IEnumerable<Point2D> GetNodesBetween(Point2D start, Point2D end)
    {
        if (start.X == end.X)
        {
            return Enumerable
                .Range(Math.Min(start.Y, end.Y), Math.Abs(start.Y - end.Y) + 1)
                .Select(y => start with { Y = y });
        }
        else if(start.Y == end.Y)
        {
            return Enumerable
                .Range(Math.Min(start.X, end.X), Math.Abs(start.X - end.X) + 1)
                .Select(x => start with { X = x });
        }
        throw new ArgumentException();
    }
}