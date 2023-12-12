using System.Collections.Immutable;

namespace AdventOfCode2023_Day10;

public sealed record Pipe(int X, int Y, PipeType Type)
{
    public (int X, int Y) Position => (X, Y);

    public static Pipe? From(int x, int y, char symbol)
    {
        var pipeType = symbol switch
        {
            '|' => PipeType.Vertical,
            '-' => PipeType.Horizontal,
            'L' => PipeType.Ne,
            'J' => PipeType.Nw,
            '7' => PipeType.Sw,
            'F' => PipeType.Se,
            'S' => PipeType.Start,
            '.' => PipeType.Ground,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "Not a valid symbol")
        };

        return pipeType == PipeType.Ground ? null : new Pipe(x, y, pipeType);
    }

    public IImmutableList<(int X, int Y)> ConnectedPoints => GetConnectedPoints(this);

    private static IImmutableList<(int X, int Y)> GetConnectedPoints(Pipe pipe)
    {
        var x = pipe.X;
        var y = pipe.Y;
        return pipe.Type switch
        {
            PipeType.Vertical => ImmutableList.Create((x, y - 1), (x, y + 1)),
            PipeType.Horizontal => ImmutableList.Create((x - 1, y), (x + 1, y)),
            PipeType.Ne => ImmutableList.Create((x + 1, y), (x, y + 1)),
            PipeType.Nw => ImmutableList.Create((x - 1, y), (x, y + 1)),
            PipeType.Sw => ImmutableList.Create((x - 1, y), (x, y - 1)),
            PipeType.Se => ImmutableList.Create((x + 1, y), (x, y - 1)),
            PipeType.Start => ImmutableList<(int, int)>.Empty,
            PipeType.Ground => ImmutableList<(int, int)>.Empty,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}

public enum PipeType
{
    Vertical, Horizontal, Ne, Nw, Sw, Se, Start, Ground
}