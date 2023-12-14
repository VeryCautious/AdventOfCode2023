using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode2023_Day14;

public sealed record PlatForm(IImmutableDictionary<Point2D, char> Positions, int Width, int Height)
{
    private const char MovingStone = 'O';
    private const char EmptySpace = '.';

    public static PlatForm From(string text)
    {
        var lines = text.AsLines();
        return new PlatForm(text.CharsBy2dPosition(), lines[0].Length, lines.Count);
    }

    public PlatForm GetAfterCycle()
    {
        return GetTiltedNorth().GetTiltedWest().GetTiltedSouth().GetTiltedEast();
    }

    public PlatForm GetAfterCycle(int amount)
    {
        var (cycleStart, cycleLength, fixPoint) = FindCycleCycle();

        int remaining;
        PlatForm startPoint;

        if (cycleStart > amount)
        {
            remaining = amount;
            startPoint = this;
        }
        else
        {
            remaining = (amount - cycleStart) % cycleLength;
            startPoint = fixPoint;
        }

        return Enumerable.Range(0, remaining).Aggregate(startPoint, (platFrom, _) => platFrom.GetAfterCycle());
    }

    public PlatForm GetTiltedNorth()
    {
        var movingStones = Positions
            .Where(t => t.Value == MovingStone)
            .Select(t => t.Key)
            .OrderBy(t => t.Y);

        return this with { Positions = movingStones.Aggregate(Positions, LetFall) };
    }

    private PlatForm GetTiltedSouth() => FlippedNorthSouth().GetTiltedNorth().FlippedNorthSouth();
    private PlatForm GetTiltedEast() => TurnRight().GetTiltedNorth().TurnLeft();
    private PlatForm GetTiltedWest() => TurnLeft().GetTiltedNorth().TurnRight();

    public PlatForm TurnLeft()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(new Point2D(Height-1-t.Key.Y, t.Key.X), t.Value))
            .ToImmutableDictionary();

        return new PlatForm(Positions: newPositions, Height: Width, Width: Height);
    }

    public PlatForm TurnRight()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(new Point2D(t.Key.Y, Width-1-t.Key.X), t.Value))
            .ToImmutableDictionary();

        return new PlatForm(Positions: newPositions, Height: Width, Width: Height);
    }

    public PlatForm FlippedNorthSouth()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(t.Key with { Y = Height - 1 - t.Key.Y }, t.Value))
            .ToImmutableDictionary();

        return this with { Positions = newPositions };
    }

    private (int cycleStart, int cycleLength, PlatForm fixPoint) FindCycleCycle()
    {
        var hashSet = new Dictionary<string, int>();
        var current = this;
        var sw = new Stopwatch();
        var times = new List<double>();

        while (hashSet.TryAdd(current.ToString(), hashSet.Count))
        {
            sw.Restart();
            current = current.GetAfterCycle();
            sw.Stop();
            times.Add(TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds);
            var average = times.Average(); //4.5sec
            var x = 1;
        }

        var startCount = hashSet[current.ToString()];
        return (startCount, hashSet.Count - startCount, current);
    }

    private static IImmutableDictionary<Point2D, char> LetFall(IImmutableDictionary<Point2D, char> positions, Point2D p)
    {
        var remainingRow = positions
            .Where(t => t.Key.X == p.X && t.Key.Y < p.Y)
            .OrderBy(t => t.Key.Y)
            .ToArray();

        var fallingOverPoints = remainingRow
            .Reverse()
            .TakeWhile(t => t.Value == EmptySpace)
            .ToArray();

        if (fallingOverPoints.Length == 0)
        {
            return positions;
        }

        return positions.SetItems([new(fallingOverPoints[^1].Key, MovingStone), new(p, EmptySpace)]);
    }

    public override string ToString() =>
        Positions
            .GroupBy(t => t.Key.Y)
            .Select(line => line.OrderBy(t => t.Key.X)
                .Select(t => t.Value).CollectToString())
            .CollectToString("\n");

    public long CalculateLoad() =>
        Positions
            .Where(t => t.Value == MovingStone)
            .Select(t => Height - t.Key.Y)
            .Sum();
}