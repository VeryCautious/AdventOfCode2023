using System.Collections.Immutable;

namespace AdventOfCode2023_Day3;

public class Day3Tests
{
    private const char GearChar = '*';
    private const char EmptyChar = '.';

    [Fact]
    public void SmallExampleEngine_GetNumberWithSymbols_4361()
    {
        var engine = new Engine(SmallExample.AsLines().AsCharArray());
        var summedNumbers = GetNumbers(engine)
            .Where(HasSymbolAround(engine))
            .Sum(n => n.Value);

        summedNumbers.Should().Be(4361);
    }

    [Fact]
    public void Puzzle1Engine_GetNumberWithSymbols_546312()
    {
        var engine = new Engine(InputLoader.LoadLineByLine().AsCharArray());
        var summedNumbers = GetNumbers(engine)
            .Where(HasSymbolAround(engine))
            .Sum(n => n.Value);

        summedNumbers.Should().Be(546312);
    }

    [Fact]
    public void SmallExampleEngine_SummedGearRatio_467835()
    {
        var engine = new Engine(SmallExample.AsLines().AsCharArray());

        var gearRatioSum = GetGearRatios(engine).Sum();

        gearRatioSum.Should().Be(467835);
    }

    [Fact]
    public void Puzzle1Engine_SummedGearRatio_467835()
    {
        var engine = new Engine(InputLoader.LoadLineByLine().AsCharArray());

        var gearRatioSum = GetGearRatios(engine).Sum();

        gearRatioSum.Should().Be(87449461);
    }

    private sealed record Engine(char[][] Chars)
    {
        public int Height => Chars.GetLength(0);
        public int Width => Chars[0].GetLength(0);
        public char At(Coordinate c) => Chars[c.Y][c.X];
        public IEnumerable<Coordinate> Coordinates => 
            CartesianCoords(Height, Width)
            .Select(tuple => new Coordinate(tuple.Item2, tuple.Item1));
    }

    private sealed record Coordinate(int X, int Y);

    private sealed record Number(int Value, Coordinate StartIndex)
    {
        public IImmutableList<Coordinate> Coordinates => 
            Enumerable
            .Range(StartIndex.X, Value.ToString().Length)
            .Select(x => StartIndex with { X = x })
            .ToImmutableList();
    }

    private static IImmutableList<int> GetGearRatios(Engine engine)
    {
        var partNumbers = GetNumbers(engine)
            .SelectMany(n => n.Coordinates.Cartesian(new[] { n }))
            .ToImmutableDictionary(t => t.Item1, t => t.Item2);

        return GetGearCoordinates(engine)
            .Select(GearRatioAt(engine, partNumbers))
            .ToImmutableList();
    }

    private static Func<Number, bool> HasSymbolAround(Engine engine) => num =>
    {
        var coordsOfNumber = num.Coordinates;

        var surroundingCoords = coordsOfNumber
            .SelectMany(GetNeighbors)
            .Except(coordsOfNumber)
            .Where(OnEngineField(engine))
            .ToArray();

        return surroundingCoords
            .Select(engine.At)
            .Any(c => !char.IsDigit(c) && c != EmptyChar);
    };

    private static Func<Coordinate, bool> OnEngineField(Engine engine) => c => 
        c is { X: >= 0, Y: >= 0 } && c.X < engine.Width && c.Y < engine.Height;

    private static IImmutableList<Number> GetNumbers(Engine engine) =>
        engine.Coordinates
            .Where(IsNumberStart(engine))
            .Select(c => new Number(NumberAtCoordinate(engine)(c), c))
            .ToImmutableList();

    private static IImmutableList<Coordinate> GetGearCoordinates(Engine engine) =>
        engine.Coordinates.Where(p => engine.At(p) == GearChar).ToImmutableList();

    private static Func<Coordinate, int> GearRatioAt(Engine engine, IImmutableDictionary<Coordinate, Number> numbers) =>
        c =>
        {
            var adjPartNumbers = GetNeighbors(c)
                .Where(OnEngineField(engine))
                .Where(numbers.ContainsKey)
                .Select(p => numbers[p])
                .Distinct()
                .ToArray();

            if (adjPartNumbers.Length != 2) return 0;

            return adjPartNumbers[0].Value * adjPartNumbers[1].Value;
        };

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