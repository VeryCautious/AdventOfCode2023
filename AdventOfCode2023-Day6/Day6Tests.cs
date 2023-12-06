using System.Collections.Immutable;

namespace AdventOfCode2023_Day6;

public class Day6Tests
{
    [Fact]
    public void SmallExample_WinConditionsAgainst_288()
    {
        var records = ParseInput(SmallExample.AsLines());

        var winConditions = records.Select(WinConditionsAgainst).Aggregate(1L, (x,y) => x * y);

        records.Should().HaveCount(3);
        winConditions.Should().Be(288);
    }

    [Fact]
    public void PuzzleInput_WinConditionsAgainst_2612736()
    {
        var records = ParseInput(InputLoader.LoadLineByLine());

        var winConditions = records.Select(WinConditionsAgainst).Aggregate(1L, (x,y) => x * y);

        records.Should().HaveCount(4);
        winConditions.Should().Be(2612736);
    }

    [Fact]
    public void SmallExample_WinConditionsAgainstSingleRace_71503()
    {
        var recordHolder = ParseInputForSingleRace(SmallExample.AsLines());

        var winConditions = WinConditionsAgainst(recordHolder);

        winConditions.Should().Be(71503);
    }

    [Fact]
    public void PuzzleInput_WinConditionsAgainstSingleRace_29891250()
    {
        var recordHolder = ParseInputForSingleRace(InputLoader.LoadLineByLine());

        var winConditions = WinConditionsAgainst(recordHolder);

        winConditions.Should().Be(29891250);
    }

    private static long WinConditionsAgainst(RecordHolder recordHolder)
    {
        var (x1, x2) = GetXIntersections(-1, recordHolder.Time, -recordHolder.Distance);
        return x2 + 1 - x1;
    }

    private static (long, long) GetXIntersections(long a, long b, long c)
    { 
        var first = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        var second = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

        var (x1, x2) = ((long)Math.Floor(first + 1), (long)Math.Ceiling(second - 1));

        return (x1, x2);
    }

    private static RecordHolder ParseInputForSingleRace(IImmutableList<string> inpLines)
    {
        var time = inpLines[0]["Time:".Length..].Replace(" ", "");
        var distance = inpLines[1]["Distance:".Length..].Replace(" ", "");

        return new RecordHolder(long.Parse(time), long.Parse(distance));
    }

    private static IImmutableList<RecordHolder> ParseInput(IImmutableList<string> inpLines)
    {
        var times = inpLines[0].IntegersAsList();
        var distances = inpLines[1].IntegersAsList();

        return times.Zip(distances)
            .Select(tuple => new RecordHolder(tuple.First, tuple.Second))
            .ToImmutableList();
    }

    private sealed record RecordHolder(long Time, long Distance);

    private static string SmallExample => 
@"Time:      7  15   30
Distance:  9  40  200";
}