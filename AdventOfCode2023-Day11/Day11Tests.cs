namespace AdventOfCode2023_Day11;

public class Day11Tests
{
    [Fact]
    public void Example1_SummedGalaxyDistances_374()
    {
        var universe = Universe.From(Example1, 2);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(374);
    }

    [Fact]
    public void Example1_SummedGalaxyDistancesFactor10_1030()
    {
        var universe = Universe.From(Example1, 10);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(1030);
    }

    [Fact]
    public void Example1_SummedGalaxyDistancesFactor100_8410()
    {
        var universe = Universe.From(Example1, 100);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(8410);
    }

    [Fact]
    public void PuzzleInput_SummedGalaxyDistances_10494813()
    {
        var universe = Universe.From(InputLoader.LoadText(), 2);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(10494813);
    }

    [Fact]
    public void PuzzleInput_SummedGalaxyDistancesFactor1000000_840988812853()
    {
        var universe = Universe.From(InputLoader.LoadText(), 1000000);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(840988812853);
    }

    private static long SummedGalaxyDistances(Universe universe)
    {
        var galaxyPairs = universe.Galaxies
            .CartesianSelf()
            .Where(t => t.Item1.GetHashCode() > t.Item2.GetHashCode())
            .ToArray();

        var summedDistance = galaxyPairs
            .Select(t => universe.DistanceBetween(t.Item1, t.Item2))
            .Sum();

        return summedDistance;
    }

    private static string Example1 => 
@"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";
}