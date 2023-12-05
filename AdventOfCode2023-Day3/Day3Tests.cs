using System.Collections.Immutable;

namespace AdventOfCode2023_Day3;

public class Day3Tests
{

    [Fact]
    public void SmallExampleEngine_GetNumberWithSymbols_4361()
    {
        var engine = new Engine(SmallExample.AsLines().AsCharArray());
        var summedNumbers = GetNumbers(engine)
            .ToArray();

        summedNumbers.Where(HasSymbolAround(engine))
            .Sum(n => n.Value).Should().Be(4361);
    }

    [Fact]
    public void Puzzle1Engine_GetNumberWithSymbols_546312()
    {
        var engine = new Engine(InputLoader.LoadLineByLine().AsCharArray());
        var summedNumbers = GetNumbers(engine)
            .ToArray();
        
        summedNumbers.Where(HasSymbolAround(engine))
            .Sum(n => n.Value).Should().Be(546312);
    }

    private sealed record Engine(char[][] Chars)
    {
        public int Height => Chars.GetLength(0);
        public int Width => Chars[0].GetLength(0);

        public char At(Coordinate c) => Chars[c.Y][c.X];
    }
    private sealed record Coordinate(int X, int Y);
    private sealed record Number(int Value, Coordinate StartIndex);

    private static Func<Number, bool> HasSymbolAround(Engine engine) => num =>
    {
        var coordsOfNumber = Enumerable
            .Range(num.StartIndex.X, num.Value.ToString().Length)
            .Select(x => num.StartIndex with { X = x })
            .ToArray();

        var surroundingCoords = coordsOfNumber
            .SelectMany(GetNeighbors)
            .Except(coordsOfNumber)
            .Where(OnEngineField(engine))
            .ToArray();

        return surroundingCoords
            .Select(engine.At)
            .Any(c => !char.IsDigit(c) && c != '.');
    };

    private static Func<Coordinate, bool> OnEngineField(Engine engine) => c => 
        c is { X: >= 0, Y: >= 0 } && c.X < engine.Width && c.Y < engine.Height;

    private static IImmutableList<Number> GetNumbers(Engine engine)
    {
        return CartesianCoords(engine.Height, engine.Width)
            .Select(tuple => new Coordinate(tuple.Item2, tuple.Item1))
            .Where(IsNumberStart(engine))
            .Select(c => new Number(NumberAtCoordinate(engine)(c), c))
            .ToImmutableList();
    }

    private static IEnumerable<Coordinate> GetNeighbors(Coordinate coordinate) =>
        new[] { -1, 0, 1 }
            .CartesianSelf()
            .Select(c => new Coordinate(coordinate.X + c.Item1, coordinate.Y + c.Item2))
            .Where(c => c.X != 0 || c.Y != 0);

    private static Func<Coordinate, bool> IsNumberStart(Engine engine) => p =>
        char.IsDigit(engine.At(p)) && (p.X == 0 || !char.IsDigit(engine.Chars[p.Y][p.X - 1]));

    private static Func<Coordinate, int> NumberAtCoordinate(Engine engine) => p =>
        int.Parse(new string(engine.Chars[p.Y].Skip(p.X).TakeWhile(char.IsDigit).ToArray()));

    private static string SmallExample => 
@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";
}