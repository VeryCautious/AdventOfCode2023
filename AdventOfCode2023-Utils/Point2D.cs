namespace AdventOfCode2023_Utils;

public sealed record Point2D(int X, int Y)
{
    public int ManhattanDistance(Point2D p) => Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
    public Point2D Add(Point2D p) => new(X + p.X, Y + p.Y);
    public Point2D Subtract(Point2D p) => new(X - p.X, Y - p.Y);
    public Point2D Times(int factor) => new(X * factor, Y  * factor);
    public Point2D Divide(int factor) => new(X / factor, Y  / factor);
    public Point2D Normalized() => Divide((int)Length);
    public double Length => Math.Sqrt(X * X + Y * Y);

    public IEnumerable<Point2D> GetNeighbours => new[] { -1, 0, 1 }
        .CartesianSelf()
        .Where(t => t.Item1 != 0 || t.Item2 != 0)
        .Select(t => new Point2D(X + t.Item1, Y + t.Item2));

    public IEnumerable<Point2D> GetDirectNeighbours => new[] { -1, 0, 1 }
        .CartesianSelf()
        .Where(t => t.Item1 != 0 || t.Item2 != 0)
        .Where(t => Math.Abs(t.Item1) + Math.Abs(t.Item2) == 1)
        .Select(t => new Point2D(X + t.Item1, Y + t.Item2));
}