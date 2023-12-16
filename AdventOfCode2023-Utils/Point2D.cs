namespace AdventOfCode2023_Utils;

public sealed record Point2D(int X, int Y)
{
    public int ManhattanDistance(Point2D p) => Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
    public Point2D Add(Point2D p) => new(X + p.X, Y + p.Y);
}