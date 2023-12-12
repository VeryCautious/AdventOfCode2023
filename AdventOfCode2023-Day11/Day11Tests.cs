namespace AdventOfCode2023_Day11;

public class Day11Tests
{
    [Fact]
    public void Example1_SummedGalaxyDistances_374()
    {
        var universe = Universe.From(Example1);

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(374);
    }

    [Fact]
    public void PuzzleInput_SummedGalaxyDistances_10494813()
    {
        var universe = Universe.From(InputLoader.LoadText());

        var summedDistance = SummedGalaxyDistances(universe);

        summedDistance.Should().Be(10494813);
    }

    private static int SummedGalaxyDistances(Universe universe)
    {
        var galaxyPairs = universe.Galaxies
            .CartesianSelf()
            .Where(t => t.Item1.GetHashCode() > t.Item2.GetHashCode())
            .ToArray();

        var summedDistance = galaxyPairs
            .Select(t => t.Item1.ManhattanDistance(t.Item2))
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