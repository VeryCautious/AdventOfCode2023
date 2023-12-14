using System.Collections.Immutable;

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
        var (cycleStart, cycleLength, cycleValues) = FindCycleCycle();

        if (cycleStart > amount)
        {
            return Enumerable.Range(0, amount).Aggregate(this, (platFrom, _) => platFrom.GetAfterCycle());
        }

        var remaining = (amount - cycleStart) % cycleLength;
        return cycleValues[remaining];
    }

    internal PlatForm GetTiltedNorth()
    {
        var movingStones = Positions
            .Where(t => t.Value == MovingStone)
            .Select(t => t.Key)
            .OrderBy(t => t.Y);

        return this with { Positions = movingStones.Aggregate(Positions.ToDictionary(), LetFall).ToImmutableDictionary() };
    }

    private PlatForm GetTiltedSouth() => FlippedNorthSouth().GetTiltedNorth().FlippedNorthSouth();
    private PlatForm GetTiltedEast() => TurnRight().GetTiltedNorth().TurnLeft();
    private PlatForm GetTiltedWest() => TurnLeft().GetTiltedNorth().TurnRight();

    internal PlatForm TurnLeft()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(new Point2D(Height-1-t.Key.Y, t.Key.X), t.Value))
            .ToImmutableDictionary();

        return new PlatForm(Positions: newPositions, Height: Width, Width: Height);
    }

    internal PlatForm TurnRight()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(new Point2D(t.Key.Y, Width-1-t.Key.X), t.Value))
            .ToImmutableDictionary();

        return new PlatForm(Positions: newPositions, Height: Width, Width: Height);
    }

    internal PlatForm FlippedNorthSouth()
    {
        var newPositions = Positions
            .Select(t => new KeyValuePair<Point2D, char>(t.Key with { Y = Height - 1 - t.Key.Y }, t.Value))
            .ToImmutableDictionary();

        return this with { Positions = newPositions };
    }

    private (int cycleStart, int cycleLength, IImmutableList<PlatForm> cycle) FindCycleCycle()
    {
        var hashSet = new Dictionary<string, int>();
        var platFroms = new List<PlatForm>();
        var current = this;

        while (hashSet.TryAdd(current.ToString(), hashSet.Count))
        {
            current = current.GetAfterCycle();
            platFroms.Add(current);
        }

        var startCount = hashSet[current.ToString()];
        return (startCount, hashSet.Count - startCount, platFroms.Skip(startCount-1).ToImmutableList());
    }

    private static Dictionary<Point2D, char> LetFall(Dictionary<Point2D, char> positions, Point2D p)
    {
        for (var i = 1; i <= 100; i++)
        {
            if (p.Y - i >= 0 && positions[p with { Y = p.Y - i }] == EmptySpace) continue;
            
            positions[p] = EmptySpace;
            positions[p with { Y = p.Y + 1 - i }] = MovingStone;
            break;
        }

        return positions;
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